using System.ComponentModel.DataAnnotations;
using System.Globalization;
using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.DTOs.DriverDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AsparagusN.Specifications.OrdersSpecifications;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace AsparagusN.Controllers.Dashboard;

public class AdminDriverController : BaseApiController
{
    private readonly IMediaService _mediaService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserPlanOrderService _userPlanOrderService;

    public AdminDriverController(IOrderService orderService, IUserPlanOrderService userPlanOrderService
        , IMediaService mediaService, IUnitOfWork unitOfWork, IMapper mapper,
        UserManager<AppUser> userManager)
    {
        _userPlanOrderService = userPlanOrderService;
        _mediaService = mediaService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpPost("add")]
    public async Task<ActionResult<AdminDriverDto>> AddDriver([FromForm] NewDriverDto newDriverDto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var zone = await _unitOfWork.Repository<Zone>().GetByIdAsync(newDriverDto.ZoneId);

                if (zone == null)
                    return Ok(new ApiResponse(404, "Zone not found"));

                newDriverDto.Email = newDriverDto.Email.ToLower();

                var driver = _mapper.Map<Driver>(newDriverDto);

                var img = await _mediaService.AddPhotoAsync(newDriverDto.Image);
                if (!img.Success) return Ok(new ApiResponse(400, img.Message));

                driver.PictureUrl = img.Url;
                driver.Zone = zone;

                var driverUser = new AppUser
                {
                    UserName = newDriverDto.Email,
                    Email = newDriverDto.Email
                };

                var result = await _userManager.CreateAsync(driverUser, newDriverDto.Password);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return Ok(new ApiResponse(400,
                        result.Errors.Aggregate("", (error, identityError) => error + identityError.Description)));
                }

                IdentityResult roleResult =
                    await _userManager.AddToRoleAsync(driverUser, Roles.Driver.GetDisplayName().ToLower());

                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new ApiResponse(400, "Failed to add roles"));
                }

                _unitOfWork.Repository<Driver>().Add(driver);

                if (await _unitOfWork.SaveChanges())
                {
                    await transaction.CommitAsync();
                    return Ok(new ApiOkResponse<AdminDriverDto>(_mapper.Map<AdminDriverDto>(driver)));
                }

                await transaction.RollbackAsync();
                return Ok(new ApiResponse(400, "Failed to add driver"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                await transaction.RollbackAsync();
                return Ok(new ApiResponse(400, "Exception happened.. failed to add driver"));
                throw;
            }
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<AdminDriverDto>>> GetAllDriver()
    {
        try
        {
            var spec = new DriverSpecification();
            var drivers = await _unitOfWork.Repository<Driver>().ListWithSpecAsync(spec);

            return Ok(new ApiOkResponse<List<AdminDriverDto>>(_mapper.Map<List<AdminDriverDto>>(drivers)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(new ApiResponse(400, "Exception happened..."));
            throw;
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AdminDriverDto>> GetDriver(int id)
    {
        try
        {
            var spec = new DriverSpecification(id);
            var driver = await _unitOfWork.Repository<Driver>().GetEntityWithSpec(spec);

            return Ok(driver == null
                ? new ApiResponse(404, "Driver not found")
                : new ApiOkResponse<AdminDriverDto>(_mapper.Map<AdminDriverDto>(driver)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(new ApiResponse(400, "Exception happened..."));
            throw;
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AdminDriverDto>> UpdateDriver(int id, [FromForm] UpdateDriverDto updateDriverDto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var spec = new DriverSpecification(id);
                var driver = await _unitOfWork.Repository<Driver>().GetEntityWithSpec(spec);

                if (driver == null)
                    return Ok(new ApiResponse(404, "Driver not found"));

                var driverUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == driver.Email);
                if (driverUser == null) return Ok(new ApiResponse(404, "Driver not found"));

                _mapper.Map(updateDriverDto, driver);

                if (updateDriverDto.ZoneId != null)
                {
                    var zone = await _unitOfWork.Repository<Zone>().GetByIdAsync(updateDriverDto.ZoneId.Value);
                    if (zone == null)
                        return Ok(new ApiResponse(404, "Zone not found"));

                    driver.Zone = zone;
                }

                if (updateDriverDto.Image != null)
                {
                    var img = await _mediaService.AddPhotoAsync(updateDriverDto.Image);
                    if (!img.Success) return Ok(new ApiResponse(400, img.Message));
                    driver.PictureUrl = img.Url;
                }

                if (updateDriverDto.Password != null && !string.IsNullOrEmpty(updateDriverDto.Password.Trim()) &&
                    updateDriverDto.Password != driver.Password)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(driverUser);
                    var result = await _userManager.ResetPasswordAsync(driverUser, token, updateDriverDto.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var r in result.Errors)
                        {
                            Console.WriteLine(r.Description);
                        }

                        await transaction.RollbackAsync();
                        return Ok(new ApiResponse(404, "Failed to update password"));
                    }

                    driver.Password = updateDriverDto.Password;
                }

                if (updateDriverDto.Email != null && updateDriverDto.Email.ToLower() != driver.Email.ToLower())
                {
                    var exist = await _userManager.Users.FirstOrDefaultAsync(x =>
                        x.Email.ToLower() == updateDriverDto.Email.ToLower());
                    if (exist != null)
                        return Ok(new ApiResponse(400, "Email already taken"));

                    driverUser.UserName = updateDriverDto.Email.ToLower();
                    driverUser.Email = updateDriverDto.Email.ToLower();
                    driverUser.NormalizedUserName = _userManager.NormalizeEmail(updateDriverDto.Email);
                }

                _unitOfWork.Repository<Driver>().Update(driver);

                if (await _unitOfWork.SaveChanges())
                {
                    await transaction.CommitAsync();
                    return Ok(new ApiOkResponse<AdminDriverDto>(_mapper.Map<AdminDriverDto>(driver)));
                }

                await transaction.RollbackAsync();
                return Ok(new ApiResponse(400, "Failed to update  driver"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await transaction.RollbackAsync();
                return Ok(new ApiResponse(400, "Exception happened.. failed to update driver"));

                throw;
            }
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteDriver(int id)
    {
        try
        {
            var driver = await _unitOfWork.Repository<Driver>().GetByIdAsync(id);

            if (driver == null)
                return Ok(new ApiResponse(404, "Driver not found"));

            _unitOfWork.Repository<Driver>().Delete(driver);

            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiResponse(200));
            return Ok(new ApiResponse(400, "Failed to delete driver"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(new ApiResponse(400, "Exception happened.. failed to add driver"));

            throw;
        }
    }

    [HttpPost("assign")]
    public async Task<ActionResult> AssignOrder([FromQuery] int orderId, [FromQuery] int driverId,
        [FromQuery] int priority)
    {
        var result = await _userPlanOrderService.AssignPlanDayOrderToDriver(orderId, driverId, priority);
        if (result.Success) return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, result.Message));
    }

    [HttpGet("orders")]
    public async Task<ActionResult> GetPlanDayOrders(PlanOrderStatus status, string day)
    {
        DateTime resultDateTime;
        if (!DateTime.TryParseExact(day.ToString(CultureInfo.InvariantCulture), "dd-MM-yyyy",
                System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
                out resultDateTime))
        {
            return Ok(new ApiResponse(400, "the date format must be in dd-MM-yyyy format"));
        }

        var spec = new PlanDayOrdersWithItemsAndDriverSpecification(status, resultDateTime);
        var orders = await _unitOfWork.Repository<UserPlanDay>().ListWithSpecAsync(spec);

        return Ok(orders);
    }
}