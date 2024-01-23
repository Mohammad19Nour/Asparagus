﻿using AsparagusN.DTOs;
using AsparagusN.Entities.Identity;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers;

public class AccountController : BaseApiController
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public AccountController(ITokenService tokenService, UserManager<AppUser> userManager,IMapper mapper,
        SignInManager<AppUser> signInManager, IUnitOfWork unitOfWork, IEmailService emailService)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _mapper = mapper;
        _signInManager = signInManager;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDto loginDto)
    {
        loginDto.Email = loginDto.Email.ToLower();

        var user = await _userManager.Users.Include(x => x.HomeAddress)
            .Include(x => x.WorkAddress)
            .FirstOrDefaultAsync(x => x.Email == loginDto.Email);

        if (user == null)
            return Ok(new ApiResponse(400, "Invalid Email", "البريد الإلكتروني خاطئ"));
        
        var res = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!res.Succeeded) return Ok(new ApiResponse(400, "Invalid password", "كلمة السر خاطئة"));
        var userDto = _mapper.Map<AppUser, UserDto>(user);
        userDto.Token = _tokenService.CreateToken(user);
        return Ok(new ApiOkResponse<UserDto>(userDto));
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        try
        {
            registerDto.Email = registerDto.Email.ToLower();

            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                    return Ok(new ApiResponse(400, "This email is already used", "هذا الحساب مستخدم من قبل"));
                var response = await GenerateTokenAndSendEmailForUser(user);

                if (!response)
                    return Ok(new ApiResponse(400, "Failed to send email.", "فشل ارسال الايميل"));

                return Ok(new ApiResponse(200, "You have already registered with this Email," +
                                               "The confirmation link will be resent to your email," +
                                               " please check your email and confirm your account.",
                    "انت مسجل مسبقا بهذا الحساب, سيتم إعادة إرسال رابط التأكيد إليك... الرجاء التأكد من صندوق الوارد لديك من اجل تاكيد حسابك"));
            }

            if (registerDto.Password != registerDto.ConfirmedPassword)
                return Ok(new ApiResponse(400, "passwords isn't identical"));

            //    if (!SomeUsefulFunction.IsValidEmail(registerDto.Email))
            //      return Ok(new ApiResponse(400,"Wrong email","بريد إالكتروني خاطئ"));

            user = new AppUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
            };
            var res = await _userManager.CreateAsync(user, registerDto.Password);

            if (res.Succeeded == false) return Ok(res.Errors);

            var respons = await GenerateTokenAndSendEmailForUser(user);

            if (!respons)
                return BadRequest(new ApiResponse(400, "Failed to send email.",
                    "حدثت مشكلة اثناء إرسال رسالة التأكيد... يرجى المحاولة لاحقا"));

            return Ok(new ApiResponse(200, "The confirmation link was send to your email successfully, " +
                                           "please check your email and confirm your account.",
                "تم إرسال رابط التأكيد إلى البريد الإلكتروني الخاص بك, الرجاء التأمد من صندوق الوارد لديك وتأكيد حسابك."));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    [HttpPost("forget-password")]
     public async Task<ActionResult> ForgetPassword(ForgetPasswordDto dto)
    {
        try
        {
            var email = dto.Email?.ToLower();
            if (email is null) return Ok(new ApiResponse(400, "email must be provided"));
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return Ok(new ApiResponse(401, "user was not found"));
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var code = ConfirmEmailService.GenerateCode(user.Id, token);

            var confirmationLink = Url.Action("ResetPassword", "Account",
                new { userId = user.Id, token = token }, Request.Scheme);

            var text = "<html><body> The code to reset your password is : " + code +
                       "</body></html>";
            var res = await _emailService.SendEmailAsync(user.Email, "Reset Password", text);

            if (!res)
                return Ok(new ApiResponse(400, "Failed to send email."));

            return Ok(new ApiResponse(200, "The Code was sent to your email"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost("reset-password")]
    
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<ActionResult> ResetPassword(ResetDto restDto)
    {
        try
        {
            var newPassword = restDto.NewPassword;
            var code = restDto.Code;
            if (newPassword == null || code == null)
                return Ok(new ApiResponse(400, "The password should not be empty"));
            var val = ConfirmEmailService.GetUserIdAndToken(code);

            if (val is null)
                return BadRequest(new ApiResponse(400, "the code is incorrect","الكود خاطئ, يرجى التأكد من الكود والمحاولة لاحقا"));
            
            var userId = val.Value.userId;
            var token = val.Value.token;

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return Ok(new ApiResponse(401, "this user is not registered"));

            var res = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (res.Succeeded == false) return Ok(new ApiResponse(400, "Cannot reset password"));

            ConfirmEmailService.RemoveUserCodes(userId);
            return Ok(new ApiResponse(200, "Password was reset successfully"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<bool> GenerateTokenAndSendEmailForUser(AppUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var confirmationLink = Url.Action("ConfirmEmail", "Account",
            new { userId = user.Id, token }, Request.Scheme);

        var text = "<html><body>To confirm your email please<a href=" + confirmationLink +
                   "> click here</a></body></html>";
        return await _emailService.SendEmailAsync(user.Email, "Confirmation Email", text);
    }

    [HttpGet("confirm-email")]
    [AllowAnonymous]
    public async Task<ActionResult> ConfirmEmail(string? userId, string? token)
    {
        if (userId is null || token is null)
        {
            return BadRequest(new { message = "token or userId missing" });
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return BadRequest(new ApiResponse(400, "invalid token user id"));
        }

        var res = await _userManager.ConfirmEmailAsync(user, token);

        if (!res.Succeeded) return BadRequest(new ApiResponse(400, "confirmation failed"));

        return Ok("Your Email is Confirmed try to login in now");
    }
}