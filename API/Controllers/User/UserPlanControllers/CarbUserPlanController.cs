using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.CarbDtos;
using AsparagusN.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers.User.UserPlanControllers;

public partial class UserPlanController
{
    [HttpGet("carb/{mealId:int}")]
    public async Task<ActionResult> GetCarbOfMeal(int mealId)
    {
        try
        {
            var meal = await _unitOfWork.Repository<UserSelectedMeal>().GetByIdAsync(mealId);

            if (meal == null)
                return Ok(new ApiResponse(404, "Meal not found"));
            if (meal.ChangedCarbId == 0)
                return Ok(new ApiResponse(404, "you didn't change the carb of this meal"));

            return Ok(new ApiOkResponse<CarbDto>(_mapper.Map<CarbDto>(meal.ChangedCarb)));
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
            var meal = await _unitOfWork.Repository<UserSelectedMeal>().GetByIdAsync(mealId);

            if (meal == null)
                return Ok(new ApiResponse(404, "Meal not found"));

            var carb = await _unitOfWork.Repository<AdminSelectedCarb>().GetByIdAsync(adminCarbId);

            if (carb == null) return Ok(new ApiResponse(404, "Carb not found"));

            meal.ChangedCarbId = carb.Id;
            _unitOfWork.Repository<UserSelectedMeal>().Update(meal);
            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiOkResponse<CarbDto>(_mapper.Map<CarbDto>(carb)));
            return Ok(new ApiResponse(400, "Failed to update carb"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpDelete("carb/{mealId:int}")]
    public async Task<ActionResult> DeleteCarbOfMeal(int mealId)
    {
        try
        {
            var meal = await _unitOfWork.Repository<UserSelectedMeal>().GetQueryable().Where(x => x.Id == mealId)
                .Include(c => c.ChangedCarb).FirstOrDefaultAsync();

            if (meal == null)
                return Ok(new ApiResponse(404, "Meal not found"));

            if (meal.ChangedCarb != null)
                _unitOfWork.Repository<UserMealCarb>().Delete(meal.ChangedCarb);

            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiResponse(200));
            return Ok(new ApiResponse(400, "Failed to update carb"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}