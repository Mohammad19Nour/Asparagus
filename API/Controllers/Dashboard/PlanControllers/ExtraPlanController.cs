using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.DTOs.AdditionDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard.PlanControllers;

public partial class PlanController
{
    [HttpPost("extras")]
    public async Task<ActionResult> AddExtra(ExtraIdsDto ids, PlanTypeEnum planTypeEnum)
    {
        if (planTypeEnum == PlanTypeEnum.CustomMealPlan) return Ok(new ApiResponse(404, "Plan type not found"));

        var (ok, message) = await _check(ids.ExtraIds, false);
        if (!ok)
        {
            return Ok(new ApiResponse(404, message));
        }

        await _addExtraOption(ids.ExtraIds, planTypeEnum);
        if (_unitOfWork.HasChanges())
        {
            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiResponse(200));
            return Ok(new ApiResponse(400, "Failed to add extras"));
        }

        return Ok(new ApiResponse(400, "already exist"));
    }

    [HttpGet("extras")]
    public async Task<ActionResult<List<ExtraOptionDto>>> GetExtras(PlanTypeEnum planTypeEnum)
    {
        return Ok(new ApiOkResponse<List<ExtraOptionDto>>(await _getExtraOptions(planTypeEnum, null)));
    }

    [HttpGet("extras/{typeId:int}")]
    public async Task<ActionResult<List<ExtraOptionDto>>> GetSpecific(PlanTypeEnum planTypeEnum, int typeId)
    {
        var optionType = ExtraOptionType.Nuts;
        if (typeId == 1) optionType = ExtraOptionType.Salad;
        return Ok(new ApiOkResponse<List<ExtraOptionDto>>(await _getExtraOptions(planTypeEnum, optionType)));
    }


    [HttpDelete("extras/{id:int}")]
    public async Task<ActionResult> DeleteExtra(int id)
    {
        var extra = await _unitOfWork.Repository<AdminSelectedExtraOption>().GetByIdAsync(id);

        if (extra == null)
            return Ok(new ApiResponse(404, "Extra not found"));

        _unitOfWork.Repository<AdminSelectedExtraOption>().Delete(extra);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, "Failed to delete extra"));
    }
}