using System.Security.Cryptography;
using AsparagusN.Data.Entities;
using AsparagusN.DTOs;
using AsparagusN.DTOs.AllergyDtos;
using AsparagusN.DTOs.BasketDtos;
using AsparagusN.Entities;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class SomeProfile : Profile
{
    public SomeProfile()
    {
        CreateMap<Allergy, AllergyDto>();
        CreateMap<NewAllergyDto,Allergy>();
        CreateMap<UpdateAllergyDto,Allergy>().ForAllMembers(x=>
            x.Condition((src,dest,srcMember)=>srcMember != null));
        CreateMap<decimal?, decimal>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<string?, string>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<CustomerBasketDto, CustomerBasket>();
        CreateMap<BasketItemDto, BasketItem>();
        CreateMap<MediaUrl, MediaUrlDto>();

    }
}