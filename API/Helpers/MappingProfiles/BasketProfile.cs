using AsparagusN.Data.Entities;
using AsparagusN.DTOs;
using AsparagusN.DTOs.BasketDtos;
using AsparagusN.Entities;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class BasketProfile : Profile
{
    public BasketProfile()
    {
        CreateMap<BasketItem, BasketItemDto>()
            .ForMember(dest => dest.MealId, opt => opt.MapFrom(src => src.Meal.Id))
            .ForMember(dest => dest.NameEN, opt => opt.MapFrom(src => src.Meal.NameEN))
            .ForMember(dest => dest.NameAR, opt => opt.MapFrom(src => src.Meal.NameAR))
            .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.Meal.PictureUrl))
            .ForMember(dest => dest.Price, opt =>
                opt.MapFrom(src =>
                    (src.Meal.Price + src.Meal.PricePerCarb * src.AddedCarb +
                     src.Meal.PricePerProtein * src.AddedProtein) * src.Quantity));
        CreateMap<AddBasketItemDto, BasketItem>();
        CreateMap<Meal, BasketItem>();
        CreateMap<CustomerBasket, CustomerBasketDto>();
    }
}