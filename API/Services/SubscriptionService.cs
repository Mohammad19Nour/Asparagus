﻿using AsparagusN.Data;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.Meal;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Enums;
using AsparagusN.Helpers;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AsparagusN.Specifications.AdminPlanSpecifications;
using AsparagusN.Specifications.UserSpecifications;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IPaymentService _paymentService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;

    public SubscriptionService(IPaymentService paymentService,IUnitOfWork unitOfWork, IMapper mapper, IValidationService validationService)
    {
        _paymentService = paymentService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validationService = validationService;
    }

    public async Task<UserPlan?> GetUserSubscriptionAsync(AppUser user, PlanTypeEnum planType)
    {
        try
        {
                var spec = new UserPlanWithMealsDrinksAndExtrasSpecification(user.Id, planType);
            return await _unitOfWork.Repository<UserPlan>().GetEntityWithSpec(spec);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<UserPlan>> GetAllUserSubscriptionsAsync(AppUser user)
    {
        try
        {
            var spec = new BaseSpecification<UserPlan>(x =>
                x.AppUserId == user.Id && x.StartDate.Date >= HelperFunctions.WeekStartDay() &&
                x.StartDate <= HelperFunctions.WeekEndDay());

            return (await _unitOfWork.Repository<UserPlan>().ListWithSpecAsync(spec)).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<(decimal? Price, string Message)> GetPriceForUpdate(UpdateSubscriptionDto subscription,
        AppUser user)
    {
        try
        {
            var plan = await GetUserSubscriptionAsync(user, subscription.PlanType);
            if (plan == null)
                return (null, "You don't have a subscription with this plan type");

            var tmp = plan.Price;
            var t = await Update((UpdateSubscriptionDto)subscription, user, plan);

            if (!t.Success) return (null, t.Message);
            return (plan.Price - tmp, "Ok");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return (null, "Exception happened");
            throw;
        }
    }

    public async Task<(decimal? Price, string Message)> GetPriceForCreate(NewSubscriptionDto subscription, AppUser user)
    {
        try
        {
            var plan = await GetUserSubscriptionAsync(user, subscription.PlanType);

            if (plan != null)
                return (null, "You have a subscription with this plan type");

            plan = new UserPlan();
            var res = await Add((NewSubscriptionDto)subscription, user, plan);

            if (!res.Succes) return (null, res.Message);
            return (plan.Price, "Ok");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return (null, "Exception happened");
            throw;
        }
    }


    public async Task<(UserPlan? createdPlan, string Message)> CreateSubscriptionAsync(
        NewSubscriptionDto subscriptionDto, AppUser user, bool seed = false)
    {
        try
        {
            if (subscriptionDto.PlanType == PlanTypeEnum.CustomMealPlan)
                return (null, "Can't subscribe with custom plan");
            user = await _unitOfWork.Repository<AppUser>()
                .GetEntityWithSpec(new UserWithAddressSpecification(user!.Email));
            var plan = await GetUserSubscriptionAsync(user, subscriptionDto.PlanType);

            if (plan != null)
                return (null, "You have a subscription with this plan type");

            if (!HelperFunctions.CheckExistAddress(user.HomeAddress) &&
                !HelperFunctions.CheckExistAddress(user.WorkAddress))
                return (null, "You have to provide at least one address");
            var planPoints = (await _unitOfWork.Repository<PlanType>().ListAllAsync())
                .First(x => x.PlanTypeE == subscriptionDto.PlanType).Points;
            plan = new UserPlan();
            if (seed)
                plan.CreatedDate = DateTime.Today.AddDays(-3);

            var result = await Add(subscriptionDto, user, plan, seed);
            if (!result.Succes) return (null, result.Message);

            if (!seed)
            {
                
                if (subscriptionDto.TransactionId == null)
                    return (null, "you have to pay first");
                
                var resultPayment = await _paymentService
                    .CheckPaymentStatus(subscriptionDto.TransactionId,(double) plan.Price);
                
                if (!resultPayment.Success)
                    return (null,resultPayment.Message);
                plan.TransactionId = subscriptionDto.TransactionId;

            }
            user.IsMealPlanMember = true;
            user.IsNormalUser = false;
            plan.NumberOfRemainingSnacks = plan.NumberOfSnacks;

            plan.User.LoyaltyPoints += planPoints;
            
            _unitOfWork.Repository<UserPlan>().Add(plan);

            if (await _unitOfWork.SaveChanges()) return (plan, "Success");
            return (null, "Failed to add user plan");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return (null, "Exception happened");
            throw;
        }
    }

    public async Task<(UserPlan?, string Message)> UpdateSubscription(UpdateSubscriptionDto subscriptionDto,
        AppUser user)
    {
        try
        {
            if (subscriptionDto.PlanType == PlanTypeEnum.CustomMealPlan)
                return (null, "Can't update custom plan");
            var plan = await GetUserSubscriptionAsync(user, subscriptionDto.PlanType);

            if (plan == null)
                return (null, "You don't have a subscription with this plan type");

            var tmp = plan.Price;
            var result = await Update(subscriptionDto, user, plan);

            if (!result.Success) return (null, result.Message);

            if (subscriptionDto.TransactionId == null)
                return (null, "you have to pay first");
            
            var resultPayment = await _paymentService
                .CheckPaymentStatus(subscriptionDto.TransactionId,(double) (plan.Price - tmp));
                
            if (!resultPayment.Success)
                return (null,resultPayment.Message);
          
            plan.TransactionId = subscriptionDto.TransactionId;
            
            _unitOfWork.Repository<UserPlan>().Update(plan);

            if (await _unitOfWork.SaveChanges())
                return (plan, "ok");

            return (null, "Something happened");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return (null, "Exception happened");
            throw;
        }
    }


    private async Task<(bool Success, string Message)> AddDrinksToDaysPlan(List<int>? drinkIds, List<UserPlanDay> days,
        PlanTypeEnum planType)
    {
        try
        {
            if (drinkIds == null) return (true, "");
            var exist = await _validationService.CheckExistingDrinks(drinkIds, planType);

            if (!exist)
                return (false, "One or more drinks not exist");

            var adminDrinks = await GetAdminSelectedDrinksWithIds(drinkIds, planType);

            var drinksToAdd = adminDrinks.Select(drink => _mapper.Map<UserSelectedDrink>(drink.Drink)).ToList();

            for (int j = 0; j < drinkIds.Count; j++)
            {
                days[j].SelectedDrinks.Add(drinksToAdd[j]);
            }

            return (true, "");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return (false, "Exception happened");
            throw;
        }
    }

    private async Task<List<AdminSelectedDrink>> GetAdminSelectedDrinksWithIds(List<int> drinkIds,
        PlanTypeEnum planType)
    {
        try
        {
            var adminDrinks = await _unitOfWork.Repository<AdminSelectedDrink>().GetQueryable()
                .Where(x => x.PlanTypeEnum == planType && drinkIds.Contains(x.Id)).Include(x => x.Drink)
                .ToListAsync();
            return adminDrinks;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<(bool Success, string Message)> AddExtrasOptionToDaysPlan(List<Item>? userSelectedExtras,
        List<UserPlanDay> days,
        PlanTypeEnum planType, ExtraOptionType optionType)
    {
        try
        {
            if (userSelectedExtras == null) return (true, "");

            if (!await _validationService.CheckExistingExtras(userSelectedExtras.Select(x => x.Id).ToList(), planType,
                    optionType))
                return (false, $"One or more {optionType.ToString()} not exist");

            var extraIds = userSelectedExtras.Select(x => x.Id).ToList();

            var adminExtras = await GetAdminSelectedExtraOptionsWithIds(planType, extraIds, optionType);

            var extrasToAdd = new List<UserSelectedExtraOption>();
            foreach (var extra in userSelectedExtras)
            {
                var dr = adminExtras.First(x => x.Id == extra.Id);

                var toAdd = _mapper.Map<UserSelectedExtraOption>(dr.ExtraOption);
                toAdd.Weight = extra.Weight;
                toAdd.Price = dr.ExtraOption.Price / dr.ExtraOption.Weight * toAdd.Weight;
                extrasToAdd.Add(toAdd);
            }

            for (var j = 0; j < extrasToAdd.Count; j++)
            {
                days[j].SelectedExtraOptions.Add(extrasToAdd[j]);
            }

            return (true, "");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return (false, "Exception happened");
            throw;
        }
    }

    private async Task<List<AdminSelectedExtraOption>> GetAdminSelectedExtraOptionsWithIds(PlanTypeEnum planType,
        List<int> extraIds, ExtraOptionType optionType)
    {
        try
        {
            var spec = new AdminSelectedExtrasAndSaladsSpecification(optionType, planType);
            var adminExtras = await _unitOfWork.Repository<AdminSelectedExtraOption>().GetQueryable()
                .Where(x => x.PlanTypeEnum == planType && extraIds.Contains(x.Id)).Include(x => x.ExtraOption)
                .ToListAsync();
            return adminExtras;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private List<UserPlanDay>? GetPlanDays(DateTime startDate, int duration)
    {
        try
        {
            var days = new List<UserPlanDay>();

            while (duration > 0)
            {
                duration--;
                var day = startDate;

                days.Add(new UserPlanDay
                {
                    Day = day,
                });
                startDate = day.AddDays(1).Date;
            }


            return days;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
            throw;
        }
    }


    private async Task<(bool Success, string Message)> AddAllergies(List<int>? selectedAllergyIds, UserPlan plan)
    {
        try
        {
            if (selectedAllergyIds == null) return (true, "");

            var allergies = await _getAllergies(selectedAllergyIds);

            if (allergies == null)
                return (false, "One or more allergy not exist");

            plan.Allergies = _mapper.Map<List<UserPlanAllergy>>(allergies);
            return (true, "");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return (false, "Exception happened");
            throw;
        }
    }

    private async Task<List<Allergy>?> _getAllergies(List<int> allergyIds)
    {
        try
        {
            allergyIds = allergyIds.Distinct().ToList();
            var dbAllergies = await _unitOfWork.Repository<Allergy>().ListAllAsync();
            dbAllergies = dbAllergies.Where(x => allergyIds.Contains(x.Id)).ToList();

            return (dbAllergies.Count == allergyIds.Count) ? dbAllergies.ToList() : null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
            throw;
        }
    }

    private async Task<(bool Success, string Message)> Update(UpdateSubscriptionDto subscriptionDto, AppUser user,
        UserPlan plan)
    {
        try
        {
            if (DateTime.Today.AddDays(1) >= plan.EndDate())
                return (false,
                    "Can't update your plan now... you have to wait until end of this subscription and subscribe again ");
            if (subscriptionDto.Duration < plan!.Duration)
                return (false, "The updated duration should be greater than or equal the previous one");

            if (plan.NumberOfSnacks > subscriptionDto.NumberOfSnacks)
                return (false, "Snacks must be greater than or equal to the original");

            if (plan.NumberOfMealPerDay > subscriptionDto.NumberOfMealPerDay)
                return (false, "Number of meal per day must be greater than or equal to the original");

            var validation = await _validationService.IsValidSubscriptionDto(subscriptionDto,0);

            var numberOfUpdatedDays = subscriptionDto.Duration - plan.Duration;
            var numberOfUpdatedSnacks = subscriptionDto.NumberOfSnacks - plan.NumberOfSnacks;
            var numberOfUpdatedMealsPerDay = subscriptionDto.NumberOfMealPerDay - plan.NumberOfMealPerDay;

            subscriptionDto.Duration -= plan.Duration;
            plan.NumberOfSnacks = subscriptionDto.NumberOfSnacks;
            plan.NumberOfMealPerDay = subscriptionDto.NumberOfMealPerDay;
            plan.NumberOfRemainingSnacks += numberOfUpdatedSnacks;

            if (!validation.Success) return (false, validation.Message);


            Console.WriteLine(plan.EndDate());
            var days = new List<UserPlanDay>();
            var updateDuration = true;

            if (subscriptionDto.Duration == 0)
            {
                updateDuration = false;
                var firstDayCanUpdate = DateTime.Now.AddDays(2).Date;
                days = await _unitOfWork.Repository<UserPlanDay>().GetQueryable()
                    .Include(c => c.UserPlan)
                    .Where(x => x.Day >= firstDayCanUpdate && x.UserPlan.AppUserId == user.Id &&
                                subscriptionDto.PlanType == x.UserPlan.PlanType)
                    .ToListAsync();

                if (days.Count == 0)
                    return (false, "You don't have days that can be updated");
            }
            else
            {
                days = GetPlanDays(plan.EndDate(), subscriptionDto.Duration);
            }

            if (days == null)
                return (false, "Something happened");

            plan.Duration += subscriptionDto.Duration;

            var tmpDays = new List<UserPlanDay>();
            days.ForEach(x => tmpDays.Add(new UserPlanDay()));

            var addProcess = await AddDrinksToDaysPlan(subscriptionDto.SelectedDrinks, tmpDays, plan.PlanType);
            if (!addProcess.Success) return (false, addProcess.Message);

            addProcess = await AddExtrasOptionToDaysPlan(subscriptionDto.SelectedSalads,
                tmpDays, plan.PlanType, ExtraOptionType.Salad);

            if (!addProcess.Success) return (false, addProcess.Message);

            addProcess = await AddExtrasOptionToDaysPlan(subscriptionDto.SelectedExtras,
                tmpDays, plan.PlanType, ExtraOptionType.Nuts);

            if (!addProcess.Success) return (false, addProcess.Message);

            var updatedPlanDays = await _unitOfWork.Repository<UserPlanDay>().GetQueryable()
                .Where(x => x.Day >= DateTime.Now.AddDays(2).Date && x.UserPlanId == plan.Id).ToListAsync();

            plan.Price +=
                numberOfUpdatedMealsPerDay * updatedPlanDays.Count * 10; // update meals per day for previous days


            if (numberOfUpdatedDays > 0)
                plan.Price += numberOfUpdatedDays * plan.NumberOfMealPerDay * 10;

            plan.Price += numberOfUpdatedSnacks * 10;

            foreach (var day in tmpDays)
            {
                day.DeliveryLocation = HelperFunctions.CheckExistAddress(user.HomeAddress)
                    ? user.HomeAddress
                    : user.WorkAddress;
                foreach (var drink in day.SelectedDrinks)
                    plan.Price += drink.Price;

                foreach (var extra in day.SelectedExtraOptions)
                {
                    plan.Price += extra.Price;
                }
            }

            for (int j = 0; j < days.Count; j++)
            {
                foreach (var drink in tmpDays[j].SelectedDrinks)
                {
                    days[j].SelectedDrinks.Add(drink);
                }

                foreach (var extra in tmpDays[j].SelectedExtraOptions)
                {
                    days[j].SelectedExtraOptions.Add(extra);
                }
            }

            if (updateDuration)
                foreach (var day in days)
                {
                    day.DeliveryLocation = HelperFunctions.CheckExistAddress(user.HomeAddress)
                        ? user.HomeAddress
                        : user.WorkAddress;
                    plan.Days.Add(day);
                }

            Console.WriteLine(plan.EndDate());
            return (true, "ok");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return (false, "Exception happened");
            throw;
        }
    }

    private async Task<(bool Succes, string Message)> Add(NewSubscriptionDto subscriptionDto, AppUser user,
        UserPlan plan, bool seed = false)
    {
        try
        {
            var validate = await _validationService.IsValidSubscriptionDto(subscriptionDto,0);
            if (!validate.Success)
                return (false, validate.Message);
            subscriptionDto.StartDate = subscriptionDto.StartDate.Date;
            if (DateTime.Today.AddDays(2).Date >= subscriptionDto.StartDate.Date && !seed)
                subscriptionDto.StartDate = DateTime.Today.AddDays(3).Date;
            else if (seed) subscriptionDto.StartDate = DateTime.Now.AddDays(-1);

            _mapper.Map(subscriptionDto, plan);

            plan.User = user;

            var days = GetPlanDays(subscriptionDto.StartDate, subscriptionDto.Duration);
            if (days == null)
                return (false, "Something happened");

            foreach (var day in days)
            {
                day.DeliveryLocationId = HelperFunctions.CheckExistAddress(user.HomeAddress)
                    ? user.HomeAddressId
                    : user.WorkAddressId;
            }

            plan.Days = days;

            var addProcess = await AddDrinksToDaysPlan(subscriptionDto.SelectedDrinks, plan.Days!, plan.PlanType);
            if (!addProcess.Success) return (false, addProcess.Message);

            addProcess = await AddExtrasOptionToDaysPlan(subscriptionDto.SelectedSalads,
                plan.Days!, plan.PlanType, ExtraOptionType.Salad);

            if (!addProcess.Success) return (false, addProcess.Message);

            addProcess = await AddExtrasOptionToDaysPlan(subscriptionDto.SelectedExtras,
                plan.Days!, plan.PlanType, ExtraOptionType.Nuts);

            if (!addProcess.Success) return (false, addProcess.Message);

            addProcess = await AddAllergies(subscriptionDto.Allergies, plan);
            if (!addProcess.Success) return (false, addProcess.Message);

            foreach (var day in plan.Days)
            {
                foreach (var drink in day.SelectedDrinks)
                    plan.Price += drink.Price;

                foreach (var extra in day.SelectedExtraOptions)
                    plan.Price += extra.Price;
            }

            plan.Price += plan.NumberOfSnacks * 10;
            plan.Price += plan.Duration * plan.NumberOfMealPerDay * 10;

            return (true, "ok");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return (false, "Exception happened");
            throw;
        }
    }
}