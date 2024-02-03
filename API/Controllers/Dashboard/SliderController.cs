using AsparagusN.DTOs;
using AsparagusN.Entities;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard;

public class SliderController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediaService _mediaService;
    private readonly IMapper _mapper;

    public SliderController(IUnitOfWork unitOfWork,IMediaService mediaService,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mediaService = mediaService;
        _mapper = mapper;
    }
    [HttpPost("add-photo")]
    public async Task<ActionResult> AddPhoto([FromForm] IFormFile file)
    {
        try
        {
            var result = await _mediaService.AddPhotoAsync(file);

            if (!result.Success)
                return BadRequest(new ApiResponse(400,messageEN: result.Message));

            var photo = new MediaUrl
            {
                Url = result.Url,
                
            };
            _unitOfWork.Repository<MediaUrl>().Add(photo) ;


            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiResponse(200, messageEN:"Added successfully"));

            return BadRequest(new ApiResponse(400,messageEN: "Failed to upload photo"));
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
/*
    [HttpDelete("delete-photo")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        try
        {
            var photo = await _unitOfWork.SliderRepository.GetPhotoNoTrackingByIdAsync(photoId);

            if (photo is null) return NotFound(new ApiResponse(404, "photo not found"));

            _unitOfWork.SliderRepository.DeletePhoto(photo);

            if (!await _unitOfWork.Complete()) return BadRequest(new ApiResponse(400, "Something went wrong"));
            await _unitOfWork.PhotoRepository.DeletePhotoByIdAsync(photo.PhotoId);
            await _unitOfWork.Complete();
            if (photo.Photo.Url != null) await _photoService.DeletePhotoAsync(photo.Photo.Url);
            return Ok(new ApiResponse(200, "Deleted successfully"));

            return BadRequest(new ApiResponse(400, "Something went wrong"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
*/
}