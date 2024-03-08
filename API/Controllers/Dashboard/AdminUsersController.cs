using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers.Dashboard;

public class AdminUsersController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AdminUsersController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [Authorize(Roles = nameof(DashboardRoles.Users) + "," + nameof(Roles.Admin))]
    [HttpGet]
    public async Task<ActionResult<List<AdminUserDto>>> GetUsers()
    {
        var spec = new CustomersSpecification();
        var users = (await _unitOfWork.Repository<AppUser>().ListWithSpecAsync(spec)).ToList();

        var mappedUsers = _mapper.Map<List<AdminUserDto>>(users);

        var plans = await _unitOfWork.Repository<UserPlan>().ListAllAsync();
        var planDays = await _unitOfWork.Repository<UserPlanDay>().GetQueryable()
            .Include(x => x.SelectedMeals)
            .Where(c => c.Day.Date >= DateTime.Now)
            .ToListAsync();

        plans = plans.Where(c => c.EndDate() >= DateTime.Today).ToList();

        foreach (var user in users)
        {
            var userPlans = plans.Where(c => c.AppUserId == user.Id).ToList();

            var numOfMeals = 0;
            var numOdSnacks = 0;

            foreach (var userPlan in userPlans)
            {
                var days = planDays.Where(c => c.UserPlanId == userPlan.Id).ToList();
                numOfMeals += days.Sum(v => userPlan.NumberOfMealPerDay - v.SelectedMeals.Count);
                numOdSnacks += userPlan.NumberOfRemainingSnacks;
            }

            var mappedUser = mappedUsers.First(c => c.Email.ToLower() == user.Email.ToLower());
            mappedUser.ReminingMeals = numOfMeals;
            mappedUser.ReminingSnack = numOdSnacks;
        }

        return Ok(new ApiOkResponse<List<AdminUserDto>>(mappedUsers));
    }
}