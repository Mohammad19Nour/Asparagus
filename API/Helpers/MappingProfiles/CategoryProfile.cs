using AsparagusN.DTOs;
using AsparagusN.DTOs.AllergyDtos;
using AsparagusN.DTOs.IngredientDtos;
using AsparagusN.DTOs.MealDtos;
using AsparagusN.Entities;
using AsparagusN.Entities.Identity;
using AsparagusN.Enums;
using AutoMapper;

namespace AsparagusN.Helpers;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
       
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<NewCategoryDto, Category>().ReverseMap();
       
        CreateMap<UpdateCategoryDto,Category>().ForAllMembers
        (opt =>
            opt.Condition((src, dest, srcMember) => srcMember != null));
       
    }
}