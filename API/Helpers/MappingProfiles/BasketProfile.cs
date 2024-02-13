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
        CreateMap<BasketItem, BasketItemDto>();
        CreateMap<AddBasketItemDto, BasketItem>();
        CreateMap<Meal, BasketItem>();
        CreateMap<CustomerBasket, CustomerBasketDto>();
    }
}