using System.Security.Cryptography;
using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.Meal;
using AsparagusN.DTOs;
using AsparagusN.DTOs.AllergyDtos;
using AsparagusN.DTOs.BasketDtos;
using AsparagusN.DTOs.PackageDtos;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class SomeProfile : Profile
{
    public SomeProfile()
    {
        CreateMap<AppUser, AdminAccountDto>();
        CreateMap<GiftSelection, MonthGiftDto>();
        CreateMap<Meal, MealInfoDto>();
        CreateMap<Notification, NotificationDto>();
        CreateMap<Allergy, AllergyDto>();
        CreateMap<NewAllergyDto, Allergy>();
        CreateMap<UpdateAllergyDto, Allergy>().ForAllMembers(x =>
            x.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<decimal?, decimal>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<string?, string>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<CustomerBasketDto, CustomerBasket>();
        CreateMap<BasketItemDto, BasketItem>();
        CreateMap<MediaUrl, MediaUrlDto>();
    }
}