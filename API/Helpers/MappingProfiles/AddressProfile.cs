using AsparagusN.DTOs;
using AsparagusN.Entities;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<Location, LocationDto>().ReverseMap();
        CreateMap<UpdateLocationDto,Location>().ForAllMembers
        (opt =>
            opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<Address,AddressDto>().ReverseMap();
        CreateMap<UpdateAddressDto, Address>()
            .ForAllMembers
            (opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}