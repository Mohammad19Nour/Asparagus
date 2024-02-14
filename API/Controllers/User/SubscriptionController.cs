using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Entities.Identity;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers.User;

public class SubscriptionController : BaseApiController
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public SubscriptionController(ISubscriptionService subscriptionService, IUnitOfWork unitOfWork, IMapper mapper,
        UserManager<AppUser> userManager)
    {
        _subscriptionService = subscriptionService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
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

    [HttpPut("updateDuration")]
    public async Task<ActionResult<UserPlanDto>> UpdateDuration(int newDuration, PlanTypeEnum planType)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "user not found"));

        var (plan, message) = await _subscriptionService.UpdateDuration(user.Id, planType, newDuration);

        if (plan == null) return Ok(new ApiResponse(400, message));
        return Ok(new ApiOkResponse<UserPlanDto>(_mapper.Map<UserPlanDto>(plan)));
    }

    private async Task<AppUser?> _getUser()
    {
        var email = HttpContext.User.GetEmail();
        return await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}