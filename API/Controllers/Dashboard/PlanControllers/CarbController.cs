using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.DTOs.CarbDtos;
using AsparagusN.Entities;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard.PlanControllers;

public partial class PlanController : BaseApiController
{
    [HttpPost("carb")]
    public async Task<ActionResult<List<CarbDto>>> AddCarb(List<int> ids, PlanTypeEnum planType)
    {
        if (ids.Count == 0)
            return Ok(new ApiResponse(400, "Ids must contains at least one element"));

        var carbs = await _unitOfWork.Repository<Meal>().ListAllAsync();

        foreach (var id in from id in ids let carb = carbs.FirstOrDefault(x => x.Id == id) where carb == null select id)
        {
            return Ok(new ApiResponse(404, $"carb with id: {id} not found"));
        }

        var existingCarb = await _unitOfWork.Repository<AdminSelectedCarb>().ListAllAsync();

        ids = ids.Except(existingCarb.Select(x => x.CarbId).ToList()).ToList();
        var toAdd = carbs.Where(x => ids.Contains(x.Id)).ToList();

        toAdd.ForEach(x => _unitOfWork.Repository<AdminSelectedCarb>().Add(new AdminSelectedCarb
        {
            PlanTypeEnum = planType,
            CarbId = x.CategoryId
        }));

        if (await _unitOfWork.SaveChanges() || toAdd.Count == 0)
            return Ok(new ApiOkResponse<List<CarbDto>>(_mapper.Map<List<CarbDto>>(toAdd)));
        return Ok(new ApiResponse(400, "Failed to add carb"));
    }

    [HttpGet("carb")]
    public async Task<ActionResult<List<CarbDto>>> GetCarb(PlanTypeEnum planType)
    {
        var spec = new AdminSelectedCarbSpecification(planType);
        var carbs = await _unitOfWork.Repository<AdminSelectedCarb>().ListWithSpecAsync(spec);

        var res = carbs.Select(x => _mapper.Map<CarbDto>(x)).ToList();

        return Ok(new ApiOkResponse<List<CarbDto>>(res));
    }

    [HttpDelete("carb/{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var carb = await _unitOfWork.Repository<AdminSelectedCarb>().GetByIdAsync(id);
        if (carb == null) return Ok(new ApiResponse(404, "Carb not found"));

        _unitOfWork.Repository<AdminSelectedCarb>().Delete(carb);

        if (await _unitOfWork.SaveChanges()) return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, "Failed to delete carb"));
    }
}