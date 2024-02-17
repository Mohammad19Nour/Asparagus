﻿using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs;
using AsparagusN.DTOs.AllergyDtos;
using AsparagusN.DTOs.CarbDtos;
using AsparagusN.DTOs.DrinksDtos;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Entities;
using AsparagusN.Entities.MealPlan;
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
        CreateMap<UserSelectedMeal, UserSelectedMealDto>().ForMember(dest => dest.ChangedCarbNameEN,
                opt => opt.MapFrom(src => src.ChangedCarb.NameEN))
            .ForMember(dest => dest.ChangedCarbNameAR, opt => opt.MapFrom(src => src.ChangedCarb.NameAR));
        CreateMap<UserSelectedExtraOption, UserSelectedExtraOptionDto>();
        CreateMap<UserSelectedDrink, UserSelectedDrinkDto>();
        CreateMap<UserPlanDay, UserPlanDayDto>()
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
    }
}