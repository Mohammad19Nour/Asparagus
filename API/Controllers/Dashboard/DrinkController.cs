using AsparagusN.Data.Entities;
using AsparagusN.DTOs.DrinksDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard;

public class DrinkController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediaService _mediaService;

    public DrinkController(IUnitOfWork unitOfWork, IMapper mapper, IMediaService mediaService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediaService = mediaService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DrinkDto>> GetById(int id)
    {
        var drink = await _unitOfWork.Repository<Drink>().GetByIdAsync(id);

        if (drink == null || drink.IsDeleted)
            return Ok(new ApiResponse(404, "drink not found"));
        return Ok(new ApiOkResponse<DrinkDto>(_mapper.Map<DrinkDto>(drink)));
    }

    [HttpGet]
    public async Task<ActionResult<List<DrinkDto>>> GetAll()
    {
        var drinks = await _unitOfWork.Repository<Drink>().ListAllAsync();
        drinks = drinks.Where(x => !x.IsDeleted).ToList();

        return Ok(new ApiOkResponse<List<DrinkDto>>(_mapper.Map<List<DrinkDto>>(drinks)));
    }

    [Authorize(Roles = nameof(DashboardRoles.Drink) + "," + nameof(Roles.Admin))]
    [HttpPost("add")]
    public async Task<ActionResult> Add([FromForm] NewDrinkDto newDrinkDto)
    {
        var resultPhoto = await _mediaService.AddPhotoAsync(newDrinkDto.Image);
        if (!resultPhoto.Success) return Ok(new ApiResponse(400, resultPhoto.Message));

        var drink = _mapper.Map<Drink>(newDrinkDto);
        drink.PictureUrl = resultPhoto.Url;

        _unitOfWork.Repository<Drink>().Add(drink);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<DrinkDto>(_mapper.Map<DrinkDto>(drink)));

        return Ok(new ApiResponse(400, "Failed to add drink"));
    }

    [Authorize(Roles = nameof(DashboardRoles.Drink) + "," + nameof(Roles.Admin))]
    [HttpPost("update/{id:int}")]
    public async Task<ActionResult> Update(int id, [FromForm] UpdateDrinkDto updateDrinkDto)
    {
        var drink = await _unitOfWork.Repository<Drink>().GetByIdAsync(id);

        if (drink == null || drink.IsDeleted)
            return Ok(new ApiResponse(404, "drink not found"));

        _mapper.Map(updateDrinkDto, drink);

        if (updateDrinkDto.Image != null)
        {
            var resultPhoto = await _mediaService.AddPhotoAsync(updateDrinkDto.Image);
            if (!resultPhoto.Success) return Ok(new ApiResponse(400, resultPhoto.Message));

            drink.PictureUrl = resultPhoto.Url;
        }

        _unitOfWork.Repository<Drink>().Update(drink);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<DrinkDto>(_mapper.Map<DrinkDto>(drink)));

        return Ok(new ApiResponse(400, "Failed to update drink"));
    }

    [Authorize(Roles = nameof(DashboardRoles.Drink) + "," + nameof(Roles.Admin))]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var drink = await _unitOfWork.Repository<Drink>().GetByIdAsync(id);

        if (drink == null || drink.IsDeleted)
            return Ok(new ApiResponse(404, "drink not found"));

        _unitOfWork.Repository<Drink>().Delete(drink);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200));

        return Ok(new ApiResponse(400, "Failed to update drink"));
    }
}