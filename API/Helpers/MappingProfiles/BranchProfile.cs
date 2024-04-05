using AsparagusN.Data.Entities;
using AsparagusN.DTOs.BranchDtos;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class BranchProfile : Profile
{
    public BranchProfile()
    {
        CreateMap<Branch, BranchDto>();
        CreateMap<Branch, BranchCasherDto>();
        CreateMap<NewBranchDto, Branch>();
        CreateMap<UpdateBranchDto,Branch>()
            .ForMember(dest=>dest.Address,opt=>
                opt.MapFrom(x=>x.UpdatedAddress))
            .ForAllMembers
            (opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));
        
    }
}