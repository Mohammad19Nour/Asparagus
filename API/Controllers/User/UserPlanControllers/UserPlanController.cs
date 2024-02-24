using System.ComponentModel.DataAnnotations;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.Meal;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Helpers;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AsparagusN.Specifications.UserSpecifications;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers.User.UserPlanControllers;

[Authorize]
public partial class UserPlanController : BaseApiController
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public UserPlanController(ISubscriptionService subscriptionService, UserManager<AppUser> userManager,
        IUnitOfWork unitOfWork, IMapper mapper)
    {
        _subscriptionService = subscriptionService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<UserPlanDto>> Get(PlanTypeEnum planType)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "user not found"));

        var plan = await _subscriptionService.GetUserSubscriptionAsync(user, planType);

        if (plan == null) return Ok(new ApiResponse(404, "No plan found"));

        Console.WriteLine("RR\n\n");
        plan.Days = plan.Days.Where(x => HelperFunctions.InThisWeek(x.Day.Date)).ToList();
        var result = _mapper.Map<UserPlanDto>(plan);
        foreach (var day in result.Days)
        {
            await AddAdminDayId(planType, day);
        }

        return Ok(new ApiOkResponse<UserPlanDto>(result));
    }

    private async Task AddAdminDayId(PlanTypeEnum planType, UserPlanDayDto day)
    {
        var adminDay = await _unitOfWork.Repository<AdminPlanDay>().GetQueryable()
            .Where(c => c.PlanType == planType && c.AvailableDate == day.Day)
            .FirstOrDefaultAsync();

        if (adminDay != null) day.AdminDayId = adminDay.Id;
    }

    [HttpGet("{dayId:int}")]
    public async Task<ActionResult<UserPlanDayDto>> GetDay(int dayId)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "user not found"));

        var spec = new UserPlanDayWithMealsAndDrinksAndAllSpecification(user.Id, dayId);
        var res = await _unitOfWork.Repository<UserPlanDay>().GetEntityWithSpec(spec);

        if (res == null) return Ok(new ApiResponse(404, "Day not found"));

        if (HelperFunctions.InThisWeek(res.Day.Date))
            return Ok(new ApiResponse(404, "day not available yet"));
        var day = _mapper.Map<UserPlanDayDto>(res);

        await AddAdminDayId(res.UserPlan.PlanType, day);
        return Ok(new ApiOkResponse<UserPlanDayDto>(day));
    }

    [HttpPut]
    public async Task<ActionResult<UserPlanDto>> UpdatePlan(UserPlanInfoDto updatePlanDto)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "user not found"));
        var spec = new UserPlanWithMealsDrinksAndExtrasSpecification(user.Id, updatePlanDto.PlanType);
        var plan = await _unitOfWork.Repository<UserPlan>().GetEntityWithSpec(spec);

        if (plan == null) return Ok(new ApiResponse(404, "Plan not found"));
        _mapper.Map(updatePlanDto, plan);
        var res = await AddAllergies(updatePlanDto.Allergies, plan);

        if (!res.Success) return Ok(new ApiResponse(400, res.Message));
        _unitOfWork.Repository<UserPlan>().Update(plan);

        var result = _mapper.Map<UserPlanDto>(plan);
        foreach (var day in result.Days)
        {
            await AddAdminDayId(updatePlanDto.PlanType, day);
        }

        if (await _unitOfWork.SaveChanges()) return Ok(new ApiOkResponse<UserPlanDto>(result));
        return Ok(new ApiResponse(400, "Failed to update plan"));
    }

    [HttpPost("period")]
    public async Task<ActionResult> UpdatePeriod(int dayId, Period period)
    {
        var planDay = await _unitOfWork.Repository<UserPlanDay>().GetByIdAsync(dayId);

        if (planDay == null)
            return Ok(new ApiResponse(404, "Day not found"));

        planDay.DeliveryPeriod = period;
        _unitOfWork.Repository<UserPlanDay>().Update(planDay);
        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200, "Done"));

        return Ok(new ApiResponse(400, "Failed to update period"));
    }
    [HttpPost("delivery")]
    public async Task<ActionResult> UpdateDeliveryAddress(int dayId,[Range(1,2)] int addressType)
    {
        var user = await _getUser();
        
        var planDay = await _unitOfWork.Repository<UserPlanDay>().GetByIdAsync(dayId);

        if (planDay == null)
            return Ok(new ApiResponse(404, "Day not found"));

        planDay.DeliveryLocationId = addressType == 1 ? user!.HomeAddressId : user!.WorkAddressId;
        planDay.IsHomeAddress = addressType == 1;

        _unitOfWork.Repository<UserPlanDay>().Update(planDay);
        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200, "Done"));

        return Ok(new ApiResponse(400, "Failed to update period"));
    }

    private async Task<AppUser?> _getUser()
    {
        var email = HttpContext.User.GetEmail();
        return await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    private async Task<(bool Success, string Message)> AddAllergies(List<int>? selectedAllergyIds, UserPlan plan)
    {
        if (selectedAllergyIds == null) return (true, "");

        var allergies = await _getAllergies(selectedAllergyIds);

        if (allergies == null)
            return (false, "One or more allergy not exist");

        plan.Allergies = _mapper.Map<List<UserPlanAllergy>>(allergies);
        return (true, "");
    }

    private async Task<List<Allergy>?> _getAllergies(List<int> allergyIds)
    {
        allergyIds = allergyIds.Distinct().ToList();
        var dbAllergies = await _unitOfWork.Repository<Allergy>().ListAllAsync();
        dbAllergies = dbAllergies.Where(x => allergyIds.Contains(x.Id)).ToList();

        return (dbAllergies.Count == allergyIds.Count) ? dbAllergies.ToList() : null;
    }
}