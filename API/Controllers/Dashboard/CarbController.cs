using AsparagusN.Data.Entities.Meal;
using AsparagusN.DTOs.CarbDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard;

public class CarbController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediaService _mediaService;

    public CarbController(IUnitOfWork unitOfWork, IMapper mapper, IMediaService mediaService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediaService = mediaService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var carbs = (await _unitOfWork.Repository<Ingredient>().ListAllAsync()).ToList();
        carbs = carbs.Where(x => x.TypeOfIngredient == IngredientType.Carb).ToList();

        return Ok(new ApiOkResponse<List<CarbDto>>(_mapper.Map<List<CarbDto>>(carbs)));
    }
}