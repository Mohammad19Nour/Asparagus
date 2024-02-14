using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.UserDtos;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Entities.Identity;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AsparagusN.Specifications.UserSpecifications;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers.User.UserPlanControllers;

public partial class UserPlanController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public UserPlanController(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<UserPlanDto>> Get(PlanTypeEnum planType)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "user not found"));

        var spec = new UserPlanWithMealsDrinksAndExtrasSpecification(user.Id, planType);
        var res = await _unitOfWork.Repository<UserPlan>().GetEntityWithSpec(spec);

        return Ok(new ApiOkResponse<UserPlanDto>(_mapper.Map<UserPlanDto>(res)));
    }

    [HttpGet("{dayId:int}")]
    public async Task<ActionResult<UserPlanDayDto>> GetDay(int dayId)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "user not found"));

        var spec = new UserPlanDayWithMealsAndDrinksAndAllSpecification(user.Id, dayId);
        var res = await _unitOfWork.Repository<UserPlanDay>().GetEntityWithSpec(spec);

        return Ok(new ApiOkResponse<UserPlanDayDto>(_mapper.Map<UserPlanDayDto>(res)));
    }

    private async Task<AppUser?> _getUser()
    {
        var email = HttpContext.User.GetEmail();
        return await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}