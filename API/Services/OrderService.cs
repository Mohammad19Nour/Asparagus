using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.Entities;
using AsparagusN.Entities.OrderAggregate;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Services;

public class OrderService : IOrderService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId, string buyerEmail)
    {
        buyerEmail = buyerEmail.ToLower();
        var spec = new OrderWithItemsSpecification(buyerEmail, orderId);
        var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        return order;
    }

    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        buyerEmail = buyerEmail.ToLower();
        var spec = new OrderWithItemsSpecification(buyerEmail);
        var orders = await _unitOfWork.Repository<Order>().ListWithSpecAsync(spec);

        /* return await  _unitOfWork.Repository<Order>().GetQueryable().Include(x=>x.Items)
             .ThenInclude(y=>y.OrderedMeal).ToListAsync();*/
        return orders;
    }

    public async Task<Order?> CreateOrderAsync(string buyerEmail, int basketId, Address shippingAddress)
    {
        var spec = new BasketSpecification(basketId);
        var basket = await _unitOfWork.Repository<CustomerBasket>().GetEntityWithSpec(spec);
        if (basket == null) return null;

        var items = new List<OrderItem>();

        foreach (var item in basket.Items)
        {
            var meal = await _unitOfWork.Repository<Meal>().GetByIdAsync(item.MealId);

            if (meal == null || !meal.IsMainMenu) return null;

            var itemOrdered = _mapper.Map<MealItemOrdered>(meal);
            itemOrdered.MealId = meal.Id;

            itemOrdered.AddedProtein = item.AddedProtein;
            itemOrdered.AddedCarb = item.AddedCarb;
            var price = meal.Price + itemOrdered.PricePerCarb * itemOrdered.AddedCarb
                                   + itemOrdered.PricePerProtein * itemOrdered.AddedProtein;

            var orderItem = new OrderItem
            {
                OrderedMeal = itemOrdered,
                Price = price,
                Quantity = item.Quantity
            };
            items.Add(orderItem);
        }

        var subtotal = items.Sum(x => x.Price * x.Quantity);

        var order = new Order
        {
            BuyerEmail = buyerEmail,
            Items = items,
            ShipToAddress = shippingAddress,
            Subtotal = subtotal
        };

        _unitOfWork.Repository<Order>().Add(order);

        if (!await _unitOfWork.SaveChanges())
            return null;
      
        return order;
    }
}