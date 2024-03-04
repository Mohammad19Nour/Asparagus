using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.CarbDtos;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers.User.UserPlanControllers;

[Authorize(Roles = nameof(Roles.User))]
public partial class UserPlanController
{
    [HttpGet("carb/{mealId:int}")]
    public async Task<ActionResult> GetCarbOfMeal(int mealId)
    {
        try
        {
            var meal = await _unitOfWork.Repository<UserSelectedMeal>().GetQueryable()
                .Include(x => x.ChangedCarb)
                .Where(c => c.Id == mealId)
                .FirstOrDefaultAsync();
            
            if (meal == null)
                return Ok(new ApiResponse(404, "Meal not found"));
         
            return Ok(new ApiOkResponse<UserMealCarbDto>(_mapper.Map<UserMealCarbDto>(meal.ChangedCarb)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost("carb/{mealId:int}")]
    public async Task<ActionResult> EditCarbOfMeal(int mealId, int adminCarbId)
    {
        try
        {
            var meal = await _unitOfWork.Repository<UserSelectedMeal>()
                .GetQueryable()
                .Include(c=>c.ChangedCarb)
                .Where(m=>m.Id == mealId).FirstOrDefaultAsync();

            if (meal == null)
                return Ok(new ApiResponse(404, "Meal not found"));

            var carb = await _unitOfWork.Repository<AdminSelectedCarb>().GetQueryable()
                .Where(x => x.Id == adminCarbId)
                .Include(x => x.Carb).FirstOrDefaultAsync();

            if (carb == null) return Ok(new ApiResponse(404, "Carb not found"));

           HelperFunctions.CalcNewPropertyForCarb(meal,carb.Carb);
            _unitOfWork.Repository<UserSelectedMeal>().Update(meal);
            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiOkResponse<UserSelectedMealDto>(_mapper.Map<UserSelectedMealDto>(meal)));
            
            return Ok(new ApiResponse(400, "Failed to update carb"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}