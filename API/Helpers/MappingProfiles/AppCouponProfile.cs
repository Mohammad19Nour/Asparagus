using AsparagusN.Data.Entities;
using AsparagusN.DTOs.CouponDtos;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class AppCouponProfile : Profile
{
    public AppCouponProfile()
    {
        CreateMap<AppCoupon, CouponDto>();
        CreateMap<NewCouponDto, AppCoupon>();
        CreateMap<UpdateCouponDto, AppCoupon>()
            .ForAllMembers(dest => dest.Condition((src, d, srcMember) => srcMember != null));
    }
}