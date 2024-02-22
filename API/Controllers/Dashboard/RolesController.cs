/*using AsparagusN.Data.Entities.Identity;
using AsparagusN.DTOs;
using AsparagusN.Enums;
using AsparagusN.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace AsparagusN.Controllers.Dashboard;

public class RolesController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public RolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet("cashiers")]
    public async Task<ActionResult> GetCashiersWithRoles()
    {
        var cashiers = await GetUsersWithRole(Roles.Cashier);
        return Ok(new ApiOkResponse<List<UserWithRolesDto>>(cashiers));
    }

    [HttpGet("drivers")]
    public async Task<ActionResult> GetDriversWithRoles()
    {
        var cashiers = await GetUsersWithRole(Roles.Driver);
        return Ok(new ApiOkResponse<List<UserWithRolesDto>>(cashiers));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> EditRoles(int id, [FromQuery] string roleDto)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null) return NotFound("user not found");


        if (Enum.TryParse(roleDto, out Roles enumRole))
        {
            if (enumRole == Roles.User) return Ok(new ApiResponse(400, "Can't add User role"));
        }
        else
        {
            Console.WriteLine(enumRole);
            return Ok(new ApiResponse(400, $"Wrong role: {roleDto}"));
        }

        if (enumRole == Roles.Admin)
            return Ok(new ApiResponse(400, "can't add admin role"));

        var appRoles = await _roleManager.Roles.ToListAsync();

        var notExistRoles = !appRoles.Any(r => r.Name.ToLower() == enumRole.GetDisplayName().ToLower());

        if (notExistRoles)
            return Ok(new ApiResponse(400, "Role " + roleDto+ " not found"));


        var userRoles = await _userManager.GetRolesAsync(user);
        userRoles = userRoles.Select(x => x.ToLower()).ToArray();

        if (userRoles.Any(
                x => string.Equals(x, Roles.Admin.GetDisplayName(), StringComparison.CurrentCultureIgnoreCase)))
            return Ok(new ApiResponse(400, "Can't update the role of admin"));

        var roleName = enumRole.GetDisplayName().ToLower();
        var result = await _userManager.AddToRolesAsync(user, rolesToAdd);

        if (!result.Succeeded)
            return BadRequest("Failed ll");

        result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(rolesToAdd));

        if (!result.Succeeded)
            return BadRequest("Failed");

        var response = await _userManager.GetRolesAsync(user);

        return Ok(new ApiOkResponse<IList<string>>(response));
    }

    private async Task<List<UserWithRolesDto>>
        GetUsersWithRole(Roles role)
    {
        string roleName = role.GetDisplayName().ToLower();

        var users = await _userManager.Users
            .Include(r => r.UserRoles)
            .ThenInclude(r => r.Role)
            .Select(u => new
            {
                u.Id,
                Roles = u.UserRoles
                    .Select(r => r.Role.Name.ToLower()
                    ).ToList(),
                UserEmail = u.Email,
                FullName = u.FullName
            })
            .ToListAsync();

        users = users.Where(x =>
            x.Roles.Any(s => s.ToLower() == roleName)).ToList();

        var res = new List<UserWithRolesDto>();

        res = users.Select(x => new UserWithRolesDto(
            x.Id,
            x.UserEmail,
            x.FullName,
            x.Roles
        )).ToList();
        return res;
    }
}*/