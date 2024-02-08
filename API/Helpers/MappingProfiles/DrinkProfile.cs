using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.DTOs.DrinksDtos;
using AsparagusN.Entities;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class DrinkProfile : Profile
{
    public DrinkProfile()
    {
        CreateMap<AdminSelectedDrink,DrinkDto>()
            .ForMember(dest=>dest.PictureUrl,opt=>opt.MapFrom(src=>src.Drink.PictureUrl))
            .ForMember(dest=>dest.Price,opt=>opt.MapFrom(src=>src.Drink.Price))
            .ForMember(dest=>dest.Volume,opt=>opt.MapFrom(src=>src.Drink.Volume))
            .ForMember(dest=>dest.NameArabic,opt=>opt.MapFrom(src=>src.Drink.NameArabic))
            .ForMember(dest=>dest.NameEnglish,opt=>opt.MapFrom(src=>src.Drink.NameEnglish));

        CreateMap<Drink,DrinkDto>();
        CreateMap<NewDrinkDto,Drink>();
        CreateMap<UpdateDrinkDto,Drink>()
            .ForAllMembers(x=>x.Condition((src,d,srcMember)=>srcMember!=null));
    }
}