using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Enums;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Services;

public class ValidationService : IValidationService
{
    private readonly IUnitOfWork _unitOfWork;

    public ValidationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public (bool Success, string Message) IsValidSubscriptionDto(object tmp)
    {
        dynamic? subscriptionDto = tmp switch
        {
            NewSubscriptionDto dto => dto,
            UpdateSubscriptionDto uDto => uDto,
            _ => null
        };

        if (subscriptionDto == null) return (false, "invalid type of subscription model");
        if (subscriptionDto.PlanType != PlanTypeEnum.CustomMealPlan)
        {
            var isEnumValue = Enum.IsDefined(typeof(SubscriptionDuration), subscriptionDto.Duration);

            if (!isEnumValue)
                return (false, "Duration not valid");

            if (subscriptionDto.NumberOfMealPerDay > 3)
                return (false, "Number of meal must be less than 4");
        }

        if (subscriptionDto.SelectedDrinks != null)
        {
            if (subscriptionDto.SelectedDrinks.Count > subscriptionDto.Duration)
                return (false, "Number of drinks should be smaller than or equal to duration of the plan");
        }

        if (subscriptionDto.SelectedSalads != null)
        {
            if (subscriptionDto.SelectedSalads.Count > subscriptionDto.Duration)
                return (false, "Number of salads should be smaller than or equal to duration of the plan");
        }

        if (subscriptionDto.SelectedExtras != null)
        {
            if (subscriptionDto.SelectedExtras.Count > subscriptionDto.Duration)
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
}