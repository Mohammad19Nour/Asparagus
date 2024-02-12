using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Entities.Identity;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers.User;

public class SubscriptionController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public SubscriptionController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<ActionResult> CreateSubscription(NewSubscriptionDto subscriptionDto)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "user not found"));

        var plan = await _unitOfWork.Repository<UserPlan>().GetQueryable()
            .Where(x => x.PlanType == subscriptionDto.PlanType).FirstOrDefaultAsync();

        if (plan != null)
            return Ok(new ApiResponse(400, "You have a subscription with this system"));

        plan = _mapper.Map<UserPlan>(subscriptionDto);
        plan.User = user;
        var ok = _addPlanDaysToPlan(subscriptionDto, plan);
        if (subscriptionDto.SelectedDrinks != null)
            ok &= await _addDrinksToDaysPlan(subscriptionDto.SelectedDrinks, plan);
        if (subscriptionDto.SelectedExtras != null)
            ok &= await _addExtrasToDaysPlan(subscriptionDto.SelectedExtras, plan);

        if (!ok) return Ok(new ApiResponse(400, "Something happened"));
        _unitOfWork.Repository<UserPlan>().Add(plan);

        await _unitOfWork.SaveChanges();
        return Ok(plan);
    }

    private async Task<bool> _addDrinksToDaysPlan(List<Item> userSelectedDrinks, UserPlan plan)
    {
        try
        {
            var adminDrinks = await _unitOfWork.Repository<AdminSelectedDrink>().GetQueryable()
                .Where(x => x.PlanTypeEnum == plan.PlanType).Include(x => x.Drink).ToListAsync();

            var drinksToAdd = new List<UserSelectedDrink>();
            foreach (var drink in userSelectedDrinks)
            {
                var dr = adminDrinks.FirstOrDefault(x => x.DrinkId == drink.Id);
                if (dr == null) return false;
                drinksToAdd.Add(_mapper.Map<UserSelectedDrink>(dr.Drink));
            }

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

    private async Task<bool> _addExtrasToDaysPlan(List<Item> userSelectedExtras, UserPlan plan)
    {
        try
        {
            var adminExtras = await _unitOfWork.Repository<AdminSelectedExtraOption>().GetQueryable()
                .Where(x => x.PlanTypeEnum == plan.PlanType).Include(x => x.ExtraOption).ToListAsync();

            var extrasToAdd = new List<UserSelectedExtraOption>();
            foreach (var extra in userSelectedExtras)
            {
                var dr = adminExtras.FirstOrDefault(x => x.ExtraOptionId == extra.Id);
                if (dr == null) return false;
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

    private bool _addPlanDaysToPlan(NewSubscriptionDto subscriptionDto, UserPlan plan)
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

    private async Task<AppUser?> _getUser()
    {
        var email = HttpContext.User.GetEmail();
        return await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}