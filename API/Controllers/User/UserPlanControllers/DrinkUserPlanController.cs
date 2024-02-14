using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.UserDtos;
using AsparagusN.Errors;
using AsparagusN.Specifications;
using AsparagusN.Specifications.UserSpecifications;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.User.UserPlanControllers;

public partial class UserPlanController
{
    [HttpPut("drink")]
    public async Task<ActionResult> UpdateDrink(UserPlanUpdateDrinkDto updateDrinkDto)
    {
        var user = await _getUser();

        if (user == null) return Ok(new ApiResponse(404, "user not found"));

        var spec = new UserPlanDayWithDrinksAndMealsAndExtra(updateDrinkDto.DayId, user.Id);
        var planDay = await _unitOfWork.Repository<UserPlanDay>().GetEntityWithSpec(spec);

        if (planDay == null) return Ok(new ApiResponse(404, "day not found not found"));

        var oldDrink = planDay.SelectedDrinks.FirstOrDefault(x => x.Id == updateDrinkDto.UserOldDrinkId);
        if (oldDrink == null)
            return Ok(new ApiResponse(400, $"You don't have drink with id={updateDrinkDto.UserOldDrinkId}"));

        if (planDay.SelectedDrinks.All(x => x.Id != updateDrinkDto.UserOldDrinkId))
            return Ok(new ApiResponse(400, $"You don't have drink with id={updateDrinkDto.UserOldDrinkId}"));

        var specx = new AdminSelectedDrinksSpecification(updateDrinkDto.AdminNewDrinkId, planDay.UserPlan.PlanType);
        var newDrink = await _unitOfWork.Repository<AdminSelectedDrink>().GetEntityWithSpec(specx);

        if (newDrink == null)
            return Ok(new ApiResponse(400, $"Drink with id = {updateDrinkDto.AdminNewDrinkId} not exist"));

        _mapper.Map(newDrink.Drink, oldDrink);

        _unitOfWork.Repository<UserPlanDay>().Update(planDay);
        await _unitOfWork.SaveChanges();
        return Ok();
    }
}