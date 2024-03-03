using AsparagusN.Data.Entities.Meal;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.SnackDtos;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Specifications.AdminPlanSpecifications;
using AsparagusN.Specifications.UserSpecifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.User.UserPlanControllers;
[Authorize]
public partial class UserPlanController
{
    [HttpGet("snacks/{dayId:int}")]
    public async Task<ActionResult<List<UserSnackDto>>> GetSnacks(int dayId)
    {
        var user = await _getUser();

        if (user == null)
        {
            return Ok(new ApiResponse(404, "User not found"));
        }

        var spec = new UserPlanDayWithSnacksOnlySpecification(user.Id, dayId);
        var day = await _unitOfWork.Repository<UserPlanDay>().GetEntityWithSpec(spec);

        if (day == null) return Ok(new ApiResponse(404, "day not found"));

        return Ok(new ApiOkResponse<List<UserSnackDto>>(_mapper.Map<List<UserSnackDto>>(day.SelectedSnacks)));
    }

    [HttpPost("snacks/{dayId:int}")]
    public async Task<ActionResult<UserSnackDto>> AddSnack(int dayId, int snackId, int quantity)
    {
        var user = await _getUser();

        if (user == null) return Ok(new ApiResponse(401));

        var spec = new UserPlanDayWithSnacksOnlySpecification(user.Id, dayId);
        var planDay = await _unitOfWork.Repository<UserPlanDay>().GetEntityWithSpec(spec);

        if (planDay == null) return Ok(new ApiResponse(404, "day not found"));

        if (planDay.UserPlan.NumberOfRemainingSnacks == 0)
            return Ok(new ApiResponse(400, "You have used up all your snacks"));

        if (planDay.UserPlan.NumberOfRemainingSnacks < quantity)
            return Ok(new ApiResponse(400, "You have no enough snacks"));

        Meal? skToAdd;
        if (planDay.UserPlan.PlanType == PlanTypeEnum.CustomMealPlan)
        {
            var deletedSnacks = await _unitOfWork.Repository<AdminSelectedSnack>().ListAllAsync();
            deletedSnacks = deletedSnacks.Where(c => c.PlanTypeEnum == PlanTypeEnum.CustomMealPlan).ToList();

            if (deletedSnacks.FirstOrDefault(c => c.SnackId == snackId) != null)
                return Ok(new ApiResponse(404, "Snack not found"));

            var mealSnack = await _unitOfWork.Repository<Meal>().GetByIdAsync(snackId);
            if (mealSnack == null)
                return Ok(new ApiResponse(404, "Snack not found"));
            skToAdd = mealSnack;
        }
        else
        {
            var adminSpec = new AdminSelectedSnackSpecification(snackId, planDay.UserPlan.PlanType);
            var adminSnack = await _unitOfWork.Repository<AdminSelectedSnack>().GetEntityWithSpec(adminSpec);

            if (adminSnack == null || adminSnack.PlanTypeEnum != planDay.UserPlan.PlanType)
                return Ok(new ApiResponse(404, "Snack not found"));
            skToAdd = adminSnack.Snack;
        }

        planDay.UserPlan.NumberOfRemainingSnacks -= quantity;

        var snackToAdd = _mapper.Map<UserSelectedSnack>(skToAdd);
        snackToAdd.Quantity = quantity;
        snackToAdd.Id = 0;
        planDay.SelectedSnacks.Add(snackToAdd);

        _unitOfWork.Repository<UserPlanDay>().Update(planDay);
        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<UserSnackDto>(_mapper.Map<UserSnackDto>(snackToAdd)));

        return Ok(new ApiResponse(400, "Failed to add snack"));
    }

    [HttpPut("snacks/{dayId:int}")]
    public async Task<ActionResult<UserSnackDto>> UpdateSnack(int dayId, UserUpdateSnackDto snackDto)
    {
        var user = await _getUser();

        if (user == null) return Ok(new ApiResponse(401));

        var spec = new UserPlanDayWithSnacksOnlySpecification(user.Id, dayId);
        var planDay = await _unitOfWork.Repository<UserPlanDay>().GetEntityWithSpec(spec);

        if (planDay == null) return Ok(new ApiResponse(404, "day not found"));

        var oldSnack = planDay.SelectedSnacks.FirstOrDefault(x => x.Id == snackDto.UserOldSnackId);

        if (oldSnack == null) return Ok(new ApiResponse(404, "Your snack not found"));

        if (planDay.UserPlan.NumberOfRemainingSnacks + oldSnack.Quantity - snackDto.Quantity < 0)
            return Ok(new ApiResponse(400, "You have no enough snacks"));

        planDay.UserPlan.NumberOfRemainingSnacks += oldSnack.Quantity;
        planDay.UserPlan.NumberOfRemainingSnacks -= snackDto.Quantity;
        var dbId = oldSnack.Id;


        if (snackDto.AdminNewSnackId != null)
        {
            var specAdmin =
                new AdminSelectedSnackSpecification(snackDto.AdminNewSnackId.Value, planDay.UserPlan.PlanType);

            var adminSnack = await _unitOfWork.Repository<AdminSelectedSnack>()
                .GetEntityWithSpec(specAdmin);

            Meal? snkToAdd;

            if (planDay.UserPlan.PlanType == PlanTypeEnum.CustomMealPlan)
            {
                var snk = await _unitOfWork.Repository<Meal>().GetByIdAsync(snackDto.AdminNewSnackId.Value);

                if (snk == null || adminSnack != null)
                    return Ok(new ApiResponse(404, "Snack not found"));
                snkToAdd = snk;
            }
            else
            {
                if (adminSnack == null || adminSnack.PlanTypeEnum != planDay.UserPlan.PlanType)
                    return Ok(new ApiResponse(404, "Snack not found"));

                snkToAdd = adminSnack.Snack;
            }

            _mapper.Map(snkToAdd, oldSnack);
        }


        oldSnack.Id = dbId;
        oldSnack.Quantity = snackDto.Quantity;

        _unitOfWork.Repository<UserPlanDay>().Update(planDay);
        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<UserSnackDto>(_mapper.Map<UserSnackDto>(oldSnack)));

        return Ok(new ApiResponse(400, "Failed to add snack"));
    }

    [HttpDelete("snacks/{dayId:int}")]
    public async Task<ActionResult> DeleteSnack(int dayId, int snackId)
    {
        var user = await _getUser();

        if (user == null) return Ok(new ApiResponse(401));

        var spec = new UserPlanDayWithSnacksOnlySpecification(user.Id, dayId);
        var planDay = await _unitOfWork.Repository<UserPlanDay>().GetEntityWithSpec(spec);

        if (planDay == null) return Ok(new ApiResponse(404, "day not found"));

        var snack = planDay.SelectedSnacks.FirstOrDefault(x => x.Id == snackId);

        if (snack == null) return Ok(new ApiResponse(404, "Snack not found"));

        planDay.UserPlan.NumberOfRemainingSnacks += snack.Quantity;
        planDay.SelectedSnacks.Remove(snack);
        _unitOfWork.Repository<UserPlanDay>().Update(planDay);
        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, "Failed to delete snack"));
    }
}