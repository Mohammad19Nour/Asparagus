using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.Meal;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.DTOs.OrderDtos;
using AsparagusN.Enums;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Services;

public class OrderService : IOrderService
{
    private readonly ILocationService _locationService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;

    public OrderService(ILocationService locationService, IMapper mapper, IUnitOfWork unitOfWork,
        INotificationService notificationService)
    {
        _locationService = locationService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId, string buyerEmail)
    {
        buyerEmail = buyerEmail.ToLower();
        var spec = new OrderWithItemsSpecification(buyerEmail, orderId);
        var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        return order;
    }

    public async Task<(bool Success, string Message)> AssignOrderToDriver(int orderId, int driverId)
    {
        var spec = new OrderWithItemsSpecification(orderId);
        var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

        if (order == null) return (false, "Order not found");

        var driver = await _unitOfWork.Repository<Driver>().GetByIdAsync(driverId);
        if (driver == null) return (false, "Driver not found");


        if (order.Status != OrderStatus.Pending) return (false, "Order isn't pending");

        if (driver.Status != DriverStatus.Idle)
            return (false, $" Can't assign to this driver because he is {driver.Status}");

        order.Driver = driver;
        _unitOfWork.Repository<Order>().Update(order);

        if (await _unitOfWork.SaveChanges())
            return (true, "Assigned successfully");

        return (false, "Failed to assign driver");
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

    public async Task<(Order? Order, string Message)> CreateOrderAsync(string buyerEmail, int basketId,
        NewOrderInfoDto newOrderInfoDto)
    {
        try
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var user = await _unitOfWork.Repository<AppUser>().GetByIdAsync(basketId);
                Order? order;
                (order, var message) = await CalcPriceOfOrder(buyerEmail, basketId, newOrderInfoDto);

                if (order == null) return (order, Message: message);
                if (!await _locationService.CanDeliver(newOrderInfoDto.ShipToAddress))
                    return (null, "Can't deliver to this location");


                if (order.PaymentType == PaymentType.Card)
                {
                    // if payment through card failed then return from here
                }

                if (order.PaymentType == PaymentType.Points)
                {
                    if (order.PointsPrice > user!.LoyaltyPoints)
                        return (null, "You don't enough points");
                    user.LoyaltyPoints -= order.PointsPrice;
                }

                if (order.PaymentType != PaymentType.Points)
                    user.LoyaltyPoints += order.GainedPoints;

                order.BuyerPhoneNumber = user!.PhoneNumber;
                _unitOfWork.Repository<Order>().Add(order);

                if (!(await _unitOfWork.SaveChanges()))
                {
                    await transaction.RollbackAsync();
                    return (null, "Something happened during saving order ");
                }

                await _notificationService.NotifyUserByEmail(user.Email, "تم اضافة الطلب", "Order added");

                await transaction.CommitAsync();
                return (order: order, "Done");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<(Order? Order, string Message)> CalcPriceOfOrder(string buyerEmail, int basketId,
        NewOrderInfoDto newOrderInfoDto)
    {
        var spec = new BasketSpecification(basketId);
        var basket = await _unitOfWork.Repository<CustomerBasket>().GetEntityWithSpec(spec);

        if (basket == null || basket.Items.Count == 0) return (null, "Your basket is empty");

        if (await _unitOfWork.Repository<Branch>().GetByIdAsync(newOrderInfoDto.BranchId) == null)
            return (null, "Branch not found");

        var items = new List<OrderItem>();

        foreach (var item in basket.Items)
        {
            var meal = await _unitOfWork.Repository<Meal>().GetByIdAsync(item.MealId);

            if (meal == null || !meal.IsMainMenu) return (null, $"Meal with id = {item.MealId} not found");

            var itemOrdered = _mapper.Map<MealItemOrdered>(meal);
            itemOrdered.MealId = meal.Id;

            itemOrdered.AddedProtein = item.AddedProtein;
            itemOrdered.AddedCarb = item.AddedCarb;
            var price = meal.Price + (decimal)itemOrdered.PricePerCarb * (decimal)itemOrdered.AddedCarb
                                   + (decimal)itemOrdered.PricePerProtein * itemOrdered.AddedProtein;

            var orderItem = new OrderItem
            {
                OrderedMeal = itemOrdered,
                Price = price,
                Quantity = item.Quantity,
                GainedPoint = (newOrderInfoDto.PaymentType != PaymentType.Points) ? meal.Points : 0
            };
            if (newOrderInfoDto.PaymentType == PaymentType.Points)
            {
                if (meal.LoyaltyPoints == null) return (null, $"you can't buy {meal.NameEN} using points ");
                orderItem.PointsPrice = meal.LoyaltyPoints.Value;
            }

            items.Add(orderItem);
        }

        var subtotal = items.Sum(x => x.Price * x.Quantity);
        var pointsPrice = items.Sum(x => x.PointsPrice * x.Quantity);
        var gainedPoints = items.Sum(x => x.GainedPoint * x.Quantity);

        var order = new Order
        {
            BuyerEmail = buyerEmail,
            Items = items,
            ShipToAddress = _mapper.Map<Address>(newOrderInfoDto.ShipToAddress),
            Subtotal = subtotal,
            PaymentType = newOrderInfoDto.PaymentType,
            BranchId = newOrderInfoDto.BranchId,
            PointsPrice = pointsPrice,
            GainedPoints = gainedPoints
        };
        AppCoupon? coupon = null;
        if (newOrderInfoDto.CouponCode != null)
        {
            coupon = await _unitOfWork.Repository<AppCoupon>().GetQueryable()
                .Where(x => x.Code == newOrderInfoDto.CouponCode).FirstOrDefaultAsync();
            if (coupon == null) return (null, "coupon not valid");
        }

        if (coupon != null)
        {
            if (newOrderInfoDto.PaymentType != PaymentType.Points)
            {
                if (coupon.Type == AppCouponType.FixedAmount) order.CouponValue = coupon.Value;
                else order.CouponValue = order.Subtotal * coupon.Value;
            }
        }

        basket.Items.Clear();
        return (order, "Done");
    }

    public async Task<ICollection<Order>> GetOrderWithStatus(OrderStatus status)
    {
        var spec = new OrderWithItemsSpecification(status);
        var orders = await _unitOfWork.Repository<Order>().ListWithSpecAsync(spec);
        return orders.ToList();
    }
}