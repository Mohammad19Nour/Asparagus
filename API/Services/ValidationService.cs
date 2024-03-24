using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.DTOs.SubscriptionDtos;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Enums;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AsparagusN.Specifications.AdminPlanSpecifications;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Services;

public class ValidationService : IValidationService
{
    private readonly IUnitOfWork _unitOfWork;

    public ValidationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<(bool Success, string Message)> IsValidSubscriptionDto(object tmp,int oldDuration)
    {
        dynamic? subscriptionDto = tmp switch
        {
            NewSubscriptionDto dto => dto,
            UpdateSubscriptionDto uDto => uDto,
            NewCustomSubscriptionDto xDto => xDto,
            _ => null
        };

        if (subscriptionDto == null) return (false, "invalid type of subscription model");
        if (subscriptionDto.PlanType != PlanTypeEnum.CustomMealPlan)
        {
            var exist = await CheckIfBundleExist(subscriptionDto.Duration,subscriptionDto.NumberOfMealPerDay);
            if (!exist)
                return (false, "Duration not valid");

            if (subscriptionDto.NumberOfMealPerDay > 3)
                return (false, "Number of meal must be less than 4");
        }

        if (subscriptionDto.SelectedDrinks != null)
        {
            if (subscriptionDto.SelectedDrinks.Count > subscriptionDto.Duration - oldDuration)
                return (false, "Number of drinks should be smaller than or equal to duration of the plan");
        }

        if (subscriptionDto.SelectedSalads != null)
        {
            if (subscriptionDto.SelectedSalads.Count > subscriptionDto.Duration - oldDuration)
                return (false, "Number of salads should be smaller than or equal to duration of the plan");
        }

        if (subscriptionDto.SelectedExtras != null)
        {
            if (subscriptionDto.SelectedExtras.Count > subscriptionDto.Duration- oldDuration)
                return (false, "Number of extras should be smaller than or equal to duration of the plan");
        }

        return (true, "");
    }

    public async Task<bool> CheckExistingExtras(ICollection<int> ids, PlanTypeEnum planType, ExtraOptionType optionType)
    {
        var spec = new AdminSelectedExtrasAndSaladsSpecification(optionType, planType);
        var adminExtras = (await _unitOfWork.Repository<AdminSelectedExtraOption>().ListWithSpecAsync(spec)).ToList()
            .Where(x => x.PlanTypeEnum == planType && ids.Contains(x.Id)).ToList();

        return adminExtras.Distinct().ToList().Count == ids.Distinct().ToList().Count;
    }

    public async Task<bool> CheckExistingDrinks(ICollection<int> ids, PlanTypeEnum planType)
    {
        var adminDrinks = await _unitOfWork.Repository<AdminSelectedDrink>().GetQueryable()
            .Where(x => x.PlanTypeEnum == planType && ids.Contains(x.Id)).ToListAsync();

        return adminDrinks.Distinct().ToList().Count == ids.Distinct().ToList().Count;
    }

    public async Task<bool> CheckIfBundleExist(int duration , int mealsPerDay)
    {
        var spec = new BundleSpecification(duration, mealsPerDay);
        var bundle = await _unitOfWork.Repository<Bundle>().GetEntityWithSpec(spec);
        return bundle != null;
    }
}