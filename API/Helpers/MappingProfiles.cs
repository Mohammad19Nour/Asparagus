using AsparagusN.DTOs;
using AsparagusN.Entities;
using AsparagusN.Entities.Identity;
using AsparagusN.Enums;
using AutoMapper;

namespace AsparagusN.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<AppUser, UserDto>();
        CreateMap<UserAddress,AddressDto>();
    }
}