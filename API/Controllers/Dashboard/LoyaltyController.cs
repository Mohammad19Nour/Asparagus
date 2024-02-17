using AsparagusN.Data.Entities.Meal;
using AsparagusN.DTOs;
using AsparagusN.DTOs.MealDtos;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers.Dashboard;

public class LoyaltyController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LoyaltyController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet("meals")]
    public async Task<ActionResult<List<MealLoyaltyPointDto>>> GetMeals()
    {
        var spec = new BaseSpecification<Meal>(x => x.LoyaltyPoints == null);
        spec.AddInclude(x => x.Include(y => y.Allergies));

        var meals = await _unitOfWork.Repository<Meal>().ListWithSpecAsync(spec);

        var result = _mapper.Map<List<MealLoyaltyPointDto>>(meals);
        return Ok(new ApiOkResponse<List<MealLoyaltyPointDto>>(result));
    }

    [HttpPost]
    public async Task<ActionResult<MealLoyaltyPointDto>> AddMeal(int mealId, int points)
    {
        var meal = await _unitOfWork.Repository<Meal>().GetByIdAsync(mealId);
        if (meal == null) return Ok(new ApiResponse(404, "Meal not found"));

        meal.LoyaltyPoints = points;
        _unitOfWork.Repository<Meal>().Update(meal);
        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<MealLoyaltyPointDto>(_mapper.Map<MealLoyaltyPointDto>(meal)));
        return Ok(new ApiResponse(400, "Failed to add points"));
    }


    [HttpDelete]
    public async Task<ActionResult> Delete(int mealId)
    {
        var meal = await _unitOfWork.Repository<Meal>().GetByIdAsync(mealId);
        if (meal == null) return Ok(new ApiResponse(404, "Meal not found"));

        if (meal.LoyaltyPoints == null) return Ok(new ApiResponse(400, "Meal doesn't have loyalty points"));
      
        meal.LoyaltyPoints = null;
        _unitOfWork.Repository<Meal>().Update(meal);
        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, "Failed to delete points"));
    }
}