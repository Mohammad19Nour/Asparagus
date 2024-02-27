using AsparagusN.Data.Entities.Identity;
using AsparagusN.DTOs;
using AsparagusN.DTOs.AccountDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AsparagusN.Services;
using AutoMapper;
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
        return Ok(new ApiOkResponse<AdminAccountDto>(adminDto));
    }
}