using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.Meal;
using AsparagusN.DTOs;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers.User;

public class LoyaltyPointsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public LoyaltyPointsController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpGet("meals")]
    public async Task<ActionResult<List<MealLoyaltyPointDto>>> GetMeals()
    {
        var spec = new BaseSpecification<Meal>(x =>
            x.LoyaltyPoints != null && x.IsMainMenu && !x.Category.NameEN.ToLower().StartsWith("snack"));
        spec.AddInclude(x => x.Include(y => y.Allergies));
        spec.AddInclude(x => x.Include(y => y.Category));


        var meals = await _unitOfWork.Repository<Meal>().ListWithSpecAsync(spec);

        var result = _mapper.Map<List<MealLoyaltyPointDto>>(meals);
        return Ok(new ApiOkResponse<List<MealLoyaltyPointDto>>(result));
    }

    [HttpGet("snacks")]
    public async Task<ActionResult<List<MealLoyaltyPointDto>>> GetSnacks()
    {
        var spec = new BaseSpecification<Meal>(x =>
            x.LoyaltyPoints != null && x.IsMainMenu && x.Category.NameEN.ToLower().StartsWith("snack"));
        spec.AddInclude(x => x.Include(y => y.Allergies));
        spec.AddInclude(x => x.Include(y => y.Category));
        var meals = await _unitOfWork.Repository<Meal>().ListWithSpecAsync(spec);

        var result = _mapper.Map<List<MealLoyaltyPointDto>>(meals);
        return Ok(new ApiOkResponse<List<MealLoyaltyPointDto>>(result));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<MealLoyaltyPointDto>> Replace(int mealId)
    {
        var email = User.GetEmail();
        var user = await _userManager.Users.FirstAsync(x => x.Email.ToLower() == email);

        var meal = await _unitOfWork.Repository<Meal>().GetByIdAsync(mealId);
        if (meal == null) return Ok(new ApiResponse(404, "Meal not found"));
        if (meal.LoyaltyPoints == null) return Ok(new ApiResponse(400, "Meal doesn't have loyalty points"));

        if (user.LoyaltyPoints < meal.LoyaltyPoints)
            return Ok(new ApiResponse(400, "You don't have enough points"));

        user.LoyaltyPoints -= meal.LoyaltyPoints.Value;

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<MealLoyaltyPointDto>(_mapper.Map<MealLoyaltyPointDto>(meal)));
        return Ok(new ApiResponse(400, "Failed to replace points"));
    }
}