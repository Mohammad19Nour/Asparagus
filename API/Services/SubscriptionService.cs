using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.DrinksDtos;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Entities.Identity;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;
using AsparagusN.Interfaces;
using AsparagusN.Specifications.UserSpecifications;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public SubscriptionService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<(UserPlan? createdPlan, string Message)> CreateSubscriptionAsync(
        NewSubscriptionDto subscriptionDto, AppUser user)
    {
        if (await CheckExistingSubscriptionPlanForUserAsync(user.Id, subscriptionDto.PlanType))
            return (null, "You have a subscription with this plan type");

        var plan = _mapper.Map<UserPlan>(subscriptionDto);
        plan.User = user;

        var days = GetPlanDaysToPlan(subscriptionDto.StartDate, subscriptionDto.Duration);

        bool ok = days != null;
        if (ok) plan.Days = days;

        if (subscriptionDto.SelectedDrinks != null)
        {
            var drinkIds = subscriptionDto.SelectedDrinks.Select(x => x.Id).ToList();
            if (await CheckExistingDrinks(drinkIds, subscriptionDto.PlanType))
            {
                ok &= await AddDrinksToDaysPlan(subscriptionDto.SelectedDrinks, plan.Days!, plan.PlanType);
            }
            else return (null, "One or more drinks not exist");
        }

        if (subscriptionDto.SelectedExtras != null)
        {
            if (await CheckExistingExtras(subscriptionDto.SelectedExtras.Select(x => x.Id).ToList(),
                    subscriptionDto.PlanType))
            {
                ok &= await AddExtrasToDaysPlan(subscriptionDto.SelectedExtras, plan.Days!, plan.PlanType);
            }
            else return (null, "One or more extra options not exist");
        }

        _unitOfWork.Repository<UserPlan>().Add(plan);

        if (await _unitOfWork.SaveChanges()) return (plan, "Success");
        return (null, "Failed to add user plan");
    }

    public async Task<bool> CheckExistingSubscriptionPlanForUserAsync(int userId, PlanTypeEnum planType)
    {
        return await GetUserPlanAsync(userId, planType) != null;
    }

    public async Task<(UserPlan?, string Message)> UpdateDuration(int userId, PlanTypeEnum planType, int duration)
    {
        if (!await CheckExistingSubscriptionPlanForUserAsync(userId, planType))
            return (null, "You don't have a subscription with this plan type");

        var plan = await GetUserPlanAsync(userId, planType);
        if (duration <= plan!.Duration) return (null, "The updated duration should be greater than the previous one");

        duration = duration - plan.Duration;

        var days = GetPlanDaysToPlan(plan.EndDate(), duration);

        if (days == null)
        {
            return (null, "Something happened");
        }

        plan.Duration += duration;
        var lastDay = plan.Days.LastOrDefault();

        if (lastDay != null)
        {
            var drinks = lastDay.SelectedDrinks.ToList();
            var extras = lastDay.SelectedExtraOptions.ToList();

            foreach (var day in days)
            {
                foreach (var drink in drinks)
                {
                    day.SelectedDrinks.Add(drink);
                }

                foreach (var extra in extras)
                {
                    day.SelectedExtraOptions.Add(extra);
                }
            }
        }

        foreach (var day in days) plan.Days.Add(day);
        _unitOfWork.Repository<UserPlan>().Update(plan);

        if (await _unitOfWork.SaveChanges())
            return (plan, "ok");

        return (null, "Something happened");
    }

    private async Task<bool> AddDrinksToDaysPlan(List<Item> userSelectedDrinks, List<UserPlanDay> days,
        PlanTypeEnum planType)
    {
        try
        {
            var drinkIds = userSelectedDrinks.Select(x => x.Id).ToList();
            var adminDrinks = await _unitOfWork.Repository<AdminSelectedDrink>().GetQueryable()
                .Where(x => x.PlanTypeEnum == planType && drinkIds.Contains(x.Id)).Include(x => x.Drink)
                .ToListAsync();

            var drinksToAdd = adminDrinks.Select(drink => _mapper.Map<UserSelectedDrink>(drink.Drink)).ToList();

            foreach (var day in days)
            {
                day.SelectedDrinks = drinksToAdd;
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
            throw;
        }
    }

    private async Task<bool> AddExtrasToDaysPlan(List<Item> userSelectedExtras, List<UserPlanDay> days,
        PlanTypeEnum planType)
    {
        try
        {
            var extraIds = userSelectedExtras.Select(x => x.Id).ToList();
            var adminExtras = await _unitOfWork.Repository<AdminSelectedExtraOption>().GetQueryable()
                .Where(x => x.PlanTypeEnum == planType && extraIds.Contains(x.Id)).Include(x => x.ExtraOption)
                .ToListAsync();

            var extrasToAdd = new List<UserSelectedExtraOption>();
            foreach (var extra in userSelectedExtras)
            {
                var dr = adminExtras.First(x => x.Id == extra.Id);

                var toAdd = _mapper.Map<UserSelectedExtraOption>(dr.ExtraOption);
                toAdd.Weight = extra.Quantity;
                extrasToAdd.Add(toAdd);
            }

            foreach (var day in days)
            {
                day.SelectedExtraOptions = extrasToAdd;
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
            throw;
        }
    }


    private List<UserPlanDay>? GetPlanDaysToPlan(DateTime startDate, int duration)
    {
        try
        {
            var days = new List<UserPlanDay>();

            while (duration > 0)
            {
                duration--;
                var day = startDate.AddDays(1);

                days.Add(new UserPlanDay
                {
                    Day = day,
                });
                startDate = day;
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


    private async Task<bool> CheckExistingDrinks(ICollection<int> ids, PlanTypeEnum planType)
    {
        var adminDrinks = await _unitOfWork.Repository<AdminSelectedDrink>().GetQueryable()
            .Where(x => x.PlanTypeEnum == planType && ids.Contains(x.Id)).ToListAsync();

        return adminDrinks.Count == ids.Count;
    }

    private async Task<UserPlan?> GetUserPlanAsync(int userId, PlanTypeEnum planType)
    {
        var spec = new UserPlanWithMealsDrinksAndExtrasSpecification(userId, planType);
        return await _unitOfWork.Repository<UserPlan>().GetEntityWithSpec(spec);
    }

    private async Task<bool> CheckExistingExtras(ICollection<int> ids, PlanTypeEnum planType)
    {
        var adminExtras = await _unitOfWork.Repository<AdminSelectedExtraOption>().GetQueryable()
            .Where(x => x.PlanTypeEnum == planType && ids.Contains(x.Id)).ToListAsync();

        return adminExtras.Count == ids.Count;
    }
}