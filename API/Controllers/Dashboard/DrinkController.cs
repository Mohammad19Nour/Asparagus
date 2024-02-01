using AsparagusN.Data.Additions;
using AsparagusN.DTOs.AdditionDtos;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard;

public class DrinkController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DrinkController(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DrinkDto>>> GetAll()
    {
        var res = await _unitOfWork.Repository<Drink>().ListAllAsync();
        var drinks = _mapper.Map<IReadOnlyList<DrinkDto>>(res);

        return Ok(new ApiOkResponse<IReadOnlyList<DrinkDto>>(drinks));
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DrinkDto>> GetById(int id)
    {
        var res = await _unitOfWork.Repository<Drink>().GetByIdAsync(id);
        var drink = _mapper.Map<DrinkDto>(res);

        if (drink == null)
            return Ok(new ApiResponse(404,"not found"));

        return Ok(new ApiOkResponse<DrinkDto>(drink));
    }

    [HttpPost("add")]
    public async Task<ActionResult<DrinkDto>> Add(NewDrinkDto newDrinkDto)
    {
        var drink = _mapper.Map<Drink>(newDrinkDto);
        _unitOfWork.Repository<Drink>().Add(drink);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<DrinkDto>(_mapper.Map<DrinkDto>(drink)));

        return Ok(new ApiResponse(400, "Failed to add"));   
    }
}