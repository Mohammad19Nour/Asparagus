﻿using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Entities;
using AsparagusN.Entities.MealPlan;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class UserPlanProfile : Profile
{
    public UserPlanProfile()
    {
        CreateMap<UserSelectedMeal, UserSelectedMealDto>();
        CreateMap<UserSelectedExtraOption, UserSelectedExtraOptionDto>();
        CreateMap<UserSelectedDrink, UserSelectedDrinkDto>();
        CreateMap<UserPlanDay, UserPlanDayDto>();
        CreateMap<UserPlan, UserPlanDto>();
        CreateMap<Meal, UserSelectedMeal>();
        CreateMap<Drink, UserSelectedDrink>()
            .ForMember(x => x.Id, opt => opt.Ignore());
        CreateMap<ExtraOption, UserSelectedExtraOption>()
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight));
        CreateMap<NewSubscriptionDto, UserPlan>();
    }
}