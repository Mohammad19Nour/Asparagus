using AsparagusN.DTOs.IngredientDtos;
using AsparagusN.Entities;
using AsparagusN.Enums;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        CreateMap<Ingredient, IngredientDto>()
            .ForMember(dest => dest.TypeOfIngredient, opt =>
                opt.MapFrom(src => Enum.GetName(typeof(IngredientType), src.TypeOfIngredient)));
        CreateMap<UpdateIngredientDto, Ingredient>()
            .ForAllMembers
            (opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null))
            ;
        CreateMap<NewIngredientDto,Ingredient>();
    }
}