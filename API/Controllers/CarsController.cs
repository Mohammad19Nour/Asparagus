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

        var d = _mapper.Map<List<BookingDto>>(car.Bookings);

        foreach (var a in d)
        {
            a.City = car.City;
        }
        return Ok(new ApiOkResponse<List<BookingDto>>(d));
    }

    [HttpGet]
    public async Task<ActionResult> GetCarInfo([FromQuery] string city)
    {
        var spec = new CarSpecification(city,false);
        var car = await _unitOfWork.Repository<Car>().GetEntityWithSpec(spec);

        return Ok(new ApiOkResponse<CatInfoDto>(_mapper.Map<CatInfoDto>(car)));
    }

    //[Authorize(Roles = nameof(Roles.Admin))]
    [Authorize(Roles = nameof(DashboardRoles.Car) + "," + nameof(Roles.Admin))]
    [HttpPost]
    public async Task<ActionResult> UpdateCar(UpdateCarDto dto )
    {
        var spec = new CarSpecification(dto.City, false);
        var car = await _unitOfWork.Repository<Car>().GetEntityWithSpec(spec);
        if (car == null) return Ok(new ApiResponse(404, "Car not found"));
        
        var res = await _carService.UpdateCar(dto, car.Id);
        if (res.car == null)
            return Ok(new ApiResponse(400, res.Message));

        return Ok(new ApiOkResponse<CatInfoDto>(_mapper.Map<CatInfoDto>(res.car)));
    }

    [Authorize(Roles = nameof(Roles.User))]
    [HttpPost("book")]
    public async Task<ActionResult> MakeBooking(DateTime start,string city)
    {
        var email = User.GetEmail();
        var user = await _unitOfWork.Repository<AppUser>().GetQueryable()
            .Where(c => c.Email.ToLower() == email.ToLower()).FirstAsync();
        var result = await _carService.MakeBooking(user.Id, start,city);

        if (result.Success)
            return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, result.Message));
    }

    [HttpGet("available")]
    public async Task<ActionResult<List<List<(DateTime Start, DateTime End)>>>> GetAvailable(string city)
    {
        var result = await _carService.GetAvailableDates(city);
        return Ok(new ApiOkResponse<object>(result));
    }
}