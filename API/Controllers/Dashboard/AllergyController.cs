using AsparagusN.Data.Entities.Meal;
using AsparagusN.DTOs.AllergyDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard;

public class AllergyController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediaService _mediaService;

    public AllergyController(IUnitOfWork unitOfWork, IMapper mapper,IMediaService mediaService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediaService = mediaService;
    }
    [Authorize(Roles=nameof(Roles.Admin))]


    [HttpPost("add")]
    public async Task<ActionResult<AllergyDto>> Add([FromForm]NewAllergyDto newAllergyDto)
    {
        var allergy = _mapper.Map<Allergy>(newAllergyDto);
        var result = await _mediaService.AddPhotoAsync(newAllergyDto.Image);

        if (!result.Success) return Ok(new ApiResponse(400, result.Message));
        
        allergy.PictureUrl = result.Url;
        
        _unitOfWork.Repository<Allergy>().Add(allergy);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<AllergyDto>(_mapper.Map<AllergyDto>(allergy)));

        return Ok(new ApiResponse(400, "Failed to add allergy"));
    }

    [HttpGet]
    public async Task<ActionResult<List<AllergyDto>>> GetAll()
    {
        var allergies = await _unitOfWork.Repository<Allergy>().ListAllAsync();

        return Ok(new ApiOkResponse<List<AllergyDto>>(_mapper.Map<List<AllergyDto>>(allergies)));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AllergyDto>> GetById(int id)
    {
        var allergy = await _unitOfWork.Repository<Allergy>().GetByIdAsync(id);

        return Ok(allergy == null
            ? new ApiResponse(404, "Allergy not found")
            : new ApiOkResponse<AllergyDto>(_mapper.Map<AllergyDto>(allergy)));
    }
    [Authorize(Roles=nameof(Roles.Admin))]


    [HttpPost("update/{id:int}")]
    public async Task<ActionResult<AllergyDto>> Update(int id,[FromForm] UpdateAllergyDto updateAllergyDto)
    {
        var allergy = await _unitOfWork.Repository<Allergy>().GetByIdAsync(id);
        if (allergy == null)
            return Ok(new ApiResponse(404, "Allergy not found"));

        _mapper.Map(updateAllergyDto, allergy);

        if (updateAllergyDto.Image != null)
        {
            var result = await _mediaService.AddPhotoAsync(updateAllergyDto.Image);

            if (!result.Success) return Ok(new ApiResponse(400, result.Message));
        
            allergy.PictureUrl = result.Url;
        }
        _unitOfWork.Repository<Allergy>().Update(allergy);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<AllergyDto>(_mapper.Map<AllergyDto>(allergy)));
        return Ok(new ApiResponse(400, "Failed to add allergy"));
    }
    [Authorize(Roles=nameof(Roles.Admin))]

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var allergy = await _unitOfWork.Repository<Allergy>().GetByIdAsync(id);
        if (allergy == null)
            return Ok(new ApiResponse(404, "Allergy not found"));

        _unitOfWork.Repository<Allergy>().Delete(allergy);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, "Failed to add allergy"));
    }
}