using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.DTOs.CarDtos;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers;

public class CarsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICarService _carService;

    public CarsController(IUnitOfWork unitOfWork, IMapper mapper,ICarService carService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _carService = carService;
    }

    /*[HttpGet]
    public async Task<ActionResult> GetCarInfo()
    {
        var spec = new CarSpecification();
        var car = await _unitOfWork.Repository<Car>().GetEntityWithSpec(spec);

        return Ok(new ApiOkResponse<CatInfoDto>(_mapper.Map<CatInfoDto>(car)));
    }*/

    [HttpPost]
    public async Task<ActionResult> UpdateCar(UpdateCarDto dto)
    {
      var res = await  _carService.UpdateCar(dto, 1);
      return Ok(res);
    }

    [HttpPost("book")]
    public async Task<ActionResult> MakeBooking(DateTime start)
    {
        var email = User.GetEmail();
        var user = await _unitOfWork.Repository<AppUser>().GetQueryable()
            .Where(c => c.Email.ToLower() == email.ToLower()).FirstAsync();
        var result = await _carService.MakeBooking(user.Id,start);
        return Ok(result);
    }
    [HttpGet("available")]
    public async Task<ActionResult<List<List<(DateTime Start,DateTime End)>>>> GetAvailable()
    {
       
        var result = await _carService.GetAvailableDates();
        return Ok(result);
    }
}