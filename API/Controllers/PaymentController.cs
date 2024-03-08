using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers;

public class PaymentController : BaseApiController
{
    private readonly IPaymentService _paymentService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IOrderService _orderService;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentController(IPaymentService paymentService, UserManager<AppUser> userManager,
        IOrderService orderService,IUnitOfWork unitOfWork)
    {
        _paymentService = paymentService;
        _userManager = userManager;
        _orderService = orderService;
        _unitOfWork = unitOfWork;
    }

    [Authorize]
    [HttpPost("checkout")]
    public async Task<ActionResult> Checkout()
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "User not found"));

        var order = await _orderService.GetOrderByIdAsync(user.Id, user.Email);
        if (order == null) return Ok(new ApiResponse(404, "No order found"));

        return Ok(new ApiOkResponse<object?>(await _paymentService.CreatePaymentIntent(order.Id)));
    }

    [HttpPost("webhook")]
    public async Task<ActionResult> FoloosiWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        
        var not = new Notification
        {
            UserEmail = "u@u.u",
            ArabicContent = "test hook",
            EnglishContent =json,

        };
         _unitOfWork.Repository<Notification>().Add(not);
        await _unitOfWork.SaveChanges();
        return Ok(not);
    }

    private async Task<AppUser?> _getUser()
    {
        var email = HttpContext.User.GetEmail();
        return await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}