using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs;
using AsparagusN.DTOs.SubscriptionDtos;
using AsparagusN.DTOs.UserPlanDtos;
namespace AsparagusN.Interfaces;

public interface ICustomSubscriptionService
{
    Task<(UserPlan? createdPlan, string Message)> CreateSubscriptionAsync(NewCustomSubscriptionDto newSubscriptionDto, AppUser user);
    public Task<(UserPlan?, string Message)> UpdateSubscription(UpdateSubscriptionDto subscriptionDto, AppUser user);
    Task<(decimal? Price, string Message)> GetPriceForUpdate(UpdateSubscriptionDto subscription, AppUser user);
    Task<(decimal? Price, string Message)> GetPriceForCreate(NewCustomSubscriptionDto subscription, AppUser user);

}