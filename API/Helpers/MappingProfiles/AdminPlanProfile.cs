using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.DTOs.AdminPlanDtos;
using AsparagusN.DTOs.CarbDtos;
using AsparagusN.DTOs.DrinksDtos;
using AsparagusN.DTOs.MealDtos;
using AsparagusN.Enums;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class AdminPlanProfile : Profile
{
    public AdminPlanProfile()
    {
        CreateMap<AdminSelectedCarb, CarbDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NameEN, opt => opt.MapFrom(src => src.Carb.NameEN))
            .ForMember(dest => dest.ExtraInfo, opt => opt.MapFrom(src => src.Carb.ExtraInfo))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Carb.Weight))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Carb.Price))
            .ForMember(dest => dest.NameAR, opt => opt.MapFrom(src => src.Carb.NameAR))
            .ForMember(x => x.Protein, o => o.MapFrom(src => src.Carb.Protein))
            .ForMember(x => x.Carb, o => o.MapFrom(src => src.Carb.Carb))
            .ForMember(x => x.Fat, o => o.MapFrom(src => src.Carb.Fat))
            .ForMember(x => x.Fiber, o => o.MapFrom(src => src.Carb.Fiber));

        CreateMap<AdminSelectedMeal, MealWithIngredientsDto>()
            .ForMember(x => x.SelectedCarb,
                opt => opt.MapFrom(src =>
                    src.Meal.Ingredients
                        .FirstOrDefault(y => y.Ingredient.TypeOfIngredient == IngredientType.Carb)))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NameEN, opt => opt.MapFrom(src => src.Meal.NameEN))
            .ForMember(dest => dest.NameAR, opt => opt.MapFrom(src => src.Meal.NameAR))
            .ForMember(dest => dest.DescriptionEN, opt => opt.MapFrom(src => src.Meal.DescriptionEN))
            .ForMember(dest => dest.DescriptionAR, opt => opt.MapFrom(src => src.Meal.DescriptionAR))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Meal.Price))
            .ForMember(dest => dest.Points, opt => opt.MapFrom(src => src.Meal.Points))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Meal.CreatedAt))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Meal.CategoryId))
            .ForMember(dest => dest.Allergies, opt => opt.MapFrom(src => src.Meal.Allergies))
            .ForMember(x => x.Protein, o => o.MapFrom(src => src.Meal.Protein))
            .ForMember(x => x.Carbs, o => o.MapFrom(src => src.Meal.Carbs))
            .ForMember(x => x.Calories, o => o.MapFrom(src => src.Meal.Calories()))
            .ForMember(x => x.Fats, o => o.MapFrom(src => src.Meal.Fats))
            .ForMember(x => x.Fibers, o => o.MapFrom(src => src.Meal.Fibers));
            
        CreateMap<UpdateAdminPlanDto, AdminPlanDay>();
        CreateMap<NewAdminPlanDto, AdminPlanDay>();
        CreateMap<AdminPlanDay, AdminPlanDayDto>()
            .ForMember(dest => dest.Meals, opt => opt.MapFrom(src => src.Meals))
            .ForMember(dest => dest.PlanType, opt =>
                opt.MapFrom(src => Enum.GetName(typeof(PlanTypeEnum), src.PlanType)));
        
    }
}