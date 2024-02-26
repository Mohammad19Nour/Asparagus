using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.PackageDtos;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class PackageProfile : Profile
{
    public PackageProfile()
    {
       
        CreateMap<UserPlan, PackageDto>();
        CreateMap<UserPlanDay, PackageDto>()
            .ForMember(dest => dest.Meals, opt => opt.MapFrom(src => src.SelectedMeals.ToList()))
            .ForMember(dest => dest.Snacks, opt => opt.MapFrom(src => src.SelectedSnacks))
            .ForMember(dest => dest.Drinks, opt => opt.MapFrom(src => src.SelectedDrinks.ToList()));

        CreateMap<AppUser, CustomerInfoDto>();
        CreateMap<UserSelectedExtraOption, SaladInfoDto>();
        CreateMap<UserSelectedExtraOption, NutsInfoDto>();
        CreateMap<UserSelectedSnack, SnackInfoDto>();

        CreateMap<UserSelectedMeal, MealInfoDto>()
            .ForMember(dest => dest.CarbNameAR, opt => opt.MapFrom(src => src.ChangedCarb.NameAR))
            .ForMember(dest => dest.CarbNameEN, opt => opt.MapFrom(src => src.ChangedCarb.NameEN));

        CreateMap<UserSelectedDrink, DrinkInfoDto>()
            .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => Enum.GetName(src.Volume)));
    }
}