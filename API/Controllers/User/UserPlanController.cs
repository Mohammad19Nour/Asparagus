using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.DTOs.UserDtos;
using AsparagusN.Entities;
using AsparagusN.Entities.Identity;
using AsparagusN.Entities.MealPlan;
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
    public async Task<ActionResult> Get()
    {
        return Ok();
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
        
        Console.WriteLine(newDrink.Drink.NameEnglish);
        Console.WriteLine("\n\n");
        Console.WriteLine(oldDrink.NameEnglish);
       
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