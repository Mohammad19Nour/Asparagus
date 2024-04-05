using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.SubscriptionDtos;
using AsparagusN.DTOs.UserPlanDtos;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class SubscriptionProfile : Profile
{
    public SubscriptionProfile()
    {
        CreateMap<NewCustomSubscriptionDto, NewSubscriptionDto>();
        CreateMap<NewCustomSubscriptionDto, UserPlan>()
            .ForMember(dest => dest.Allergies, opt =>
                opt.Ignore())
            .ForMember(c => c.CarbPerMealForCustomPlan, opt => opt.MapFrom(src => src.CarbPerMeal))
            .ForMember(c => c.ProteinPerMealForCustomPlan, opt => opt.MapFrom(src => src.ProteinPerMeal));
    }
}