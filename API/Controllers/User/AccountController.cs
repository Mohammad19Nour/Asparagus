using AsparagusN.Data.Entities.Identity;
using AsparagusN.DTOs;
using AsparagusN.DTOs.AccountDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AsparagusN.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace AsparagusN.Controllers.User;

public class AccountController : BaseApiController
{
    private readonly IMediaService _mediaService;
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public AccountController(IMediaService mediaService, ITokenService tokenService, UserManager<AppUser> userManager,
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
    public async Task<ActionResult<AccountDto>> Login(LoginDto loginDto)
    {
        loginDto.Email = loginDto.Email.ToLower();

        var user = await _userManager.Users.Include(x => x.HomeAddress)
            .Include(x => x.WorkAddress)
            .FirstOrDefaultAsync(x => x.Email == loginDto.Email);

        if (user == null)
            return Ok(new ApiResponse(400, messageEN: "Invalid Email", messageAR: "البريد الإلكتروني خاطئ"));

        var isUserRole = await _userManager.IsInRoleAsync(user, Roles.User.GetDisplayName().ToLower());
        if (!isUserRole) return Ok(new ApiResponse(403, "Can't access to this resource"));

        var res = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!res.Succeeded)
            return Ok(new ApiResponse(400, messageEN: "Invalid password", messageAR: "كلمة السر خاطئة"));

        /*   if (!user.EmailConfirmed)
        {
            var response = await GenerateTokenAndSendEmailForUser(user);

            if (!response)
                return Ok(new ApiResponse(400, messageEN: "Failed to send email.", messageAR: "فشل ارسال الايميل"));

            return Ok(new ApiResponse(200, messageEN: "You have to confirm your account first," +
                                                      "The confirmation link will be resent to your email," +
                                                      " please check it and confirm your account.",
                messageAR:
                "يجب عليك تاكيد الحساب اولا, سيتم إعادة إرسال رابط التأكيد إليك... الرجاء التأكد من صندوق الوارد لديك من اجل تاكيد حسابك"));
        }*/

        var userDto = _mapper.Map<AppUser, AccountDto>(user);
        userDto.Token = _tokenService.CreateToken(user);
        return Ok(new ApiOkResponse<AccountDto>(userDto));
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromForm] RegisterDto registerDto)
    {
        try
        {
            registerDto.Email = registerDto.Email.ToLower();

            
            var user = await _userManager.Users.FirstOrDefaultAsync(c=>c.Email.ToLower()==registerDto.Email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                    return Ok(new ApiResponse(400, messageEN: "This email is already used",
                        messageAR: "هذا الحساب مستخدم من قبل"));
                var response = await GenerateTokenAndSendEmailForUser(user);

                if (!response)
                    return Ok(new ApiResponse(400, messageEN: "Failed to send email.", messageAR: "فشل ارسال الايميل"));

                return Ok(new ApiResponse(200, messageEN: "You have already registered with this Email," +
                                                          "The confirmation link will be resent to your email," +
                                                          " please check it and confirm your account.",
                    messageAR:
                    "انت مسجل مسبقا بهذا الحساب, سيتم إعادة إرسال رابط التأكيد إليك... الرجاء التأكد من صندوق الوارد لديك من اجل تاكيد حسابك"));
            }

            if (registerDto.Password != registerDto.ConfirmedPassword)
                return Ok(new ApiResponse(400, messageEN: "passwords aren't identical"));

            var photoRes = await _mediaService.AddPhotoAsync(registerDto.Image);

            if (!photoRes.Success)
                return Ok(new ApiResponse(400, photoRes.Message));

            user = new AppUser();
            user.PictureUrl = photoRes.Url;
            _mapper.Map(registerDto, user);
            user.UserName = registerDto.Email;
            user.IsNormalUser = true;
            var res = await _userManager.CreateAsync(user, registerDto.Password);

            if (res.Succeeded == false) return Ok(res.Errors);
            IdentityResult roleResult;
            roleResult = await _userManager.AddToRoleAsync(user, "User");


            if (!roleResult.Succeeded) return Ok(new ApiResponse(400, string.Join(", ", res.Errors.Select(v=>v.Description).ToList())));

            var respons = await GenerateTokenAndSendEmailForUser(user);

            if (!respons)
                return BadRequest(new ApiResponse(400, messageEN: "Failed to send email.",
                    messageAR: "حدثت مشكلة اثناء إرسال رسالة التأكيد... يرجى المحاولة لاحقا"));

            return Ok(new ApiResponse(200, messageEN: "The confirmation link was send to your email successfully, " +
                                                      "please check your email and confirm your account.",
                messageAR:
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
                if (email is null) return Ok(new ApiResponse(400, messageEN: "email must be provided"));
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);

                if (user == null)
                {
                    return Ok(new ApiResponse(401, messageEN: "user was not found"));
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var code = ConfirmEmailService.GenerateCode(user.Id, token);

                var confirmationLink = Url.Action("ResetPassword", "Account",
                    new { userId = user.Id, token = token }, Request.Scheme);

                var text = "<html><body> The code to reset your password is : " + code +
                           "</body></html>";
                var res = await _emailService.SendEmailAsync(user.Email, "Reset Password", text);

                if (!res)
                    return Ok(new ApiResponse(400, messageEN: "Failed to send email."));

                return Ok(new ApiResponse(200, messageEN: "The Code was sent to your email"));
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
                return Ok(new ApiResponse(400, messageEN: "The password should not be empty"));
            var val = ConfirmEmailService.GetUserIdAndToken(code);

            if (val is null)
                return BadRequest(new ApiResponse(400, messageEN: "the code is incorrect",
                    messageAR: "الكود خاطئ, يرجى التأكد من الكود والمحاولة لاحقا"));

            var userId = val.Value.userId;
            var token = val.Value.token;

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return Ok(new ApiResponse(401, messageEN: "this user is not registered"));

            var res = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (res.Succeeded == false) return Ok(new ApiResponse(400, messageEN: "Cannot reset password"));

            ConfirmEmailService.RemoveUserCodes(userId);
            return Ok(new ApiResponse(200, messageEN: "Password was reset successfully"));
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
            return BadRequest(new ApiResponse(400, messageEN: "invalid token user id"));
        }

        var res = await _userManager.ConfirmEmailAsync(user, token);

        if (!res.Succeeded) return BadRequest(new ApiResponse(400, messageEN: "confirmation failed"));

        return Ok("Your Email is Confirmed try to login in now");
    }

    [HttpPut("change-password")]
    public async Task<ActionResult> UpdatePassword([FromBody] UpdatePasswordDto dto)
    {
        try
        {
            var email = User.GetEmail();

            if (email == null) return Ok(new ApiResponse(401));
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user is null)
                return Unauthorized(new ApiResponse(403));

            var res =
                await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);

            if (!res.Succeeded)
                return BadRequest(new ApiResponse(400, messageEN: "Failed to update password"));

            return Ok(new ApiResponse(200, messageEN: "updated successfully"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}