using AsparagusN.Data.Entities;
using AsparagusN.DTOs.BranchDtos;
using AsparagusN.DTOs.CashierDtos;
using AsparagusN.Enums;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class CashierProfile : Profile
{
    public CashierProfile()
    {
         CreateMap<Cashier, CashierDto>()
            .ForMember(dest => dest.Period, opt =>
                opt.MapFrom(src => Enum.GetName(typeof(Period), src.Period)));
        CreateMap<Cashier, AdminCashierDto>();
        CreateMap<NewCashierDto, Cashier>();
        CreateMap<UpdateCashierDto, Cashier>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));

    }
}