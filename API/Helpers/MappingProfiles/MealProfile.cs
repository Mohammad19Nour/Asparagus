using AsparagusN.Data.Entities.Meal;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.DTOs;
using AsparagusN.DTOs.AdminPlanDtos;
using AsparagusN.DTOs.AllergyDtos;
using AsparagusN.DTOs.CarbDtos;
using AsparagusN.DTOs.IngredientDtos;
using AsparagusN.DTOs.MealDtos;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class MealProfile : Profile
{
    public MealProfile()
    {
        CreateMap<Meal, MealLoyaltyPointDto>();
        CreateMap<Ingredient, UserMealCarb>().ForMember(dest=>dest.Id,opt=>opt.Ignore());
        CreateMap<Meal, CarbDto>();
        CreateMap<MealAllergy, AllergyDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.Allergy.Id))
            .ForMember(dest => dest.ArabicName, opt => opt.MapFrom(x => x.Allergy.ArabicName))
            .ForMember(dest => dest.EnglishName, opt => opt.MapFrom(x => x.Allergy.EnglishName))
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(x => x.Allergy.PictureUrl));
            
        CreateMap<MealIngredient,MealIngredientDetailsDto>();
        CreateMap<Meal, MealWithIngredientsDto>();
        CreateMap<Meal, MealWithoutIngredientsDto>();
        CreateMap<Meal, MealItemOrdered>();
        
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