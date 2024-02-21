using AsparagusN.DTOs;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers;

public class NotificationsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;

    public NotificationsController(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _notificationService = notificationService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<NotificationDto>>> GetAllUserNotifications()
    {
        var email = User!.GetEmail()!;
        var result = await _notificationService.GetAllNotificationsForUserAsync(email);

        var notifications = _mapper.Map<List<NotificationDto>>(result);
        return Ok(new ApiOkResponse<List<NotificationDto>>(notifications));
    }

    [HttpPost]
    public async Task<ActionResult<NotificationDto>> SendNotification(NotificationType type, NewNotificationDto dto)
    {
        var result = false;
        if (type == NotificationType.SingleUser)
        {
            if (dto.UserEmail == null) return Ok(new ApiResponse(404, "Email should be specified"));
            result = await _notificationService.NotifyUserByEmail(dto.UserEmail, dto.ArabicContent, dto.EnglishContent);
        }
        else
        {
            if (type == NotificationType.AllNormalUsers)
                result = await _notificationService.NotifyAllNormalUsers(dto.ArabicContent, dto.EnglishContent);
            else result = await _notificationService.NotifyAllMealPlanUsers(dto.ArabicContent, dto.EnglishContent);
        }

        return Ok(new ApiOkResponse<bool>(result));
    }
}