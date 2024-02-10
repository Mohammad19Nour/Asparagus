using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.DTOs.OrderDtos;
using AsparagusN.Entities.OrderAggregate;
using AutoMapper;

namespace AsparagusN.Helpers.MappingProfiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<MealItemOrdered,MealItemOrderedDto>();
    }
}