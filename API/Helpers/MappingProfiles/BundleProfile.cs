using AsparagusN.Data.Entities;
using AsparagusN.DTOs.BundleDtos;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class BundleProfile:Profile
{
    public BundleProfile()
    {
        CreateMap<Bundle, BundleDto>();
        CreateMap<NewBundleDto, Bundle>();
        CreateMap<UpdateBundleDto, Bundle>().ForAllMembers(dest=>dest.Condition(src=>src != null));
    }
}