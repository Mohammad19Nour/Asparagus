using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.DTOs.OrderDtos;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.BranchNameAR, opt => opt.MapFrom(src =>src.Branch.NameAR))
            .ForMember(dest => dest.BranchNameEN, opt => opt.MapFrom(src => src.Branch.NameEN))
            .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => Enum.GetName(src.PaymentType)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.GetName(src.Status)));

        ;
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<MealItemOrdered, MealItemOrderedDto>();
    }
}