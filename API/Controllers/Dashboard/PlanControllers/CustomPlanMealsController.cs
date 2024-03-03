using AsparagusN.Data.Entities.Meal;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.DTOs.MealDtos;
using AsparagusN.Errors;
using AsparagusN.Specifications;
using AsparagusN.Specifications.AdminPlanSpecifications;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard.PlanControllers;

public partial class PlanController
{
    [HttpGet("custom/all")]
    public async Task<ActionResult<List<MealIfoForCustomPlanDto>>> GetAllMealInDay(int dayId)
    {
        var adminDay = await _unitOfWork.Repository<AdminPlanDay>().GetByIdAsync(dayId);

        if (adminDay == null) return Ok(new ApiResponse(404, "day not found"));

        var mealsToReturn = await _getAllMealsForAdminCustom(adminDay.Id);

        return Ok(new ApiOkResponse<List<MealIfoForCustomPlanDto>>(mealsToReturn));
    }

    private async Task<List<MealIfoForCustomPlanDto>> _getAllMealsForAdminCustom(int dayId)
    {
        var spec = new AdminPlanDaysWithMealsSpecification(dayId);
        var unavailableMeals = (await _unitOfWork.Repository<AdminPlanDay>().GetEntityWithSpec(spec))?.Meals;
        var meals = await _unitOfWork.Repository<Meal>().ListAllAsync();

        var mealsToReturn = _mapper.Map<List<MealIfoForCustomPlanDto>>(meals);

        foreach (var ml in mealsToReturn)
        {
            if (unavailableMeals != null) 
                ml.IsAvailable = unavailableMeals.All(c => c.MealId != ml.Id);
        }

        return mealsToReturn;
    }
}