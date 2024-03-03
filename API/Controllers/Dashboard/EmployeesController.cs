using System.ComponentModel.DataAnnotations;
using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.DTOs.EmployeeDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace AsparagusN.Controllers.Dashboard;

public class EmployeesController : BaseApiController
{
    private readonly IMediaService _mediaService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public EmployeesController(IMediaService mediaService, IUnitOfWork unitOfWork, IMapper mapper,
        UserManager<AppUser> userManager)
    {
        _mediaService = mediaService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpPost("add")]
    public async Task<ActionResult<EmployeeDto>> AddEmployee([FromForm] NewEmployeeDto newEmployeeDto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                newEmployeeDto.Email = newEmployeeDto.Email.ToLower();

                var employee = _mapper.Map<Employee>(newEmployeeDto);

                var employeeUser = new AppUser
                {
                    UserName = newEmployeeDto.Email,
                    Email = newEmployeeDto.Email,
                    FullName = newEmployeeDto.FullName,
                };

                var result = await _userManager.CreateAsync(employeeUser, newEmployeeDto.Password);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return Ok(new ApiResponse(400,
                        result.Errors.Aggregate("", (error, identityError) => error + identityError.Description)));
                }

                IdentityResult roleResult =
                    await _userManager.AddToRoleAsync(employeeUser, Roles.Employee.GetDisplayName().ToLower());

                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new ApiResponse(400, "Failed to add roles"));
                }

                _unitOfWork.Repository<Employee>().Add(employee);

                if (await _unitOfWork.SaveChanges())
                {
                    await transaction.CommitAsync();
                    return Ok(new ApiOkResponse<EmployeeDto>(_mapper.Map<EmployeeDto>(employee)));
                }

                await transaction.RollbackAsync();
                return Ok(new ApiResponse(400, "Failed to add employee"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                await transaction.RollbackAsync();
                return Ok(new ApiResponse(400, "Exception happened.. failed to add employee"));
                throw;
            }
        }
    }
    [HttpGet]
    public async Task<ActionResult> GetEmployees()
    {
        var employeeSpec = new EmployeesSpecification();
        var employees = await _unitOfWork.Repository<AppUser>().ListWithSpecAsync(employeeSpec);

        var resultList = new List<EmployeeDto>();
        var cantEditRoles = new List<string>();
        foreach (DashboardRoles role in Enum.GetValues(typeof(DashboardRoles)))
        {
            var roleName = role.GetDisplayName().ToLower();
            cantEditRoles.Add(roleName);
        }

        var emTable = await _unitOfWork.Repository<Employee>().ListAllAsync();

        foreach (var employee in employees)
        {
            var map = new Dictionary<string, bool>();

            var userRole = employee.UserRoles.Select(c => c.Role.Name.ToLower());
            userRole = userRole.Except(cantEditRoles);
            foreach (DashboardRoles role in Enum.GetValues(typeof(DashboardRoles)))
            {
                var roleName = role.GetDisplayName().ToLower();
                if (userRole.Contains(roleName)) map[roleName] = true;
                else map[roleName] = false;
            }

            var em = emTable.First(c => c.Email.ToLower() == employee.Email.ToLower());
            var dto = _mapper.Map<EmployeeDto>(em);
            dto.Roles = map;
            resultList.Add(dto);
        }

        return Ok(new ApiOkResponse<List<EmployeeDto>>(resultList));
    }


     [HttpPut("roles")]
    public async Task<ActionResult> UpdateEmployeeRoles(Dictionary<string, bool> rolesDto,
        [EmailAddress] string employeeEmail)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var employeeSpec = new EmployeesSpecification(employeeEmail.ToLower());
                var employee = await _unitOfWork.Repository<AppUser>().GetEntityWithSpec(employeeSpec);

                if (employee == null)
                    return Ok(new ApiResponse(404, "Employee not found"));

                var employeeRoles = (await _userManager.GetRolesAsync(employee)).Select(r => r.ToLower());

                var resultList = new List<EmployeeDto>();

                var cantEditRoles = new List<string>();

                foreach (DashboardRoles role in Enum.GetValues(typeof(DashboardRoles)))
                {
                    var roleName = role.GetDisplayName().ToLower();
                    cantEditRoles.Add(roleName);
                }

                var userCantEditRoles = employeeRoles.Where(r => cantEditRoles.Contains(r)).ToList();

                foreach (var roleDto in rolesDto)
                {
                    if (roleDto.Value)
                    {
                        if (Enum.TryParse<DashboardRoles>(roleDto.Key, true, out DashboardRoles result))
                        {
                            userCantEditRoles.Add(result.GetDisplayName().ToLower());
                        }
                        else return Ok(new ApiResponse(404, $"Role {roleDto.Key} not found"));
                    }
                }

                var ok = await _userManager.RemoveFromRolesAsync(employee, employeeRoles);
                var ok1 = await _userManager.AddToRolesAsync(employee, userCantEditRoles);

                if (ok.Succeeded && ok1.Succeeded)
                {
                    await transaction.CommitAsync();
                    return Ok(new ApiResponse(200));
                }

                return Ok(new ApiResponse(400));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult<EmployeeDto>> UpdateEmployee(int id, [FromForm] UpdateEmployeeDto updateEmployeeDto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(id);

                if (employee == null)
                    return Ok(new ApiResponse(404, "Employee not found"));

                var employeeUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == employee.Email);
                if (employeeUser == null) return Ok(new ApiResponse(404, "Employee not found"));

                _mapper.Map(updateEmployeeDto, employee);

                if (updateEmployeeDto.Password != null && !string.IsNullOrEmpty(updateEmployeeDto.Password.Trim()) &&
                    employee.Password != updateEmployeeDto.Password)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(employeeUser);
                    var result = await _userManager.ResetPasswordAsync(employeeUser, token, updateEmployeeDto.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var r in result.Errors)
                        {
                            Console.WriteLine(r.Description);
                        }

                        await transaction.RollbackAsync();
                        return Ok(new ApiResponse(404, "Failed to update password"));
                    }

                    employee.Password = updateEmployeeDto.Password;
                }


                if (updateEmployeeDto.Email != null && updateEmployeeDto.Email.ToLower() != employee.Email.ToLower())
                {
                    var exist = await _userManager.Users.FirstOrDefaultAsync(x =>
                        x.Email.ToLower() == updateEmployeeDto.Email.ToLower());

                    if (exist != null)
                        return Ok(new ApiResponse(400, "Email already taken"));

                    employeeUser.UserName = updateEmployeeDto.Email.ToLower();
                    employeeUser.Email = updateEmployeeDto.Email.ToLower();
                    employeeUser.NormalizedUserName = _userManager.NormalizeEmail(updateEmployeeDto.Email);
                }

                _unitOfWork.Repository<Employee>().Update(employee);

                if (await _unitOfWork.SaveChanges())
                {
                    await transaction.CommitAsync();
                    return Ok(new ApiOkResponse<EmployeeDto>(_mapper.Map<EmployeeDto>(employee)));
                }

                await transaction.RollbackAsync();
                return Ok(new ApiResponse(400, "Failed to update  employee"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await transaction.RollbackAsync();
                return Ok(new ApiResponse(400, "Exception happened.. failed to update employee"));

                throw;
            }
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteEmployee(int id)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(id);

                if (employee == null)
                    return Ok(new ApiResponse(404, "Employee not found"));

                var userEmployee =
                    await _userManager.Users.FirstOrDefaultAsync(c => c.Email.ToLower() == employee.Email.ToLower());
                if (userEmployee == null)
                    return Ok(new ApiResponse(404, "Employee not found"));

                _unitOfWork.Repository<AppUser>().Delete(userEmployee);
                _unitOfWork.Repository<Employee>().Delete(employee);

                if (await _unitOfWork.SaveChanges())
                {
                    await transaction.CommitAsync();
                    return Ok(new ApiResponse(200));
                }

                await transaction.RollbackAsync();
                return Ok(new ApiResponse(400, "Failed to delete employee"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await transaction.RollbackAsync();
                return Ok(new ApiResponse(400, "Exception happened.. failed to add employee"));

                throw;
            }
        }
    }
}