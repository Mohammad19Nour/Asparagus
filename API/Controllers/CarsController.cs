using AsparagusN.Data;
using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.DTOs.CarDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers;

public class CarsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICarService _carService;

    public CarsController(IUnitOfWork unitOfWork, IMapper mapper, ICarService carService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _carService = carService;
    }


    [Authorize(Roles = nameof(DashboardRoles.Car) + "," + nameof(Roles.Admin))]
    [HttpGet("booking")]
    public async Task<ActionResult> GetBooking()
    {
        var spec = new CarSpecification();
        var car = await _unitOfWork.Repository<Car>().GetEntityWithSpec(spec);

        return Ok(new ApiOkResponse<List<BookingDto>>(_mapper.Map<List<BookingDto>>(car.Bookings)));
    }

    [HttpGet]
    public async Task<ActionResult> GetCarInfo()
    {
        var spec = new CarSpecification(false);
        var car = await _unitOfWork.Repository<Car>().GetEntityWithSpec(spec);

        return Ok(new ApiOkResponse<CatInfoDto>(_mapper.Map<CatInfoDto>(car)));
    }

    //[Authorize(Roles = nameof(Roles.Admin))]
    [Authorize(Roles = nameof(DashboardRoles.Car) + "," + nameof(Roles.Admin))]
    [HttpPost]
    public async Task<ActionResult> UpdateCar(UpdateCarDto dto)
    {
        var res = await _carService.UpdateCar(dto, 1);
        if (res.car == null)
            return Ok(new ApiResponse(400, res.Message));

        return Ok(new ApiOkResponse<CatInfoDto>(_mapper.Map<CatInfoDto>(res.car)));
    }

    [Authorize(Roles = nameof(Roles.User))]
    [HttpPost("book")]
    public async Task<ActionResult> MakeBooking(DateTime start)
    {
        var email = User.GetEmail();
        var user = await _unitOfWork.Repository<AppUser>().GetQueryable()
            .Where(c => c.Email.ToLower() == email.ToLower()).FirstAsync();
        var result = await _carService.MakeBooking(user.Id, start);

        if (result.Success)
            return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, result.Message));
    }

    [HttpGet("available")]
    public async Task<ActionResult<List<List<(DateTime Start, DateTime End)>>>> GetAvailable()
    {
        var result = await _carService.GetAvailableDates();
        return Ok(new ApiOkResponse<object>(result));
    }
}