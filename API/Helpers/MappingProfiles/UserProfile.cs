using AsparagusN.Data.Entities.Identity;
using AsparagusN.DTOs;
using AsparagusN.DTOs.AccountDtos;
using AsparagusN.DTOs.UserDtos;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<AppUser, AdminUserDto>()
            .ForMember(dest=>dest.Gender,opt=>opt.MapFrom(src=>Enum.GetName(src.Gender)));
        
        CreateMap<RegisterDto, AppUser>();
        CreateMap<AppUser, AccountDto>();
        
            CreateMap<AppUser, UserInfoDto>()
            .ForMember(dest=>dest.WorkAddress,opt=>opt.MapFrom(y=>HelperFunctions.CheckExistAddress(y.WorkAddress) ? y.WorkAddress : null))
            .ForMember(dest=>dest.HomeAddress,opt=>opt.MapFrom(y=>HelperFunctions.CheckExistAddress(y.HomeAddress) ? y.HomeAddress : null))
            .ForMember(dest=>dest.Gender,opt=>opt.MapFrom(src=>Enum.GetName(src.Gender)));
        CreateMap<UpdateUserInfoDto, AppUser>().
            ForAllMembers
        (opt =>
            opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}