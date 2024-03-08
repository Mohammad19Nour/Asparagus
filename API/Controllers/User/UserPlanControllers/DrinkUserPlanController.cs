using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.DrinksDtos;
using AsparagusN.DTOs.UserDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Specifications;
using AsparagusN.Specifications.UserSpecifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.User.UserPlanControllers;

[Authorize(Roles = nameof(Roles.User))]
public partial class UserPlanController
{
    [HttpGet("drinks/{dayId:int}")]
    public async Task<ActionResult<List<UserSelectedDrinkDto>>> GetDrinks(int dayId)
    {
        var user = await _getUser();

        if (user == null)
            return Ok(new ApiResponse(401));

        var spec = new UserPlanDayWithDrinksOnlySpecification(user.Id, dayId: dayId);
        var planDay = (await _unitOfWork.Repository<UserPlanDay>().ListWithSpecAsync(spec)).ToList();

        return Ok(new ApiOkResponse<List<UserSelectedDrinkDto>>(_mapper.Map<List<UserSelectedDrinkDto>>(planDay)));
    }
}