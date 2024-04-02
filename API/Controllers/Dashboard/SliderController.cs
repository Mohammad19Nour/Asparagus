using AsparagusN.Data.Entities;
using AsparagusN.DTOs;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard;

public class SliderController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediaService _mediaService;
    private readonly IMapper _mapper;

    public SliderController(IUnitOfWork unitOfWork, IMediaService mediaService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mediaService = mediaService;
        _mapper = mapper;
    }

    [Authorize
        (Roles = nameof(DashboardRoles.Slider) + "," + nameof(Roles.Admin))]
    [HttpPost("add-photo")]
    public async Task<ActionResult> AddPhoto(IFormFile file)
    {
        try
        {
            var result = await _mediaService.AddPhotoAsync(file);

            if (!result.Success)
                return Ok(new ApiResponse(400, messageEN: result.Message));

            var photo = new MediaUrl
            {
                Url = result.Url,
            };
            _unitOfWork.Repository<MediaUrl>().Add(photo);


            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiResponse(200, messageEN: "Added successfully"));

            return Ok(new ApiResponse(400, messageEN: "Failed to upload photo"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MediaUrlDto>>> GetSliderPhotos()
    {
        var spec = new SplashScreenAndSliderSpecification(isSplash: false);
        var res = await _unitOfWork.Repository<MediaUrl>().ListWithSpecAsync(spec);
        return Ok(new ApiOkResponse<IReadOnlyList<MediaUrlDto>>(_mapper.Map<IReadOnlyList<MediaUrlDto>>(res)));
    }

    
    [Authorize
        (Roles = nameof(DashboardRoles.Slider) + "," + nameof(Roles.Admin))]
    [HttpDelete("delete/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        try
        {
            var spec = new SplashScreenAndSliderSpecification(sliderPhotoId: photoId);
            var photo = await _unitOfWork.Repository<MediaUrl>().GetEntityWithSpec(spec);

            if (photo is null) return Ok(new ApiResponse(404, "photo not found"));

            _unitOfWork.Repository<MediaUrl>().Delete(photo);

            if (!await _unitOfWork.SaveChanges()) return BadRequest(new ApiResponse(400, "Something went wrong"));

            await _mediaService.DeletePhotoAsync(photo.Url);
            return Ok(new ApiResponse(200, "Deleted successfully"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}