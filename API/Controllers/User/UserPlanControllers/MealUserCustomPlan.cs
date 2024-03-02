using AsparagusN.Data.Entities.Meal;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.MealDtos;
using AsparagusN.DTOs.PackageDtos;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Helpers;
using AsparagusN.Specifications;
using AsparagusN.Specifications.AdminPlanSpecifications;
using AsparagusN.Specifications.UserSpecifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.User.UserPlanControllers;

[Authorize]
public partial class UserPlanController
{
    [HttpPost("meals/custom")]
    public async Task<ActionResult> AddMealForCustomPlan(int mealId, int dayId)
    {
        var user = await _getUser();
        if (user == null) return (Ok(new ApiResponse(401)));

        var spec = new UserPlanDayWithMealsOnlySpecification(user.Id, dayId);
        var planDay = await _unitOfWork.Repository<UserPlanDay>().GetEntityWithSpec(spec);

        if (planDay == null || planDay.UserPlan.PlanType != PlanTypeEnum.CustomMealPlan)
            return Ok(new ApiResponse(404, "Plan day not found"));

        if (!HelperFunctions.CanUpdate(planDay.Day.Date))
            return Ok(new ApiResponse(403, "Can't update before two days or less"));

        var allowedMeals = planDay.UserPlan.NumberOfMealPerDay;
        var usedMeal = planDay.SelectedMeals.Count;

        if (allowedMeals - usedMeal <= 0)
            return Ok(new ApiResponse(400, "You have already reached your day limit "));

        var meal = await _unitOfWork.Repository<Meal>().GetByIdAsync(mealId);

        if (meal == null)
            return Ok(new ApiResponse(400, "Meal is not available"));

        var adminDaySpec = new AdminDayFromDateSpecification(planDay.Day.Date, PlanTypeEnum.CustomMealPlan);
        var adminDay = await _unitOfWork.Repository<AdminPlanDay>().GetEntityWithSpec(adminDaySpec);

        if (adminDay == null)
            return Ok(new ApiResponse(404, "The corresponding day in admin days not found"));


        var specUn = new UnavailableMealsInAdminDaySpecification(dayId, mealId);
        var unavailableMeal = await _unitOfWork.Repository<UnavailableMeal>().GetEntityWithSpec(specUn);

        if (unavailableMeal != null)
            return Ok(new ApiResponse(400, "This meal is unavailable"));

        var selectedMeal = _mapper.Map<UserSelectedMeal>(meal);
        selectedMeal.PricePerCarb = 0;
        selectedMeal.PricePerProtein = 0;
        selectedMeal.Protein = planDay.UserPlan.ProteinPerMealForCustomPlan;
        selectedMeal.Carbs = planDay.UserPlan.CarbPerMealForCustomPlan;
        planDay.SelectedMeals.Add(selectedMeal);

        _unitOfWork.Repository<UserPlanDay>().Update(planDay);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<UserPlanDayDto>(_mapper.Map<UserPlanDayDto>(planDay)));
        return Ok(new ApiResponse(400, "Failed to add meal"));
    }

    [HttpGet("meals/custom")]
    public async Task<ActionResult<List<MealIfoForCustomPlanDto>>> GetMealsForCustom(int dayId)
    {
        var user = await _getUser();
        if (user == null) return (Ok(new ApiResponse(401)));

        var planDay = await _unitOfWork.Repository<UserPlanDay>().GetByIdAsync(dayId);

        if (planDay == null || planDay.UserPlan.PlanType != PlanTypeEnum.CustomMealPlan)
            return Ok(new ApiResponse(404, "Plan day not found"));

        var adminDaySpec = new AdminDayFromDateSpecification(planDay.Day.Date, PlanTypeEnum.CustomMealPlan);
        var adminDay = await _unitOfWork.Repository<AdminPlanDay>().GetEntityWithSpec(adminDaySpec);

        if (adminDay == null)
            return Ok(new ApiResponse(404, "The corresponding day in admin days not found"));


        var specUn = new UnavailableMealsInAdminDaySpecification(adminDay.Id);
        var unavailableMeals = await _unitOfWork.Repository<UnavailableMeal>().ListWithSpecAsync(specUn);

        var meals = await _unitOfWork.Repository<Meal>().ListAllAsync();

        meals = meals.Where(m => unavailableMeals.All(un => un.MealId != m.Id)).ToList();
        return Ok(new ApiOkResponse<List<MealIfoForCustomPlanDto>>(_mapper.Map<List<MealIfoForCustomPlanDto>>(meals)));
    }
}