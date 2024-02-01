using AsparagusN.Data.Additions;
using AsparagusN.DTOs.AdditionDtos;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class AdditionsProfile : Profile
{
    public AdditionsProfile()
    {
        CreateMap<Drink, DrinkDto>();
        CreateMap<NewDrinkDto, Drink>();
        CreateMap<UpdateDrinkDto,Drink>()
            .ForAllMembers (opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));;
    }
}