using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Specifications.UserSpecifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.User.UserPlanControllers;

[Authorize(Roles = nameof(Roles.User))]
public partial class UserPlanController
{
    [HttpGet("extras/{dayId:int}")]
    public async Task<ActionResult<List<UserSelectedExtraOptionDto>>> GetExtras(int dayId)
    {
        var user = await _getUser();

        if (user == null)
            return Ok(new ApiResponse(401));

        var spec = new UserSelectedPlanDayWithExtrasOnlySpecification(user.Id, dayId: dayId);
        var planDay = (await _unitOfWork.Repository<UserPlanDay>().ListWithSpecAsync(spec)).ToList();

        return Ok(new ApiOkResponse<List<UserSelectedExtraOptionDto>>(
            _mapper.Map<List<UserSelectedExtraOptionDto>>(planDay)));
    }
}