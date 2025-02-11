﻿using System.ComponentModel.DataAnnotations;
using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.DTOs;
using AsparagusN.DTOs.AddressDtos;
using AsparagusN.DTOs.UserDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AsparagusN.Specifications.UserSpecifications;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers.User;

[Authorize(Roles = nameof(Roles.User))]
public class UserController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IMediaService _mediaService;

    public UserController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMapper mapper,
        IMediaService mediaService)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
        _mediaService = mediaService;
    }

    [HttpGet("user-info")]
    public async Task<ActionResult<UserInfoDto>> GetUserInfo()
    {
        try
        {
            var user = await GetUser();

            return Ok(new ApiOkResponse<UserInfoDto>(_mapper.Map<UserInfoDto>(user)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost("phone")]
    public async Task<ActionResult> UpdatePhoneNumber(
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter a valid phone number.")]
        string phoneNumber)
    {
        var user = await _userManager.Users.FirstAsync(x => x.Email == User.GetEmail());
        user.PhoneNumber = phoneNumber;

        if ((await _userManager.UpdateAsync(user)).Succeeded)
            return Ok(new ApiResponse(200, "Updated"));
        return Ok(new ApiResponse(400, "Failed to update phone number"));
    }

    [HttpPost("address")]
    public async Task<ActionResult> UpdateAddress(AddressDto address, bool homeAddress)
    {
        var user = await GetUser();
        if (homeAddress)
        {
            _mapper.Map(address, user.HomeAddress);
        }
        else
        {
            _mapper.Map(address, user.WorkAddress);
        }

        _unitOfWork.Repository<AppUser>().Update(user);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200, "Updated"));
        return Ok(new ApiResponse(400, "Failed to update phone number"));
    }

    [HttpPut("info")]
    public async Task<ActionResult> UpdateUser([FromForm] UpdateUserInfoDto dto)
    {
        try
        {
            var user = await GetUser();

            if (dto.Image != null)
            {
                var photoResult = await _mediaService.AddPhotoAsync(dto.Image);
                if (!photoResult.Success) return Ok(new ApiResponse(400, photoResult.Message));
                user.PictureUrl = photoResult.Url;
            }

            if (dto.FullName != null) user.FullName = dto.FullName;
            if (dto.Gender != null) user.Gender = dto.Gender.Value;
            _unitOfWork.Repository<AppUser>().Update(user);

            return Ok(await _unitOfWork.SaveChanges()
                ? new ApiOkResponse<UserInfoDto>(_mapper.Map<UserInfoDto>(user))
                : new ApiResponse(400, messageEN: "Failed to update"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<AppUser> GetUser()

    {
        var email = User.GetEmail();
        var userSpec = new UserWithAddressSpecification(email!);
        var user = await _unitOfWork.Repository<AppUser>().GetEntityWithSpec(userSpec);
        return user!;
    }
}