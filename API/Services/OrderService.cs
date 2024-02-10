using AsparagusN.Entities;
using AsparagusN.Entities.OrderAggregate;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;

namespace AsparagusN.Services;

public class OrderService : IOrderService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBasketRepository _basketRepository;

    public OrderService(IMapper mapper, IUnitOfWork unitOfWork, IBasketRepository basketRepository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _basketRepository = basketRepository;
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId, string buyerEmail)
    {
        buyerEmail = buyerEmail.ToLower();
        var spec = new OrderWithItemsSpecification(buyerEmail,orderId);
        var order = await  _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        return order;
    }

    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        buyerEmail = buyerEmail.ToLower();
        var spec = new OrderWithItemsSpecification(buyerEmail);
      var orders = await  _unitOfWork.Repository<Order>().ListWithSpecAsync(spec);
      return orders;
    }

    public async Task<Order?> CreateOrderAsync(string buyerEmail, int basketId, Address shippingAddress)
    {
        var basket = await _basketRepository.GetBasketAsync(basketId);
        if (basket == null) return null;
        
        var items = new List<OrderItem>();

        foreach (var item in basket.Items)
        {
            var spec = new MealWithIngredientsAdnAllergiesSpecification(item.Id);
            var meal = await _unitOfWork.Repository<Meal>().GetEntityWithSpec(spec);

            if (meal == null) return null;

            var itemOrdered = _mapper.Map<MealItemOrdered>(meal);
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
        await _basketRepository.DeleteBasket(basketId);
        return order;
    }
}