using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers.User;

[Authorize]
public class SubscriptionsController : BaseApiController
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public SubscriptionsController(ISubscriptionService subscriptionService, IUnitOfWork unitOfWork, IMapper mapper,
        UserManager<AppUser> userManager)
    {
        _subscriptionService = subscriptionService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpGet("id")]
    public async Task<ActionResult> Subscriptions()
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "user not found"));

        var subs = await _subscriptionService.GetAllUserSubscriptionsAsync(user);

        if (subs.Count == 0) return Ok(new ApiOkResponse<int?>(null));
        int id = (int)subs.First().PlanType;
        return Ok(new ApiOkResponse<int>(id));
    }

    [HttpGet]
    public async Task<ActionResult<SubscriptionDto>> Subscription(PlanTypeEnum planType)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "user not found"));

        var subs = await _subscriptionService.GetUserSubscriptionAsync(user, planType);

        if (subs == null) return Ok(new ApiResponse(404, "You dont have subscription"));
        return Ok(new ApiOkResponse<SubscriptionDto>(_mapper.Map<SubscriptionDto>(subs)));
    }

    [HttpPost]
    public async Task<ActionResult<UserPlanDto>> CreateSubscription(NewSubscriptionDto subscriptionDto)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "user not found"));

        var (plan, message) = await _subscriptionService.CreateSubscriptionAsync(subscriptionDto, user);

        if (plan == null)
            return Ok(new ApiResponse(400, message));

        return Ok(new ApiOkResponse<UserPlanDto>(_mapper.Map<UserPlanDto>(plan)));
    }

    [HttpPut]
    public async Task<ActionResult<UserPlanDto>> UpdateSubscription(UpdateSubscriptionDto subscriptionDto)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "user not found"));

        var (plan, message) = await _subscriptionService.UpdateSubscription(subscriptionDto, user);

        if (plan == null) return Ok(new ApiResponse(400, message));
        return Ok(new ApiOkResponse<UserPlanDto>(_mapper.Map<UserPlanDto>(plan)));
    }

    [HttpPost("price/update")]
    public async Task<ActionResult<decimal>> GetUpdatePrice(UpdateSubscriptionDto dto)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "user not found"));

        var res = await _subscriptionService.GetPriceForUpdate(dto, user);

        return Ok(res.Price == null ? new ApiResponse(400, res.Message) : new ApiOkResponse<decimal>(res.Price.Value));
    }

    [HttpPost("price/add")]
    public async Task<ActionResult<decimal>> GetAddPrice(NewSubscriptionDto dto)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "user not found"));
        var res = await _subscriptionService.GetPriceForCreate(dto, user);

        return Ok(res.Price == null ? new ApiResponse(400, res.Message) : new ApiOkResponse<decimal>(res.Price.Value));
    }

    private async Task<AppUser?> _getUser()
    {
        var email = HttpContext.User.GetEmail();
        return await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}