using AsparagusN.DTOs.DriverDtos;
using AsparagusN.DTOs.ZoneDtos;
using AsparagusN.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AsparagusN.Helpers.MappingProfiles;

public class DriverProfile : Profile
{
    public DriverProfile()
    {
        CreateMap<Zone, ZoneDto>();
        CreateMap<NewZoneDto, Zone>();
        CreateMap<UpdateZoneDto, Zone>()
            .ForAllMembers(x => x.Condition(
                (src, dest, srcMember) => src != null));
        CreateMap<Driver, DriverDto>();
        CreateMap<NewDriverDto, Driver>();
        CreateMap<UpdateDriverDto, Driver>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => src != null));
    }
}