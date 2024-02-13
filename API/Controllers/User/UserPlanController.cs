using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs;
using AsparagusN.DTOs.UserDtos;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Entities;
using AsparagusN.Entities.Identity;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AsparagusN.Specifications.UserSpecifications;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers.User;

public class UserPlanController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public UserPlanController(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult> Get(PlanTypeEnum planType)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "user not found"));

        var spec = new UserPlanWithMealsDrinksAndExtrasSpecification(user.Id, planType);
        var res = await _unitOfWork.Repository<UserPlan>().GetEntityWithSpec(spec);

        return Ok(new ApiOkResponse<UserPlanDto>(_mapper.Map<UserPlanDto>(res)));
    }

    [HttpPost("meals/{dayId:int}")]
    public async Task<ActionResult> AddMeal(int dayId, AddNewUserMealDto updatedMeal)
    {
        try
        {
            var user = await _getUser();
            if (user == null) return (Ok(new ApiResponse(401)));

            var spec = new UserPlanDayWithMealsOnlySpecification(user.Id, dayId);
            var planDay = await _unitOfWork.Repository<UserPlanDay>().GetEntityWithSpec(spec);

            if (planDay == null) return Ok(new ApiResponse(404, "Plan day not found"));

            var allowedMeals = planDay.UserPlan.NumberOfMealPerDay;
            var usedMeal = planDay.SelectedMeals.Count;

            if (allowedMeals - usedMeal <= 0)
                return Ok(new ApiResponse(400, "You have already reached your day limit "));

            var adminMealSpec =
                new AdminPlanDaysWithMealsSpecification(dayId: updatedMeal.AdminDayId);

            var adminDay = await _unitOfWork.Repository<AdminPlanDay>().GetEntityWithSpec(adminMealSpec);

            if (adminDay == null) return Ok(new ApiResponse(400, "Admin day not found"));

            if (planDay.UserPlan.PlanType != adminDay.PlanType)
                return Ok(new ApiResponse(400, "user plan type not as same as admin plan type"));

            var meal = adminDay.Meals.FirstOrDefault(x => x.MealId == updatedMeal.AdminMealId);
            if (meal == null) return Ok(new ApiResponse(404, "Meal not found"));

            var mealToAdd = new UserSelectedMeal
            {
                UserPlanDay = planDay,
                UserPlanDayId = planDay.Id
            };
            _mapper.Map(meal.Meal, mealToAdd);

            mealToAdd.Id = 0;
            planDay.SelectedMeals.Add(mealToAdd);

            _unitOfWork.Repository<UserPlanDay>().Update(planDay);
            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiOkResponse<UserSelectedMealDto>(_mapper.Map<UserSelectedMealDto>(mealToAdd)));


            return Ok(new ApiResponse(400, "Failed to add meal"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPut("meals/{dayId:int}")]
    public async Task<ActionResult> UpdateMeal(int dayId, UpdateUserMealDto updatedMeal)
    {
        try
        {
            var user = await _getUser();
            if (user == null) return (Ok(new ApiResponse(401)));

            var spec = new UserPlanDayWithMealsOnlySpecification(user.Id, dayId);
            var planDay = await _unitOfWork.Repository<UserPlanDay>().GetEntityWithSpec(spec);

            if (planDay == null) return Ok(new ApiResponse(404, "Plan day not found"));

            var oldMeal = planDay.SelectedMeals.FirstOrDefault(x => x.Id == updatedMeal.OldUserMealId);
            if (oldMeal == null) return Ok(new ApiResponse(404, "Meal not found in user selections"));

            var adminMealSpec =
                new AdminPlanDaysWithMealsSpecification(dayId: updatedMeal.AdminDayId);

            var adminDay = await _unitOfWork.Repository<AdminPlanDay>().GetEntityWithSpec(adminMealSpec);

            if (adminDay == null) return Ok(new ApiResponse(400, "Admin day not found"));

            if (planDay.UserPlan.PlanType != adminDay.PlanType)
                return Ok(new ApiResponse(400, "user plan type not as same as admin plan type"));

            var newMeal = adminDay.Meals.FirstOrDefault(x => x.MealId == updatedMeal.AdminMealId);
            if (newMeal == null) return Ok(new ApiResponse(404, "New meal not found"));

            var mealToAdd = new UserSelectedMeal
            {
                UserPlanDay = planDay,
                UserPlanDayId = planDay.Id
            };

            var dbId = oldMeal.Id;
            _mapper.Map(newMeal.Meal, oldMeal);
            oldMeal.Id = dbId;

            _unitOfWork.Repository<UserPlanDay>().Update(planDay);
            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiOkResponse<UserSelectedMealDto>(_mapper.Map<UserSelectedMealDto>(oldMeal)));

            return Ok(new ApiResponse(400, "Failed to add meal"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpDelete("meals/{dayId:int}")]
    public async Task<ActionResult> DeleteMeal(int dayId, int mealId)
    {
        var user = await _getUser();
        if (user == null) return (Ok(new ApiResponse(401)));

        var spec = new UserPlanDayWithMealsOnlySpecification(user.Id, dayId);
        var planDay = await _unitOfWork.Repository<UserPlanDay>().GetEntityWithSpec(spec);

        if (planDay == null) return Ok(new ApiResponse(404, "Plan day not found"));

        var meal = planDay.SelectedMeals.FirstOrDefault(x => x.Id == mealId);
        if (meal == null) return Ok(new ApiResponse(404, "Meal not found"));

        planDay.SelectedMeals.Remove(meal);

        if (await _unitOfWork.SaveChanges()) return Ok(new ApiResponse(200, "deleted"));
        return Ok(new ApiResponse(400, "Failed to delete meal"));
    }


    [HttpPut("updateDrink")]
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

        Console.WriteLine(newDrink.Drink.NameEnglish);
        Console.WriteLine("\n\n");
        Console.WriteLine(oldDrink.NameEnglish);

        _mapper.Map(newDrink.Drink, oldDrink);

        _unitOfWork.Repository<UserPlanDay>().Update(planDay);
        await _unitOfWork.SaveChanges();
        return Ok();
    }

    private async Task<AppUser?> _getUser()
    {
        var email = HttpContext.User.GetEmail();
        return await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}