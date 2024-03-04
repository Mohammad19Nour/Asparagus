using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs;
using AsparagusN.DTOs.SubscriptionDtos;
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


[Authorize(Roles = nameof(Roles.User))]
public class SubscriptionsController : BaseApiController
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly ICustomSubscriptionService _customSubscriptionService;


    public SubscriptionsController(ICustomSubscriptionService customSubscriptionService,
        ISubscriptionService subscriptionService, IUnitOfWork unitOfWork, IMapper mapper,
        UserManager<AppUser> userManager)
    {
        _subscriptionService = subscriptionService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
        _customSubscriptionService = customSubscriptionService;
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
    public async Task<ActionResult<UserPlanDto>> CreateSubscription(NewCustomSubscriptionDto subscriptionDto)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "user not found"));

        var normal = new NewSubscriptionDto();
        if (subscriptionDto.PlanType != PlanTypeEnum.CustomMealPlan)
            normal = _mapper.Map<NewSubscriptionDto>(subscriptionDto);

        (UserPlan? plan, string message) = (null, "");
        if (subscriptionDto.PlanType != PlanTypeEnum.CustomMealPlan)
            (plan, message) = await _subscriptionService.CreateSubscriptionAsync(normal, user);
        else
            (plan, message) = await _customSubscriptionService.CreateSubscriptionAsync(subscriptionDto, user);

        if (plan == null)
            return Ok(new ApiResponse(400, message));

        return Ok(new ApiOkResponse<UserPlanDto>(_mapper.Map<UserPlanDto>(plan)));
    }

    [HttpPut]
    public async Task<ActionResult<UserPlanDto>> UpdateSubscription(UpdateSubscriptionDto subscriptionDto)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "user not found"));

        (UserPlan? plan, string message) = (null, "");

        if (subscriptionDto.PlanType != PlanTypeEnum.CustomMealPlan)
            (plan, message) = await _subscriptionService.UpdateSubscription(subscriptionDto, user);
        else

            (plan, message) = await _customSubscriptionService.UpdateSubscription(subscriptionDto, user);

        if (plan == null) return Ok(new ApiResponse(400, message));
        return Ok(new ApiOkResponse<UserPlanDto>(_mapper.Map<UserPlanDto>(plan)));
    }

    [HttpPost("price/update")]
    public async Task<ActionResult<decimal>> GetUpdatePrice(UpdateSubscriptionDto dto)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "user not found"));

        (decimal? price, string message) = (null, "");

        if (dto.PlanType != PlanTypeEnum.CustomMealPlan)
            (price, message) = await _subscriptionService.GetPriceForUpdate(dto, user);
        else (price, message) = await _customSubscriptionService.GetPriceForUpdate(dto, user);

        return Ok(price == null ? new ApiResponse(400, message) : new ApiOkResponse<decimal>(price.Value));
    }

    [HttpPost("price/add")]
    public async Task<ActionResult<decimal>> GetAddPrice(NewCustomSubscriptionDto dto)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "user not found"));
        (decimal? price, string message) = (null, "");

        NewSubscriptionDto normal = null;
        if (dto.PlanType != PlanTypeEnum.CustomMealPlan)
            normal = _mapper.Map<NewSubscriptionDto>(dto);
        
        if (dto.PlanType != PlanTypeEnum.CustomMealPlan)
            (price, message) = await _subscriptionService.GetPriceForCreate(normal, user);
        else (price, message) = await _customSubscriptionService.GetPriceForCreate(dto, user);

        return Ok(price == null ? new ApiResponse(400, message) : new ApiOkResponse<decimal>(price.Value));
    }

    private async Task<AppUser?> _getUser()
    {
        var email = HttpContext.User.GetEmail();
        return await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}