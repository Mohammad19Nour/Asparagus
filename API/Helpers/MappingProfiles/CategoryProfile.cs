using AsparagusN.Data.Entities;
using AsparagusN.DTOs.CategoryDtos;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<NewCategoryDto, Category>().ReverseMap();

        CreateMap<UpdateCategoryDto, Category>()
            .ForAllMembers
            (opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}