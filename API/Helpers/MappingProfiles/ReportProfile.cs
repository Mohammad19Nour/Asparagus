using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.OrderDtos;
using AsparagusN.DTOs.ReportDtos;
using AutoMapper;
using Stripe.Climate;

namespace AsparagusN.Helpers.MappingProfiles;

public class ReportProfile : Profile
{
    public ReportProfile()
    {
        CreateMap<UserPlan, PlanReportDto>()
            .ForMember(dest => dest.PlanType, opt => opt.MapFrom(src => Enum.GetName(src.PlanType)))
            .ForAllMembers(dest => dest.Condition((src, d, mem) => src != null));
        CreateMap<Driver, DriverReportDto>()
            .ForMember(desr => desr.Period, opt => opt.MapFrom(src => Enum.GetName(src.Period)))
            .ForMember(desr => desr.Zone, opt => opt.MapFrom(src => src.Zone.NameEN));
        CreateMap<AppUser, UserReportDto>();
        CreateMap<Order, OrderReportDto>();
    }
}