using AsparagusN.DTOs;
using AsparagusN.Entities.Identity;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AsparagusN.Specifications.UserSpecifications;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.User;

[Authorize]
public class UserController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public UserController(IUnitOfWork unitOfWork, UserManager<AppUser>userManager,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
    }
    
    [HttpGet("user-info")]
    public async Task<ActionResult<UserInfoDto>> GetUserInfo()
    {
        try
        {
            var user = await GetUser();
            Console.WriteLine(user == null);

            return Ok(user == null ? new ApiResponse(404, "user not found") 
                : new ApiOkResponse<UserInfoDto>(_mapper.Map<UserInfoDto>(user)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    [HttpPut("update-user-info")]
    public async Task<ActionResult> UpdateUser(UpdateUserInfoDto updateUserInfoDto)
    {
        try
        {
            var user = await GetUser();

            if (user is null) return Ok(new ApiResponse(401, "user not fount"));

            _mapper.Map(updateUserInfoDto, user);
          _unitOfWork.Repository<AppUser>().Update(user);

         return Ok(await _unitOfWork.SaveChanges()? new ApiResponse(200, "Updated") : new ApiResponse(400, "Failed to update"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<AppUser?> GetUser()
    
    {
        var email = User.GetEmail();
        if (email == null) return null;
        var userSpec = new UserWithAddressSpecification(email) ;
        var user = await _unitOfWork.Repository<AppUser>().GetEntityWithSpec(userSpec);
        return user;
    }
}