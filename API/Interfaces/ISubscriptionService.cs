using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Entities.Identity;
using AsparagusN.Enums;

namespace AsparagusN.Interfaces;

public interface ISubscriptionService
{
    Task<UserPlan?> CreateSubscriptionAsync(NewSubscriptionDto newSubscriptionDto, AppUser user);
    Task<bool>  CheckExistingSubscriptionPlanForUserAsync(int userId, PlanTypeEnum planType);
}