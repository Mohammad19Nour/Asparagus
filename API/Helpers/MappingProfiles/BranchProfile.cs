using AsparagusN.DTOs.BranchDtos;
using AsparagusN.Entities;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class BranchProfile : Profile
{
    public BranchProfile()
    {
        CreateMap<Branch, BranchDto>();
        CreateMap<NewBranchDto, Branch>();
        CreateMap<UpdateBranchDto,Branch>()
            .ForMember(dest=>dest.Address,opt=>
                opt.MapFrom(x=>x.UpdatedAddress))
            .ForAllMembers
            (opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));
        
    }
}