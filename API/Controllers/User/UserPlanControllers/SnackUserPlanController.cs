using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs;
using AsparagusN.DTOs.SnackDtos;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Errors;
using AsparagusN.Specifications;
using AsparagusN.Specifications.AdminPlanSpecifications;
using AsparagusN.Specifications.UserSpecifications;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.User.UserPlanControllers;

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

        var adminSpec = new AdminSelectedSnackSpecification(snackId);
        var adminSnack = await _unitOfWork.Repository<AdminSelectedSnack>().GetEntityWithSpec(adminSpec);

        if (adminSnack == null || adminSnack.PlanTypeEnum != planDay.UserPlan.PlanType)
            return Ok(new ApiResponse(404, "Snack not found"));

        planDay.UserPlan.NumberOfRemainingSnacks -= quantity;

        var snackToAdd = _mapper.Map<UserSelectedSnack>(adminSnack.Snack);
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
            var specAdmin = new AdminSelectedSnackSpecification(snackDto.AdminNewSnackId.Value);
            var adminSnack = await _unitOfWork.Repository<AdminSelectedSnack>()
                .GetEntityWithSpec(specAdmin);

            if (adminSnack == null || adminSnack.PlanTypeEnum != planDay.UserPlan.PlanType)
                return Ok(new ApiResponse(404, "Snack not found"));


            _mapper.Map(adminSnack.Snack, oldSnack);
        }

        oldSnack.Id = dbId;
        oldSnack.Quantity = snackDto.Quantity;

        _unitOfWork.Repository<UserPlanDay>().Update(planDay);
        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<UserSnackDto>(_mapper.Map<UserSnackDto>(oldSnack)));

        return Ok(new ApiResponse(400, "Failed to add snack"));
    }
}