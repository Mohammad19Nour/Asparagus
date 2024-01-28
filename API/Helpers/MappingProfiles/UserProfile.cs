using AsparagusN.DTOs;
using AsparagusN.Entities;
using AsparagusN.Entities.Identity;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AppUser, AccountDto>();
        CreateMap<UserAddress,AddressDto>().ReverseMap();
        CreateMap<AppUser, UserInfoDto>();
        CreateMap<UpdateUserInfoDto, AppUser>().ForAllMembers
        (opt =>
            opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}