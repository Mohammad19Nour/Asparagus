using AsparagusN.Data.Entities.Meal;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Helpers;
using AsparagusN.Specifications;
using AsparagusN.Specifications.AdminPlanSpecifications;
using AsparagusN.Specifications.UserSpecifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers.User.UserPlanControllers;

public partial class UserPlanController : BaseApiController
{
    [HttpGet("meals/{dayId:int}")]
    public async Task<ActionResult<List<UserSelectedMealDto>>> GetMeals(int dayId)
    {
        try
        {
            var user = await _getUser();
            if (user == null) return (Ok(new ApiResponse(401)));

            var spec = new UserPlanDayWithMealsOnlySpecification(user.Id, dayId);
            var planDay = await _unitOfWork.Repository<UserPlanDay>().GetEntityWithSpec(spec);

            if (planDay == null) return Ok(new ApiResponse(404, "Plan day not found"));

            return Ok(new ApiOkResponse<List<UserSelectedMealDto>>(
                _mapper.Map<List<UserSelectedMealDto>>(planDay.SelectedMeals)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost("meals/{dayId:int}")]
    public async Task<ActionResult> AddMeal(int dayId, int adminMealId)
    {
        try
        {
            var user = await _getUser();
            if (user == null) return (Ok(new ApiResponse(401)));

            var spec = new UserPlanDayWithMealsOnlySpecification(user.Id, dayId);
            var planDay = await _unitOfWork.Repository<UserPlanDay>().GetEntityWithSpec(spec);

            if (planDay == null) return Ok(new ApiResponse(404, "Plan day not found"));

            if (!HelperFunctions.CanUpdate(planDay.Day.Date))
                return Ok(new ApiResponse(403, "Can't update before two days or less"));


            var allowedMeals = planDay.UserPlan.NumberOfMealPerDay;
            var usedMeal = planDay.SelectedMeals.Count;

            if (allowedMeals - usedMeal <= 0)
                return Ok(new ApiResponse(400, "You have already reached your day limit "));

            var adminMealSpec =
                new AdminPlanDaysWithMealsSpecification(dayDate: planDay.Day, planDay.UserPlan.PlanType);

            var adminDay = await _unitOfWork.Repository<AdminPlanDay>().GetEntityWithSpec(adminMealSpec);

            if (adminDay == null) return Ok(new ApiResponse(400, "Admin day not found"));

            if (planDay.Day != adminDay.AvailableDate)
                return Ok(new ApiResponse(400, "Meal not found in this day"));

            if (planDay.UserPlan.PlanType != adminDay.PlanType)
                return Ok(new ApiResponse(400, "user plan type not as same as admin plan type"));


            var meal = adminDay.Meals.FirstOrDefault(x => x.Id == adminMealId);
            if (((planDay.UserPlan.PlanType == PlanTypeEnum.CustomMealPlan) && (meal != null)) ||
                (meal == null) && (planDay.UserPlan.PlanType != PlanTypeEnum.CustomMealPlan))
            {
                return Ok(new ApiResponse(404, "Meal not found"));
            }

            var mealToAdd = new UserSelectedMeal
            {
                UserPlanDay = planDay,
                UserPlanDayId = planDay.Id
            };

            _mapper.Map(meal.Meal, mealToAdd);
            var ingredients = await _unitOfWork.Repository<MealIngredient>().GetQueryable()
                .Include(x => x.Ingredient)
                .Where(x => x.MealId == meal.MealId).ToListAsync();

            var carb = ingredients.FirstOrDefault(x => x.Ingredient.TypeOfIngredient == IngredientType.Carb);

            var carbSelected = new UserMealCarb();
            if (carb != null)
            {
                _mapper.Map(carb.Ingredient, carbSelected);
                HelperFunctions.CalcNewPropertyForCarbOfMeal(carbSelected, carb.Weight, carb.Ingredient.Weight);
            }

            mealToAdd.ChangedCarb = carbSelected;

            mealToAdd.OriginalMealId = meal.MealId;
            if (planDay.UserPlan.PlanType == PlanTypeEnum.CustomMealPlan)
            {
                mealToAdd.PricePerCarb = 0;
                mealToAdd.PricePerProtein = 0;
                mealToAdd.Protein = planDay.UserPlan.ProteinPerMealForCustomPlan;
                mealToAdd.Carbs = planDay.UserPlan.CarbPerMealForCustomPlan;
            }
            planDay.SelectedMeals.Add(mealToAdd);

            _unitOfWork.Repository<UserPlanDay>().Update(planDay);
            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiOkResponse<UserSelectedMealDto>(_mapper.Map<UserSelectedMealDto>(mealToAdd)));


            return Ok(new ApiResponse(400, "Failed to add meal"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /* [HttpPut("meals/{dayId:int}")]
     public async Task<ActionResult> UpdateMeal(int dayId, UpdateUserMealDto updatedMeal)
     {
         try
         {
             var user = await _getUser();
             if (user == null) return (Ok(new ApiResponse(401)));

             var spec = new UserPlanDayWithMealsOnlySpecification(user.Id, dayId);
             var planDay = await _unitOfWork.Repository<UserPlanDay>().GetEntityWithSpec(spec);

             if (planDay == null) return Ok(new ApiResponse(404, "Plan day not found"));

             var oldMeal = planDay.SelectedMeals.FirstOrDefault(x => x.Id == updatedMeal.OldUserMealId);
             if (oldMeal == null) return Ok(new ApiResponse(404, "Meal not found in user selections"));

             var adminMealSpec =
                 new AdminPlanDaysWithMealsSpecification(dayDate: planDay.Day, planDay.UserPlan.PlanType);

             var adminDay = await _unitOfWork.Repository<AdminPlanDay>().GetEntityWithSpec(adminMealSpec);

             if (adminDay == null) return Ok(new ApiResponse(400, "Admin day not found"));

             if (planDay.UserPlan.PlanType != adminDay.PlanType)
                 return Ok(new ApiResponse(400, "user plan type not as same as admin plan type"));

             var newMeal = adminDay.Meals.FirstOrDefault(x => x.Id == updatedMeal.AdminMealId);
             if (newMeal == null) return Ok(new ApiResponse(404, "New meal not found"));

             var mealToAdd = new UserSelectedMeal
             {
                 UserPlanDay = planDay,
                 UserPlanDayId = planDay.Id
             };

             var dbId = oldMeal.Id;
             _mapper.Map(newMeal.Meal, oldMeal);
             oldMeal.Id = dbId;

             _unitOfWork.Repository<UserPlanDay>().Update(planDay);
             if (await _unitOfWork.SaveChanges())
                 return Ok(new ApiOkResponse<UserSelectedMealDto>(_mapper.Map<UserSelectedMealDto>(oldMeal)));

             return Ok(new ApiResponse(400, "Failed to add meal"));
         }
         catch (Exception e)
         {
             Console.WriteLine(e);
             throw;
         }
     }*/

    [HttpDelete("meals/{dayId:int}")]
    public async Task<ActionResult> DeleteMeal(int dayId, int mealId)
    {
        var user = await _getUser();
        if (user == null) return (Ok(new ApiResponse(401)));

        var spec = new UserPlanDayWithMealsOnlySpecification(user.Id, dayId);
        var planDay = await _unitOfWork.Repository<UserPlanDay>().GetEntityWithSpec(spec);

        if (planDay == null) return Ok(new ApiResponse(404, "Plan day not found"));


        if (!HelperFunctions.CanUpdate(planDay.Day.Date))
            return Ok(new ApiResponse(403, "Can't update before two days or less"));

        var meal = planDay.SelectedMeals.FirstOrDefault(x => x.Id == mealId);
        if (meal == null) return Ok(new ApiResponse(404, "Meal not found"));

        planDay.SelectedMeals.Remove(meal);

        if (await _unitOfWork.SaveChanges()) return Ok(new ApiResponse(200, "deleted"));
        return Ok(new ApiResponse(400, "Failed to delete meal"));
    }
}