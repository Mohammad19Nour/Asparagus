using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.DTOs.AdditionDtos;
using AsparagusN.DTOs.AdminPlanDtos;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class AdminPlanProfile : Profile
{
    public AdminPlanProfile()
    {
        CreateMap<AdminSelectedMeal, AdminSelectedMealDto>();
          
        CreateMap<DrinkItem,DrinkDto>()
            .ForAllMembers(dest=>dest.MapFrom(src=>src.Drink));
        CreateMap<UpdateAdminPlanDto, AdminPlan>();
        CreateMap<NewAdminPlanDto, AdminPlan>();
        CreateMap<AdminPlan, AdminPlanDto>()
            .ForMember(dest => dest.PlanType, opt =>
                opt.MapFrom(src => Enum.GetName(typeof(MealPlanType), src.PlanType)));
        
    }
}