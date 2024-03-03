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
    public async Task<ActionResult> AddSnack(SnackIdsDto ids, PlanTypeEnum planType)
    {
        var adminSpec = new AdminSelectedSnacksSpecification(planType);
        var snks = (await _unitOfWork.Repository<AdminSelectedSnack>().ListWithSpecAsync(adminSpec)).ToList();
        var selectedSnackIds =
            snks.Select(x => x.SnackId);


        if (planType == PlanTypeEnum.CustomMealPlan)
        {
            bool ff = false;
            foreach (var id in ids.SnackIds)
            {
                var tmp = snks.FirstOrDefault(y => y.SnackId == id);
                if (tmp != null)
                {
                    _unitOfWork.Repository<AdminSelectedSnack>().Delete(tmp);
                    snks.Remove(tmp);
                }

                ff = true;
            }

            if (!ff) return Ok(new ApiResponse(200));
            if (await _unitOfWork.SaveChanges()) return Ok(new ApiResponse(200));
            return Ok(new ApiResponse(400, "Failed to add snack"));
        }

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
            var a = new AdminSelectedSnack { SnackId = toAdd, PlanTypeEnum = planType, Snack = snk };
            _unitOfWork.Repository<AdminSelectedSnack>().Add(a);

            res.Add(a);
        }

        await _unitOfWork.SaveChanges();
        var result = await _unitOfWork.Repository<AdminSelectedSnack>().ListWithSpecAsync(adminSpec);

        result = result.Where(x => ids.SnackIds.Contains(x.SnackId)).ToList();
        return Ok(new ApiOkResponse<List<SnackDto>>(_mapper.Map<List<SnackDto>>(result)));
    }

    [HttpGet("snacks")]
    public async Task<ActionResult<List<SnackDto>>> GetSnacks(PlanTypeEnum planType)
    {
        if (planType == PlanTypeEnum.CustomMealPlan)
        {
            var snackSpec = new SnackMealsSpecification();
            var snk = await _unitOfWork.Repository<Meal>().ListWithSpecAsync(snackSpec);
            var deletedSnacks = await _unitOfWork.Repository<AdminSelectedSnack>().ListAllAsync();
            deletedSnacks = deletedSnacks.Where(c => c.PlanTypeEnum == PlanTypeEnum.CustomMealPlan).ToList();
            var deletedSnacksIds = deletedSnacks.Select(c => c.SnackId).ToList();

            snk = snk.Where(s => deletedSnacksIds.All(r => r != s.Id)).ToList();
            var result = _mapper.Map<List<SnackDto>>(snk);
            return Ok(new ApiOkResponse<List<SnackDto>>(result));
        }
        else
        {
            var snacks = (await _unitOfWork.Repository<AdminSelectedSnack>().GetQueryable()
                .Where(x => x.PlanTypeEnum == planType).Include(x => x.Snack)
                .ToListAsync());

            var result = _mapper.Map<List<SnackDto>>(snacks);
            return Ok(new ApiOkResponse<List<SnackDto>>(result));
        }
    }

    [HttpDelete("snacks/{id:int}")]
    public async Task<ActionResult> DeleteSnack(int id, PlanTypeEnum planType)
    {
        if (planType == PlanTypeEnum.CustomMealPlan)
        {
            var snackmeal = await _unitOfWork.Repository<Meal>().GetByIdAsync(id);

            if (snackmeal == null) return Ok(new ApiResponse(404, "snack not found"));

            var tmp = await _unitOfWork.Repository<AdminSelectedSnack>().ListAllAsync();

            if (tmp.FirstOrDefault(c => c.SnackId == id && c.PlanTypeEnum == PlanTypeEnum.CustomMealPlan) != null)
                return Ok(new ApiResponse(200));
            _unitOfWork.Repository<AdminSelectedSnack>().Add(new AdminSelectedSnack
            {
                SnackId = id,
                PlanTypeEnum = planType
            });
            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiResponse(200));
            return Ok(new ApiResponse(400, "Failed to delete snack"));
        }

        var snack = await _unitOfWork.Repository<AdminSelectedSnack>().GetByIdAsync(id);
        if (snack == null) return Ok(new ApiResponse(404, "Snack not found"));

        _unitOfWork.Repository<AdminSelectedSnack>().Delete(snack);

        if (await _unitOfWork.SaveChanges()) return Ok(new ApiResponse(200, "deleted"));

        return Ok(new ApiResponse(400, "Failed to delete snack"));
    }
}