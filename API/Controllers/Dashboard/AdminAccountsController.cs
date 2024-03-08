using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.DTOs;
using AsparagusN.DTOs.AccountDtos;
using AsparagusN.DTOs.CashierDtos;
using AsparagusN.DTOs.DriverDtos;
using AsparagusN.DTOs.UserDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AsparagusN.Services;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace AsparagusN.Controllers.Dashboard;

public class AdminAccountsController : BaseApiController
{
    private readonly IMediaService _mediaService;
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public AdminAccountsController(IMediaService mediaService, ITokenService tokenService,
        UserManager<AppUser> userManager,
        IMapper mapper,
        SignInManager<AppUser> signInManager, IUnitOfWork unitOfWork, IEmailService emailService)
    {
        _mediaService = mediaService;
        _tokenService = tokenService;
        _userManager = userManager;
        _mapper = mapper;
        _signInManager = signInManager;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AdminAccountDto>> Login(LoginDto loginDto)
    {
        loginDto.Email = loginDto.Email.ToLower();

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);

        if (user == null)
            return Ok(new ApiResponse(400, messageEN: "Invalid Email", messageAR: "البريد الإلكتروني خاطئ"));

        var isUserRole = await _userManager.IsInRoleAsync(user, Roles.User.GetDisplayName().ToLower());
        if (isUserRole) return Ok(new ApiResponse(403, "Can't access to this resource"));

        var res = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!res.Succeeded)
            return Ok(new ApiResponse(400, messageEN: "Invalid password", messageAR: "كلمة السر خاطئة"));

        var adminDto = _mapper.Map<AppUser, AdminAccountDto>(user);
        adminDto.Token = _tokenService.CreateToken(user);
        var roles = await _userManager.GetRolesAsync(user);
        adminDto.Role = roles.First() ?? "No role";
        if (adminDto.Role.ToLower() == Roles.Driver.GetDisplayName().ToLower())
        {
            var driverSpec = new DriverSpecification(adminDto.Email.ToLower());
            var driver = await _unitOfWork.Repository<Driver>().GetEntityWithSpec(driverSpec);
            adminDto.Name = driver.Name;
            adminDto.PictureUrl = driver.PictureUrl;
            adminDto.PhoneNumber = driver.PhoneNumber;
            if (!driver.IsActive) return Ok(new ApiResponse(400, "driver not active"));
        }
        else if (adminDto.Role.ToLower() == Roles.Cashier.GetDisplayName().ToLower())
        {
            var cashierSpec = new CashierWithBranchSpecification(adminDto.Email.ToLower());
            var cashier = await _unitOfWork.Repository<Cashier>().GetEntityWithSpec(cashierSpec);
            adminDto.Name = cashier.Name;
            adminDto.PictureUrl = cashier.PictureUrl;
            adminDto.PhoneNumber = cashier.PhoneNumber;
        }
        else if (adminDto.Role.ToLower() == Roles.Employee.GetDisplayName().ToLower())
        {
            var employee = await _unitOfWork.Repository<Employee>().GetQueryable()
                .Where(c => c.Email.ToLower() == adminDto.Email.ToLower()).FirstAsync();

            adminDto.Name = employee.FullName;
            adminDto.PictureUrl = "No Photo";
            adminDto.PhoneNumber = employee.PhoneNumber;
        }
        else if (adminDto.Role.ToLower() == Roles.Admin.GetDisplayName().ToLower())
        {
            adminDto.Name = user.FullName;
            adminDto.PictureUrl = "No Photo";
            adminDto.PhoneNumber = user.PhoneNumber;
        }
        else
        {
            Console.WriteLine(roles[0]);
            return Ok(new ApiResponse(404, "Not found"));
        }

        return Ok(new ApiOkResponse<AdminAccountDto>(adminDto));
    }

    [Authorize]
    [HttpGet("info")]
    public async Task<ActionResult> GetInfo()
    {
        var email = User.GetEmail();
        var user = await _unitOfWork.Repository<AppUser>().GetQueryable().Where(u => u.Email.ToLower() == email)
            .FirstAsync();
        var roles = (await _userManager.GetRolesAsync(user)).Select(c => c.ToLower()).ToList();


        if (roles.Contains(Roles.Cashier.GetDisplayName().ToLower()))
        {
            var spec = new CashierWithBranchSpecification(email.ToLower());
            var cashier = await _unitOfWork.Repository<Cashier>().GetEntityWithSpec(spec);

            if (cashier == null) return Ok(new ApiResponse(404, "Cashier not found"));
            return Ok(new ApiOkResponse<CashierDto>(_mapper.Map<CashierDto>(cashier)));
        }

        if (roles.Contains(Roles.Driver.GetDisplayName().ToLower()))
        {
            var spec = new DriverSpecification(email.ToLower());
            var driver = await _unitOfWork.Repository<Driver>().GetEntityWithSpec(spec);

            if (driver == null) return Ok(new ApiResponse(404, "driver not found"));

            return Ok(new ApiOkResponse<DriverDto>(_mapper.Map<DriverDto>(driver)));
        }

        return Ok(new ApiOkResponse<UserInfoDto>(_mapper.Map<UserInfoDto>(user)));
    }
}