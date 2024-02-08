using AsparagusN.DTOs.AdditionDtos;
using AsparagusN.Entities;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers.Dashboard;

public class ExtraController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediaService _mediaService;

    public ExtraController(IUnitOfWork unitOfWork, IMapper mapper, IMediaService mediaService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediaService = mediaService;
    }

    [HttpGet("nuts")]
    public async Task<ActionResult<List<ExtraOptionDto>>> GetNuts()
    {
        var result = await _getExtraOptions(ExtraOptionType.Nuts);
        return Ok(new ApiOkResponse<List<ExtraOptionDto>>(_mapper.Map<List<ExtraOptionDto>>(result)));
    }

    [HttpGet("salads")]
    public async Task<ActionResult<List<ExtraOptionDto>>> GetSalads()
    {
        var result = await _getExtraOptions(ExtraOptionType.Salad);
        return Ok(new ApiOkResponse<List<ExtraOptionDto>>(_mapper.Map<List<ExtraOptionDto>>(result)));
    }

    [HttpGet("sauce")]
    public async Task<ActionResult<List<ExtraOptionDto>>> GetSauces()
    {
        var result = await _getExtraOptions(ExtraOptionType.Sauce);
        return Ok(new ApiOkResponse<List<ExtraOptionDto>>(_mapper.Map<List<ExtraOptionDto>>(result)));
    }

    [HttpGet]
    public async Task<ActionResult<Dictionary<ExtraOptionType, IEnumerable<ExtraOptionDto>>>> GetAllExtraOptions()
    {
        var table = _unitOfWork.Repository<ExtraOption>().GetQueryable();
        var result = await table.Where(x => !x.IsDeleted)
            .GroupBy(x => x.OptionType).ToDictionaryAsync(g => g.Key,
                x => x.Select(y => _mapper.Map<ExtraOptionDto>(y)));

        return Ok(new ApiOkResponse<Dictionary<ExtraOptionType, IEnumerable<ExtraOptionDto>>?>(
            _mapper.Map<Dictionary<ExtraOptionType, IEnumerable<ExtraOptionDto>>>(result)));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExtraOptionDto>> GetById(int id)
    {
        var extraOpt = await _unitOfWork.Repository<ExtraOption>().GetByIdAsync(id);

        if (extraOpt == null) return Ok(new ApiResponse(404, "Not found"));

        return Ok(new ApiOkResponse<ExtraOptionDto>(_mapper.Map<ExtraOptionDto>(extraOpt)));
    }

    [HttpPost("add")]
    public async Task<ActionResult<ExtraOptionDto>> Add([FromForm] NewExtraOptionDto newExtraOptionDto)
    {
        try
        {
            var extraOption = _mapper.Map<ExtraOption>(newExtraOptionDto);
            var resultPhoto = await _mediaService.AddPhotoAsync(newExtraOptionDto.Image);

            if (!resultPhoto.Success)
                return Ok(new ApiResponse(400, resultPhoto.Message));

            extraOption.PictureUrl = resultPhoto.Url;

            _unitOfWork.Repository<ExtraOption>().Add(extraOption);

            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiOkResponse<ExtraOptionDto>(_mapper.Map<ExtraOptionDto>(extraOption)));
            return Ok(new ApiResponse(400, "Failed to add "));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost("update/{id:int}")]
    public async Task<ActionResult<ExtraOptionDto>> Update(int id, [FromForm] UpdateExtraOptionDto updateExtraOptionDto)
    {
        try
        {
            var extraOption = await _unitOfWork.Repository<ExtraOption>().GetByIdAsync(id);

            if (extraOption == null || extraOption.IsDeleted)
                return Ok(new ApiResponse(404, "Not found"));

            _mapper.Map(updateExtraOptionDto, extraOption);

            if (updateExtraOptionDto.Image != null)
            {
                var resultPhoto = await _mediaService.AddPhotoAsync(updateExtraOptionDto.Image);

                if (!resultPhoto.Success)
                    return Ok(new ApiResponse(400, resultPhoto.Message));

                extraOption.PictureUrl = resultPhoto.Url;
            }

            _unitOfWork.Repository<ExtraOption>().Update(extraOption);

            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiOkResponse<ExtraOptionDto>(_mapper.Map<ExtraOptionDto>(extraOption)));
            return Ok(new ApiResponse(400, "Failed to update "));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var extra = await _unitOfWork.Repository<ExtraOption>().GetByIdAsync(id);

        if (extra == null || extra.IsDeleted) return Ok(new ApiResponse(404, "Not found "));

        _unitOfWork.Repository<ExtraOption>().SoftDelete(extra);

        if (await _unitOfWork.SaveChanges()) return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, "Failed to delete"));
    }

    private async Task<List<ExtraOption>> _getExtraOptions(ExtraOptionType optionType)
    {
        var table = _unitOfWork.Repository<ExtraOption>().GetQueryable();
        var result = await table.Where(x => x.OptionType == optionType && !x.IsDeleted).ToListAsync();

        return result;
    }
}