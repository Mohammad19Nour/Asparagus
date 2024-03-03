using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.DTOs.ExtraOptionDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard.PlanControllers;

public partial class PlanController
{
    [HttpPost("extras")]
    public async Task<ActionResult> AddExtra(ExtraIdsDto ids, PlanTypeEnum planType)
    {
        if (planType == PlanTypeEnum.CustomMealPlan) return Ok(new ApiResponse(404, "Plan type not found"));

        var (ok, message) = await _check(ids.ExtraIds, false);
        if (!ok)
        {
            return Ok(new ApiResponse(404, message));
        }

        await _addExtraOption(ids.ExtraIds, planType);
        if (_unitOfWork.HasChanges())
        {
            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiResponse(200));
            return Ok(new ApiResponse(400, "Failed to add extras"));
        }

        return Ok(new ApiResponse(400, "already exist"));
    }

    [HttpGet("extras")]
    public async Task<ActionResult<List<ExtraOptionDto>>> GetExtras(PlanTypeEnum planType)
    {
        return Ok(new ApiOkResponse<List<ExtraOptionDto>>(await _getExtraOptions(planType, null)));
    }

    [HttpGet("extras/{typeId:int}")]
    public async Task<ActionResult<List<ExtraOptionDto>>> GetSpecific(PlanTypeEnum planType, int typeId)
    {
        var optionType = ExtraOptionType.Nuts;
        if (typeId == 1) optionType = ExtraOptionType.Salad;
        return Ok(new ApiOkResponse<List<ExtraOptionDto>>(await _getExtraOptions(planType, optionType)));
    }


    [HttpDelete("extras/{id:int}")]
    public async Task<ActionResult> DeleteExtra(int id,[FromQuery]PlanTypeEnum planType)
    {
        if (planType == PlanTypeEnum.CustomMealPlan)
        {
            return Ok(new ApiResponse(400, "Can't delete from custom plan"));
        }
        var extra = await _unitOfWork.Repository<AdminSelectedExtraOption>().GetByIdAsync(id);

        if (extra == null)
            return Ok(new ApiResponse(404, "Extra not found"));

        _unitOfWork.Repository<AdminSelectedExtraOption>().Delete(extra);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, "Failed to delete extra"));
    }
}