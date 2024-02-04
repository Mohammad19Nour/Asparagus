using AsparagusN.DTOs.AdminPlanDtos;
using AsparagusN.DTOs.AllergyDtos;
using AsparagusN.DTOs.IngredientDtos;
using AsparagusN.DTOs.MealDtos;
using AsparagusN.Entities;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class MealProfile : Profile
{
    public MealProfile()
    {
        CreateMap<MealAllergy, AllergyDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.Allergy.Id))
            .ForMember(dest => dest.ArabicName, opt => opt.MapFrom(x => x.Allergy.ArabicName))
            .ForMember(dest => dest.EnglishName, opt => opt.MapFrom(x => x.Allergy.EnglishName))
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(x => x.Allergy.PictureUrl));
            
        CreateMap<MealIngredient,MealIngredientDetailsDto>();
        CreateMap<Meal, MealWithIngredientsDto>()
            .ForMember(dest => dest.PricePerProtein, opt => opt.MapFrom(src => Convert.ToDecimal(src.PricePerProtein())))
            .ForMember(dest => dest.PricePerCarb, opt => opt.MapFrom(src => Convert.ToDecimal(src.PricePerCarb())))

            .ForMember(dest => dest.Protein, opt => opt.MapFrom(src => Convert.ToDecimal(src.Protein())))
            .ForMember(dest => dest.Carbs, opt => opt.MapFrom(src => Convert.ToDecimal(src.Carbs())))
            .ForMember(dest => dest.Fats, opt => opt.MapFrom(src => Convert.ToDecimal(src.Fats())))
            .ForMember(dest => dest.Calories, opt => opt.MapFrom(src => Convert.ToDecimal(src.Calories())))
            .ForMember(dest => dest.Fibers, opt => opt.MapFrom(src => Convert.ToDecimal(src.Fibers())));
        CreateMap<Meal, MealWithoutIngredientsDto>();
        
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