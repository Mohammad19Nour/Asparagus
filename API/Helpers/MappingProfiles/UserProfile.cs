using AsparagusN.DTOs;
using AsparagusN.Entities;
using AsparagusN.Entities.Identity;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterDto, AppUser>();
        CreateMap<AppUser, AccountDto>();
        CreateMap<AppUser, UserInfoDto>();
        CreateMap<UpdateUserInfoDto, AppUser>().ForAllMembers
        (opt =>
            opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}