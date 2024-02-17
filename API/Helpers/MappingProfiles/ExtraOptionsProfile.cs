using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.DTOs.AdditionDtos;
using AsparagusN.Entities;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class ExtraOptionsProfile : Profile
{
    public ExtraOptionsProfile()
    {
        CreateMap<AdminSelectedExtraOption, ExtraOptionDto>()
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.ExtraOption.Weight))
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.ExtraOption.PictureUrl))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.ExtraOption.Price))
            .ForMember(dest => dest.OptionType, opt => opt.MapFrom(src => src.ExtraOption.OptionType))
            .ForMember(dest => dest.NameArabic, opt => opt.MapFrom(src => src.ExtraOption.NameArabic))
            .ForMember(dest => dest.NameEnglish, opt => opt.MapFrom(src => src.ExtraOption.NameEnglish));

        CreateMap<ExtraOption, ExtraOptionDto>();
        CreateMap<NewExtraOptionDto, ExtraOption>();
        CreateMap<UpdateExtraOptionDto, ExtraOption>().ForAllMembers(dest =>
            dest.Condition((src, des, srcMember) => srcMember != null));
    }
}