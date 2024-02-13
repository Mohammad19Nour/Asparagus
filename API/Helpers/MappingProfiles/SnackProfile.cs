using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.DTOs;
using AsparagusN.Entities;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class SnackProfile : Profile
{
    public SnackProfile()
    {
        CreateMap<Meal, SnackDto>();
        CreateMap<AdminSelectedSnack,SnackDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Snack.Id))
            .ForMember(dest => dest.NameEN, opt => opt.MapFrom(src => src.Snack.NameEN))
            .ForMember(dest => dest.NameAR, opt => opt.MapFrom(src => src.Snack.NameAR))
            .ForMember(dest => dest.DescriptionEN, opt => opt.MapFrom(src => src.Snack.DescriptionEN))
            .ForMember(dest => dest.DescriptionAR, opt => opt.MapFrom(src => src.Snack.DescriptionAR))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Snack.Price))
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.Snack.PictureUrl))
            .ForMember(x => x.Protein, o => o.MapFrom(src => src.Snack.Protein))
            .ForMember(x => x.Carbs, o => o.MapFrom(src => src.Snack.Carbs))
            .ForMember(x => x.Calories, o => o.MapFrom(src => src.Snack.Calories()))
            .ForMember(x => x.Fats, o => o.MapFrom(src => src.Snack.Fats))
            .ForMember(x => x.Fibers, o => o.MapFrom(src => src.Snack.Fibers));
        ;
    }
}