using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.Meal;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.DTOs.OrderDtos;
using AsparagusN.Enums;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AsparagusN.Specifications.OrdersSpecifications;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Services;

public class OrderService : IOrderService
{
    private readonly ILocationService _locationService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;
    private readonly IPaymentService _paymentService;

    public OrderService(ILocationService locationService, IMapper mapper, IUnitOfWork unitOfWork,
        INotificationService notificationService, IPaymentService paymentService)
    {
        _locationService = locationService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
        _paymentService = paymentService;
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

    public async Task<(Order? Order, string Message)> CreateOrderAsync(string buyerEmail, int basketId,
        NewOrderInfoDto newOrderInfoDto)
    {
        try
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                if (newOrderInfoDto.PaymentType == PaymentType.Gift) return (null, "Wrong payment type");
                var user = await _unitOfWork.Repository<AppUser>().GetByIdAsync(basketId);

                Order? order;
                (order, var message) = await CalcPriceOfOrder(buyerEmail, basketId, newOrderInfoDto);

                if (order == null) return (order, Message: message);

                if (order.PaymentType == PaymentType.Points)
                {
                    if (order.PointsPrice > user!.LoyaltyPoints)
                        return (null, "You don't enough points");
                    user.LoyaltyPoints -= order.PointsPrice;
                }

                if (order.PaymentType != PaymentType.Points)
                    user.LoyaltyPoints += order.GainedPoints;


                order.BuyerPhoneNumber = user.PhoneNumber;
                order.BuyerId = user.Id;
                
                if (order.PaymentType == PaymentType.Card)
                {
                    if (newOrderInfoDto.BillId == null)
                        return (null, "Transaction_id must be provided");
                    var paymentResult = await
                        _paymentService.CheckPaymentStatus(newOrderInfoDto.BillId, (double)order.GetTotal());
                    if (!paymentResult.Success)
                        return (null, paymentResult.Message);
                    order.BillId = newOrderInfoDto.BillId;
                }

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

    public async Task<(bool Success, string Message)> CreateGiftOrderAsync(AppUser user, int mealId)
    {
        try
        {
            var meal = await _unitOfWork.Repository<Meal>().GetByIdAsync(mealId);
            var mealOrdered = _mapper.Map<MealItemOrdered>(meal);
            var items = new List<OrderItem> { new OrderItem { OrderedMeal = mealOrdered } };
            var branchId =
                await _locationService.GetClosestBranch(user.HomeAddress.Latitude, user.HomeAddress.Longitude);

            if (branchId == 0) return (true, "ok");
            var order = new Order
            {
                Items = items,
                BuyerEmail = user.Email,
                BuyerPhoneNumber = user.PhoneNumber,
                BranchId = branchId,
                PaymentType = PaymentType.Gift,
                BuyerId = user.Id
            };
            _unitOfWork.Repository<Order>().Add(order);
            if (await _unitOfWork.SaveChanges())
                return (true, "Done");
            return (false, "Failed");
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
                else order.CouponValue = order.Subtotal * coupon.Value/100;
            }
        }
        Console.WriteLine(coupon == null);

        basket.Items.Clear();
        return (order, "Done");
    }

    public async Task<ICollection<Order>> GetOrderWithStatus(OrderStatus status)
    {
        var spec = new OrderWithItemsSpecification(status);
        var orders = await _unitOfWork.Repository<Order>().ListWithSpecAsync(spec);
        return orders.ToList();
    }

    public async Task<(Order? Order, string Message)> CreateCashierOrderAsync(string userEmail, int mealId,
        string cashierEmail, int quantity)
    {
        cashierEmail = cashierEmail.ToLower();
        userEmail = userEmail.ToLower();

        var cashier = await _unitOfWork.Repository<Cashier>().GetQueryable().Where(c => c.Email == cashierEmail)
            .FirstOrDefaultAsync();
        if (cashier == null) return (null, $"Cashier with email {userEmail} not found ");


        var user = await _unitOfWork.Repository<AppUser>().GetQueryable().Where(u => u.Email.ToLower() == userEmail)
            .FirstOrDefaultAsync();
        if (user == null) return (null, $"User with email {userEmail} not found ");

        var meal = await _unitOfWork.Repository<Meal>().GetByIdAsync(mealId);

        if (meal == null) return (null, "Meal not found");

        if (meal.LoyaltyPoints == null) return (null, "You can't buy this meal");

        if (user.LoyaltyPoints < quantity * meal.LoyaltyPoints) return (null, "User doesn't have enough points");
        user.LoyaltyPoints -= meal.LoyaltyPoints.Value * quantity;

        var orderItem = new OrderItem();
        var mealOrdered = _mapper.Map<MealItemOrdered>(meal);

        orderItem.OrderedMeal = mealOrdered;
        orderItem.Quantity = quantity;
        orderItem.PointsPrice = meal.LoyaltyPoints.Value;

        var order = new Order
        {
            BuyerEmail = userEmail,
            BranchId = user.Id,
            BuyerPhoneNumber = user.PhoneNumber,
            Items = new List<OrderItem> { orderItem },
            BuyerId = cashier.BranchId,
            PointsPrice = orderItem.PointsPrice * orderItem.Quantity,
            PaymentType = PaymentType.Points
        };
        _unitOfWork.Repository<Order>().Add(order);

        if (await _unitOfWork.SaveChanges())
        {
            await _notificationService.NotifyUserByEmail(user.Email, "تم اضافة الطلب", "Order added");

            return (order, "Done");
        }

        return (null, "Failed to make an order");
    }
}