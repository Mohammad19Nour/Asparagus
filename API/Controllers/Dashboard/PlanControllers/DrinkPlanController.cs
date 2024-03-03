using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.DTOs.DrinksDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard.PlanControllers;

public partial class PlanController
{
    [HttpPost("drinks")]
    public async Task<ActionResult> AddDrink(DrinkIdsDto ids, PlanTypeEnum planType)
    {
        if (planType == PlanTypeEnum.CustomMealPlan) return Ok(new ApiResponse(404, "Plan type not found"));

        var (ok, message) = await _check(ids.DrinkIds, true);
        if (!ok)
        {
            return Ok(new ApiResponse(404, message));
        }

        await _addDrinks(ids.DrinkIds, planType);
        if (_unitOfWork.HasChanges())
        {
            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiResponse(200));
            return Ok(new ApiResponse(400, "Failed to add drink"));
        }

        return Ok(new ApiResponse(400, "already exist"));
    }

    [HttpGet("drinks")]
    public async Task<ActionResult<List<DrinkDto>>> GetDrinks(PlanTypeEnum planType)
    {
        return Ok(new ApiOkResponse<List<DrinkDto>>(await _getDrinks(planType)));
    }

    [HttpDelete("drinks/{id:int}")]
    public async Task<ActionResult> DeleteDrink(int id,PlanTypeEnum planType)
    {
        if (planType == PlanTypeEnum.CustomMealPlan)
        {
            return Ok(new ApiResponse(400, "Can't delete from custom plan"));
        }
        
        var drink = await _unitOfWork.Repository<AdminSelectedDrink>().GetByIdAsync(id);

        if (drink == null)
            return Ok(new ApiResponse(404, "Drink not found"));

        _unitOfWork.Repository<AdminSelectedDrink>().Delete(drink);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, "Failed to delete drink"));
    }
}