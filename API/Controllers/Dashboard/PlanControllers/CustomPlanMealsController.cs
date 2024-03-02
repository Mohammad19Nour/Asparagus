using AsparagusN.Data.Entities.Meal;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.DTOs.MealDtos;
using AsparagusN.Errors;
using AsparagusN.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard.PlanControllers;

public partial class PlanController
{
    [HttpGet("custom/all")]
    public async Task<ActionResult<List<MealIfoForCustomPlanDto>>> GetAllMealInDay(int dayId)
    {
        var adminDay = await _unitOfWork.Repository<AdminPlanDay>().GetByIdAsync(dayId);

        if (adminDay == null) return Ok(new ApiResponse(404, "day not found"));

        var mealsToReturn = await _getMealsForCustom(adminDay.Id);

        return Ok(new ApiOkResponse<List<MealIfoForCustomPlanDto>>(mealsToReturn));
    }

    [HttpGet("custom/change")]
    public async Task<ActionResult> DisableMealBy(int dayId, int mealId)
    {
        var adminDay = await _unitOfWork.Repository<AdminPlanDay>().GetByIdAsync(dayId);

        if (adminDay == null) return Ok(new ApiResponse(404, "day not found"));
        var meal = await _unitOfWork.Repository<Meal>().GetByIdAsync(mealId);

        if (meal == null) return Ok(new ApiResponse(404, "Meal not found"));


        var spec = new UnavailableMealsInAdminDaySpecification(dayId, mealId);
        var unavailableMeal = await _unitOfWork.Repository<UnavailableMeal>().GetEntityWithSpec(spec);

        if (unavailableMeal == null)
        {
            _unitOfWork.Repository<UnavailableMeal>().Add(new UnavailableMeal { MealId = mealId, AdminDayId = dayId });
        }
        else
        {
            _unitOfWork.Repository<UnavailableMeal>().Delete(unavailableMeal);
        }

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200, "Done"));

        return Ok(new ApiResponse(400, "Failed to update meal"));
    }
    private async Task<List<MealIfoForCustomPlanDto>> _getMealsForCustom(int dayId)
    {
        var spec = new UnavailableMealsInAdminDaySpecification(dayId);
        var unavailableMeals = await _unitOfWork.Repository<UnavailableMeal>().ListWithSpecAsync(spec);
        var meals = await _unitOfWork.Repository<Meal>().ListAllAsync();

        var mealsToReturn = _mapper.Map<List<MealIfoForCustomPlanDto>>(meals);

        foreach (var ml in mealsToReturn)
        {
            ml.IsAvailable = unavailableMeals.All(c => c.MealId != ml.Id);
        }

        return mealsToReturn;
    }
}