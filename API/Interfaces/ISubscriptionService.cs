using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Entities.Identity;
using AsparagusN.Enums;
using Stripe;

namespace AsparagusN.Interfaces;

public interface ISubscriptionService
{
    Task<(UserPlan? createdPlan, string Message)> CreateSubscriptionAsync(NewSubscriptionDto newSubscriptionDto, AppUser user);
    public Task<(UserPlan?, string Message)> UpdateSubscription(UpdateSubscriptionDto subscriptionDto, AppUser user);
    Task<UserPlan?> GetUserSubscriptionAsync(AppUser user, PlanTypeEnum planTypeEnum);
    Task<List<UserPlan>> GetAllUserSubscriptionsAsync(AppUser user);
    Task<(decimal? Price, string Message)> GetPriceForUpdate(UpdateSubscriptionDto subscription, AppUser user);
    Task<(decimal? Price, string Message)> GetPriceForCreate(NewSubscriptionDto subscription, AppUser user);
}