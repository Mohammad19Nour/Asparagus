using AsparagusN.DTOs;
using AsparagusN.DTOs.AllergyDtos;
using AsparagusN.Entities;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class SomeProfile : Profile
{
    public SomeProfile()
    {
        CreateMap<Allergy, AllergyDto>();
        CreateMap<decimal?, decimal>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<CustomerBasketDto, CustomerBasket>();
        CreateMap<BasketItemDto, BasketItem>();
        CreateMap<MediaUrl, MediaUrlDto>();

    }
}