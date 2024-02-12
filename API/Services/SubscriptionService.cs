/*using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Entities.Identity;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;
using AsparagusN.Interfaces;
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

    public async Task<UserPlan?> CreateSubscriptionAsync(NewSubscriptionDto subscriptionDto, AppUser user)
    {
        if (await CheckExistingSubscriptionPlanForUserAsync(user.Id, subscriptionDto.PlanType))
            return null;

        var plan = _mapper.Map<UserPlan>(subscriptionDto);
        plan.User = user;

        var ok = AddPlanDaysToPlan(subscriptionDto, plan);
        if (subscriptionDto.SelectedDrinks != null)
        {
            var drinkIds = subscriptionDto.SelectedDrinks.Select(x => x.Id).ToList();
            if (await CheckExistingDrinks(drinkIds, subscriptionDto.PlanType))
            {
                ok &= await AddDrinksToDaysPlan(subscriptionDto.SelectedDrinks, plan);
            }
            else return null;
        }

        if (subscriptionDto.SelectedExtras != null)
        {
            if (await CheckExistingExtras(subscriptionDto.SelectedExtras.Select(x => x.Id).ToList(),
                    subscriptionDto.PlanType))
            {
                ok &= await AddExtrasToDaysPlan(subscriptionDto.SelectedExtras, plan);
            }
        }
    }

    public async Task<bool> CheckExistingSubscriptionPlanForUserAsync(int userId, PlanTypeEnum planType)
    {
        var plan = await _unitOfWork.Repository<UserPlan>().GetQueryable()
            .Where(x => x.PlanType == planType).FirstOrDefaultAsync();

        return plan != null;
    }

    private async Task<bool> AddDrinksToDaysPlan(List<Item> userSelectedDrinks, UserPlan plan)
    {
        try
        {
            var drinkIds = userSelectedDrinks.Select(x => x.Id).ToList();
            var adminDrinks = await _unitOfWork.Repository<AdminSelectedDrink>().GetQueryable()
                .Where(x => x.PlanTypeEnum == plan.PlanType && drinkIds.Contains(x.Id)).Include(x => x.Drink)
                .ToListAsync();

            var drinksToAdd = adminDrinks.Select(drink => _mapper.Map<UserSelectedDrink>(drink.Drink)).ToList();

            foreach (var day in plan.Days)
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

    private async Task<bool> AddExtrasToDaysPlan(List<Item> userSelectedExtras, UserPlan plan)
    {
        try
        {
            var extraIds = userSelectedExtras.Select(x => x.Id).ToList();
            var adminExtras = await _unitOfWork.Repository<AdminSelectedExtraOption>().GetQueryable()
                .Where(x => x.PlanTypeEnum == plan.PlanType && extraIds.Contains(x.Id)).Include(x => x.ExtraOption)
                .ToListAsync();

            var extrasToAdd = new List<UserSelectedExtraOption>();
            foreach (var extra in userSelectedExtras)
            {
                var dr = adminExtras.First(x => x.ExtraOptionId == extra.Id);

                var toAdd = _mapper.Map<UserSelectedExtraOption>(dr.ExtraOption);
                toAdd.Weight = extra.Quantity;
                extrasToAdd.Add(toAdd);
            }

            foreach (var day in plan.Days)
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


    private bool AddPlanDaysToPlan(NewSubscriptionDto subscriptionDto, UserPlan plan)
    {
        try
        {
            var days = new List<UserPlanDay>();

            while (subscriptionDto.Duration > 0)
            {
                subscriptionDto.Duration--;
                var day = subscriptionDto.StartDate.AddDays(1);

                days.Add(new UserPlanDay
                {
                    Day = day,
                });
                subscriptionDto.StartDate = day;
            }

            plan.Days = days;
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
            throw;
        }
    }


    private async Task<bool> CheckExistingDrinks(ICollection<int> ids, PlanTypeEnum planType)
    {
        var adminDrinks = await _unitOfWork.Repository<AdminSelectedDrink>().GetQueryable()
            .Where(x => x.PlanTypeEnum == planType && ids.Contains(x.Id)).ToListAsync();

        return adminDrinks.Count == ids.Count;
    }

    private async Task<bool> CheckExistingExtras(ICollection<int> ids, PlanTypeEnum planType)
    {
        var adminExtras = await _unitOfWork.Repository<AdminSelectedExtraOption>().GetQueryable()
            .Where(x => x.PlanTypeEnum == planType && ids.Contains(x.Id)).ToListAsync();

        return adminExtras.Count == ids.Count;
    }
}*/