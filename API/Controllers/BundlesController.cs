using AsparagusN.Data.Entities;
using AsparagusN.DTOs.BundleDtos;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers;

public class BundlesController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BundlesController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<BundleDto>>> GetBundles()
    {
        var result = await _unitOfWork.Repository<Bundle>().ListAllAsync();
        var bundles = _mapper.Map<List<BundleDto>>(result);
        return Ok(new ApiOkResponse<List<BundleDto>>(bundles));
    }

    [HttpPost]
    public async Task<ActionResult<BundleDto>> AddBundle(NewBundleDto dto)
    {
        var bundle = _mapper.Map<Bundle>(dto);
        _unitOfWork.Repository<Bundle>().Add(bundle);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<BundleDto>(_mapper.Map<BundleDto>(bundle)));

        return Ok(new ApiResponse(400, "Failed to add bundle"));
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult<BundleDto>> UpdateBundle(UpdateBundleDto dto, int id)
    {
        var bundle = await _unitOfWork.Repository<Bundle>().GetByIdAsync(id);
        if (bundle == null)
            return Ok(new ApiResponse(404, "Bundle not found"));

        _mapper.Map(dto, bundle);
        _unitOfWork.Repository<Bundle>().Update(bundle);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<BundleDto>(_mapper.Map<BundleDto>(bundle)));

        return Ok(new ApiResponse(400, "Failed to update bundle"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<bool>> DeleteBundle(int id)
    {
        var bundle = await _unitOfWork.Repository<Bundle>().GetByIdAsync(id);
        if (bundle == null)
            return Ok(new ApiResponse(404, "Bundle not found"));

        _unitOfWork.Repository<Bundle>().Delete(bundle);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<bool>(true));

        return Ok(new ApiResponse(400, "Failed to update bundle"));
    }
}