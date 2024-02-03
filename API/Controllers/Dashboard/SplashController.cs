using AsparagusN.Entities;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard;

public class SplashController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediaService _mediaService;
    private readonly IMapper _mapper;

    public SplashController(IUnitOfWork unitOfWork,IMediaService mediaService,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mediaService = mediaService;
        _mapper = mapper;
    }

    [HttpPost("add")]
    [DisableRequestSizeLimit]
    public async Task<ActionResult> AddVideo(IFormFile file)
    {
        var res = await _mediaService.AddVideoAsync(file);

        if (!res.Success) return Ok(new ApiResponse(400,messageEN:res.Message));

        var spec = new SplashScreenAndSliderSpecification();
         var video = await _unitOfWork.Repository<MediaUrl>().GetEntityWithSpec(spec);

         if (video != null)
         {
             video.Url = res.Url;
             
             _unitOfWork.Repository<MediaUrl>().Update(video);
         }
         else
         {
             video = new MediaUrl { Url = res.Url,IsSplashScreenUrl = true};
             _unitOfWork.Repository<MediaUrl>().Add(video);
         }

         if (await _unitOfWork.SaveChanges()) return Ok(new ApiOkResponse<string>(res.Url));
        return Ok(new ApiResponse(400,messageEN:"Failed to upload video"));

    }   

    [HttpGet]
    public async Task<ActionResult<string>> GetSplashUrl()
    {
        var spec = new SplashScreenAndSliderSpecification();
        var media = await _unitOfWork.Repository<MediaUrl>().GetEntityWithSpec(spec);

        return Ok(media == null ? new ApiResponse(404,messageEN:"Video not found") 
            : new ApiOkResponse<string>(media.Url));
    }
}