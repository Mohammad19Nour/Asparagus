using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Meal;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs;
using AsparagusN.DTOs.AllergyDtos;
using AsparagusN.DTOs.CarbDtos;
using AsparagusN.DTOs.DrinksDtos;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Enums;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class UserPlanProfile : Profile
{
    public UserPlanProfile()
    {
        CreateMap<UserMealCarb, UserMealCarbDto>();
        CreateMap<Allergy, UserPlanAllergy>().ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UserSelectedSnack, UserSnackDto>();
        CreateMap<UserSelectedMeal, UserSelectedMealDto>();
        CreateMap<UserSelectedExtraOption, UserSelectedExtraOptionDto>();
        CreateMap<UserSelectedDrink, UserSelectedDrinkDto>();
        CreateMap<UserPlanDay, UserPlanDayDto>()
            .ForMember(dest => dest.Carb,
                src => src.MapFrom(x => HelperFunctions.Calculate("carb", x.SelectedMeals, x.SelectedSnacks)))
            .ForMember(dest => dest.Protein,
                src => src.MapFrom(x => HelperFunctions.Calculate("protein", x.SelectedMeals, x.SelectedSnacks)))
            .ForMember(dest => dest.Fat,
                src => src.MapFrom(x => HelperFunctions.Calculate("fat", x.SelectedMeals, x.SelectedSnacks)))
            .ForMember(dest => dest.SelectedExtraOptions,
                opt => opt.MapFrom(
                    x => x.SelectedExtraOptions.Where(y => y.OptionType == ExtraOptionType.Nuts).ToList()))
            .ForMember(dest => dest.SelectedSalads,
                opt => opt.MapFrom(
                    x => x.SelectedExtraOptions.Where(y => y.OptionType == ExtraOptionType.Salad).ToList()));
        CreateMap<UserPlan, UserPlanDto>();
        CreateMap<Meal, UserSelectedMeal>().ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<Drink, UserSelectedDrink>()
            .ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<ExtraOption, UserSelectedExtraOption>()
            .ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<NewSubscriptionDto, UserPlan>().ForMember(dest => dest.Allergies, opt =>
            opt.Ignore());
        CreateMap<UpdateSubscriptionDto, UserPlan>()
            .ForMember(dest => dest.Allergies, opt =>
                opt.Ignore());
        CreateMap<UserPlanInfoDto, UserPlan>()
            .ForMember(dest => dest.Allergies, opt =>
                opt.Ignore())
            .ForAllMembers(dest => dest.Condition((src, de, mem) => mem != null));

        CreateMap<UserPlan, UserPlanInfoDto>();
        CreateMap<UserPlan, SubscriptionDto>();
        CreateMap<UserPlanAllergy, AllergyDto>();
        
        CreateMap<UserPlanDay, OrderUserPlanDayDto>()
            .ForMember(dest => dest.Carb,
                src => src.MapFrom(x => HelperFunctions.Calculate("carb", x.SelectedMeals, x.SelectedSnacks)))

            .ForMember(dest => dest.Carb,
                src => src.MapFrom(x => HelperFunctions.Calculate("carb", x.SelectedMeals, x.SelectedSnacks)))
            .ForMember(dest => dest.Protein,
                src => src.MapFrom(x => HelperFunctions.Calculate("protein", x.SelectedMeals, x.SelectedSnacks)))
            .ForMember(dest => dest.Fat,
                src => src.MapFrom(x => HelperFunctions.Calculate("fat", x.SelectedMeals, x.SelectedSnacks)))
            .ForMember(dest => dest.SelectedExtraOptions,
                opt => opt.MapFrom(
                    x => x.SelectedExtraOptions.Where(y => y.OptionType == ExtraOptionType.Nuts).ToList()))
            .ForMember(dest => dest.SelectedSalads,
                opt => opt.MapFrom(
                    x => x.SelectedExtraOptions.Where(y => y.OptionType == ExtraOptionType.Salad).ToList()));

    }
}