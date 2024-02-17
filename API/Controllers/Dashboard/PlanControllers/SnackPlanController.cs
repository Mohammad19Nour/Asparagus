using AsparagusN.Data.Entities.Meal;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.DTOs;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Specifications;
using AsparagusN.Specifications.AdminPlanSpecifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers.Dashboard.PlanControllers;

public partial class PlanController
{
    [HttpPost("snacks")]
    public async Task<ActionResult> AddSnack(SnackIdsDto ids, PlanTypeEnum planTypeEnum)
    {
        if (planTypeEnum == PlanTypeEnum.CustomMealPlan)
            return Ok(new ApiResponse(400, "Can't add to this plan type"));

        var adminSpec = new AdminSelectedSnacksSpecification(planTypeEnum);
        var selectedSnackIds =
            (await _unitOfWork.Repository<AdminSelectedSnack>().ListWithSpecAsync(adminSpec)).Select(x => x.SnackId);
        ids.SnackIds = ids.SnackIds.Where(x => !selectedSnackIds.Contains(x)).ToList();

        var spec = new SnackMealsSpecification();
        var snacks = await _unitOfWork.Repository<Meal>().ListWithSpecAsync(spec);
        var snackIds = snacks.Select(x => x.Id).ToList();
        var notExistSnackIds = ids.SnackIds.Where(x => !snackIds.Contains(x)).ToList();

        if (notExistSnackIds.Count > 0)
            return Ok(new ApiResponse(404, $"snacks with ids: {notExistSnackIds.ToList()} not exist"));

        if (ids.SnackIds.Count == 0) return Ok(new ApiResponse(400, "0 snacks added. maybe it already exist"));

        var res = new List<AdminSelectedSnack>();
        foreach (var toAdd in ids.SnackIds)
        {
            var snk = snacks.First(x => x.Id == toAdd);
            var a = new AdminSelectedSnack { SnackId = toAdd, PlanTypeEnum = planTypeEnum, Snack = snk };
            _unitOfWork.Repository<AdminSelectedSnack>().Add(a);

            res.Add(a);
        }

        await _unitOfWork.SaveChanges();
        var result = await _unitOfWork.Repository<AdminSelectedSnack>().ListWithSpecAsync(adminSpec);

        result = result.Where(x => ids.SnackIds.Contains(x.SnackId)).ToList();
        return Ok(new ApiOkResponse<List<SnackDto>>(_mapper.Map<List<SnackDto>>(result)));
    }

    [HttpGet("snacks")]
    public async Task<ActionResult<List<SnackDto>>> GetSnacks(PlanTypeEnum planTypeEnum)
    {
        var snacks = (await _unitOfWork.Repository<AdminSelectedSnack>().GetQueryable()
            .Where(x => x.PlanTypeEnum == planTypeEnum).Include(x => x.Snack)
            .ToListAsync());

        return Ok(new ApiOkResponse<List<SnackDto>>(_mapper.Map<List<SnackDto>>(snacks)));
    }

    [HttpDelete("snacks/{id:int}")]
    public async Task<ActionResult> DeleteSnack(int id)
    {
        var snack = await _unitOfWork.Repository<AdminSelectedSnack>().GetByIdAsync(id);
        if (snack == null) return Ok(new ApiResponse(404, "Snack not found"));

        _unitOfWork.Repository<AdminSelectedSnack>().Delete(snack);

        if (await _unitOfWork.SaveChanges()) return Ok(new ApiResponse(200, "deleted"));

        return Ok(new ApiResponse(400, "Failed to delete snack"));
    }
}