using AsparagusN.DTOs.MealDtos;
using AsparagusN.Entities;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class MealProfile : Profile
{
    public MealProfile()
    {
        CreateMap<MealIngredient,MealIngredientDetailsDto>();
        CreateMap<Meal, MealDto>();
        
        CreateMap<UpdateMealDto, Meal>()
            .ForMember(dest=>dest.Ingredients,opt=>
                opt.Ignore())
            .ForMember(dest=>dest.Allergies,opt=>
                opt.Ignore())
            .ForAllMembers
            (opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));
        
        CreateMap<NewMealDto, Meal>()
            .ForMember(dest=>dest.Ingredients,opt=>
                opt.Ignore())
            .ForMember(dest=>dest.Allergies,opt=>
                opt.Ignore());
    }
}