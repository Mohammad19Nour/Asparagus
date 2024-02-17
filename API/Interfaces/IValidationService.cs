using AsparagusN.Enums;

namespace AsparagusN.Interfaces;

public interface IValidationService
{
     (bool Success, string Message) IsValidSubscriptionDto(object tmp);
     Task<bool> CheckExistingExtras(ICollection<int> ids, PlanTypeEnum planType, ExtraOptionType optionType);
     Task<bool> CheckExistingDrinks(ICollection<int> ids, PlanTypeEnum planType);
}