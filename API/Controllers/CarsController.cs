using AsparagusN.Data.Entities.Identity;
using AsparagusN.DTOs.CarDtos;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
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
}