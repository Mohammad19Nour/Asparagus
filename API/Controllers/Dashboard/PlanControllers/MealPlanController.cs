using AsparagusN.Data.Entities.Meal;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.DTOs.AdminPlanDtos;
using AsparagusN.Errors;
using AsparagusN.Specifications;
using AsparagusN.Specifications.AdminPlanSpecifications;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard.PlanControllers;

public partial class PlanController
{
    [HttpGet("meals")]
    public async Task<ActionResult<AdminPlanDayDto>> GetMeals(int dayId)
    {
        var spec = new AdminPlanSpecification(dayId);

        var plan = await _unitOfWork.Repository<AdminPlanDay>().GetEntityWithSpec(spec);

        return Ok(new ApiOkResponse<AdminPlanDayDto>(_mapper.Map<AdminPlanDayDto>(plan)));
    }

    [HttpPost("meals/{dayId:int}")]
    public async Task<ActionResult<AdminPlanDayDto>> AddMeal(NewAdminPlanDto newAdminPlanDto, int dayId)
    {
        var ok = true;

        var spec = new AdminPlanSpecification(dayId);

        var plan = await _unitOfWork.Repository<AdminPlanDay>().GetEntityWithSpec(spec);

        if (plan == null) return Ok(new ApiResponse(404, messageEN: "Plan not found"));

        ok = await _addMeals(newAdminPlanDto.MealIds, plan);

        _unitOfWork.Repository<AdminPlanDay>().Update(plan);
        await _unitOfWork.SaveChanges();

        if (!ok) return Ok(new ApiResponse(404, messageEN: "not found"));

        return Ok(new ApiOkResponse<AdminPlanDayDto>(_mapper.Map<AdminPlanDayDto>(plan)));
    }

    [HttpDelete("meals/{mealId:int}")]
    public async Task<ActionResult<AdminPlanDayDto>> DeleteItems(int mealId)
    {
        var meal = await _unitOfWork.Repository<AdminSelectedMeal>().GetByIdAsync(mealId);

        if (meal == null)
            return Ok(new ApiResponse(404, "meal not found"));

        _unitOfWork.Repository<AdminSelectedMeal>().Delete(meal);

        if (!await _unitOfWork.SaveChanges()) return Ok(new ApiResponse(400, "Failed to delete meal"));


        var spec = new AdminPlanSpecification(meal.AdminPlanDayId);
        var plan = await _unitOfWork.Repository<AdminPlanDay>().GetEntityWithSpec(spec);
        return Ok(new ApiOkResponse<AdminPlanDayDto>(_mapper.Map<AdminPlanDayDto>(plan)));
    }

    private async Task<bool> _addMeals(List<int> mealIds, AdminPlanDay planDay)
    {
        foreach (var id in mealIds)
        {
            var meal = await _unitOfWork.Repository<Meal>().GetByIdAsync(id);

            if (meal is not { IsMealPlan: true }) return false;

            if (planDay.Meals.Any(x => x.MealId == meal.Id)) continue;
            planDay.Meals.Add(new AdminSelectedMeal { MealId = meal.Id });
        }

        return true;
    }
}