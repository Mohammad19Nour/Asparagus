using AsparagusN.Enums;

namespace AsparagusN.Interfaces;

public interface IValidationService
{
     Task<(bool Success, string Message)> IsValidSubscriptionDto(object tmp, int oldDuration);
     Task<bool> CheckExistingExtras(ICollection<int> ids, PlanTypeEnum planType, ExtraOptionType optionType);
     Task<bool> CheckExistingDrinks(ICollection<int> ids, PlanTypeEnum planType);
}