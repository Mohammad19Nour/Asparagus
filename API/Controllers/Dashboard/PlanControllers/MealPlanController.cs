using AsparagusN.Data.Entities.Meal;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.DTOs.AdminPlanDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Helpers;
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
        if (plan == null) return Ok(new ApiResponse(400, "Day not found"));

        var res = _mapper.Map<AdminPlanDayDto>(plan);

        if (plan.PlanType == PlanTypeEnum.CustomMealPlan)
            res.Meals = await _getAdminSelectedMeals(plan);

        return Ok(new ApiOkResponse<AdminPlanDayDto>(res));
    }

    [HttpPost("meals/{dayId:int}")]
    public async Task<ActionResult<AdminPlanDayDto>> AddMeal(NewAdminPlanDto newAdminPlanDto, int dayId)
    {
        var ok = true;

        var spec = new AdminPlanSpecification(dayId);

        var planDay = await _unitOfWork.Repository<AdminPlanDay>().GetEntityWithSpec(spec);

        if (planDay == null) return Ok(new ApiResponse(404, messageEN: "Day not found"));


        if (!HelperFunctions.CanUpdate(planDay.AvailableDate))
            return Ok(new ApiResponse(403, "Can't update before two days or less"));

        ok = await _addMeals(newAdminPlanDto.MealIds, planDay);

        _unitOfWork.Repository<AdminPlanDay>().Update(planDay);
        await _unitOfWork.SaveChanges();

        if (!ok) return Ok(new ApiResponse(404, messageEN: "not found"));

        return Ok(new ApiOkResponse<AdminPlanDayDto>(_mapper.Map<AdminPlanDayDto>(planDay)));
    }

    [HttpDelete("meals/{mealId:int}")]
    public async Task<ActionResult<AdminPlanDayDto>> DeleteItems(int mealId, [FromQuery] int dayId)
    {
        var adminDaySpec = new AdminPlanDaysWithMealsSpecification(dayId);
        var adminDay = await _unitOfWork.Repository<AdminPlanDay>().GetEntityWithSpec(adminDaySpec);

        if (adminDay == null) return Ok(new ApiResponse(404, "day not found"));

        if (adminDay.PlanType == PlanTypeEnum.CustomMealPlan)
        {
            var ml = await _unitOfWork.Repository<Meal>().GetByIdAsync(mealId);

            if (ml == null) return Ok(new ApiResponse(404, "Meal not found"));

            if (adminDay.Meals.FirstOrDefault(c => c.MealId == mealId) == null)
            {
                Console.WriteLine(mealId);
                adminDay.Meals.Add(new AdminSelectedMeal { MealId = ml.Id });

                if (await _unitOfWork.SaveChanges())
                    return Ok(new ApiResponse(200));
                return Ok(new ApiResponse(400, "Failed to delete"));
            }

            return Ok(new ApiResponse(200));
        }

        var meal = await _unitOfWork.Repository<AdminSelectedMeal>().GetByIdAsync(mealId);

        if (meal == null)
            return Ok(new ApiResponse(404, "meal not found"));

        _unitOfWork.Repository<AdminSelectedMeal>().Delete(meal);

        if (!await _unitOfWork.SaveChanges()) return Ok(new ApiResponse(400, "Failed to delete meal"));


        var spec = new AdminPlanSpecification(meal.AdminPlanDayId);
        var planDay = await _unitOfWork.Repository<AdminPlanDay>().GetEntityWithSpec(spec);
        if (planDay == null) return Ok(new ApiResponse(404, messageEN: "Day not found"));

        if (planDay.PlanType == PlanTypeEnum.CustomMealPlan)
            return Ok(new ApiResponse(400, "Can't delete meal in custom plan"));
        if (!HelperFunctions.CanUpdate(planDay.AvailableDate))
            return Ok(new ApiResponse(403, "Can't update before two days or less"));

        return Ok(new ApiOkResponse<AdminPlanDayDto>(_mapper.Map<AdminPlanDayDto>(planDay)));
    }

    private async Task<bool> _addMeals(List<int> mealIds, AdminPlanDay planDay)
    {
        var meals = await _unitOfWork.Repository<Meal>().ListAllAsync();
        foreach (var id in mealIds)
        {
            if (planDay.PlanType != PlanTypeEnum.CustomMealPlan)
            {
                var meal = meals.FirstOrDefault(m => m.Id == id);
                if (meal is not { IsMealPlan: true }) return false;

                if (planDay.Meals.Any(x => x.MealId == meal.Id)) continue;
                planDay.Meals.Add(new AdminSelectedMeal { MealId = meal.Id });
            }
            else
            {
                var adminMeal = planDay.Meals.FirstOrDefault(c => c.MealId == id);
                if (adminMeal != null) planDay.Meals.Remove(adminMeal);
            }
        }

        return true;
    }
}