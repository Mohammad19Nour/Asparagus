using AsparagusN.DTOs.BranchDtos;
using AsparagusN.Entities;
using AsparagusN.Entities.Identity;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers.Dashboard;

public class BranchController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private IDistanceCalculationService _distanceService;

    public BranchController(IUnitOfWork unitOfWork, IMapper mapper, IDistanceCalculationService distanceService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _distanceService = distanceService;
    }

    [HttpGet("branches")]
    public async Task<ActionResult<IReadOnlyList<BranchCasherDto>>> GetBranchesName()
    {
        var spec = new BranchWithAddressSpecification();
        var results = await _unitOfWork.Repository<Branch>().ListWithSpecAsync(spec);
        var branches = _mapper.Map<IReadOnlyList<BranchCasherDto>>(results);
        return Ok(new ApiOkResponse<IReadOnlyList<BranchCasherDto>>(branches));
    }
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BranchDto>>> GetAllBranches()
    {
        var spec = new BranchWithAddressSpecification();
        var results = await _unitOfWork.Repository<Branch>().ListWithSpecAsync(spec);
        var branches = _mapper.Map<IReadOnlyList<BranchDto>>(results);
        return Ok(new ApiOkResponse<IReadOnlyList<BranchDto>>(branches));
    }


    [HttpGet("{id:int}")]
    public async Task<ActionResult<BranchDto>> GetBranch(int id)
    {
        var spec = new BranchWithAddressSpecification(id);
        var branch = await _unitOfWork.Repository<Branch>().GetEntityWithSpec(spec);
        if (branch == null) return Ok(new ApiResponse(404, "Branch not found"));
        _unitOfWork.Repository<Branch>().Update(branch);

        var result = _mapper.Map<BranchDto>(branch);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<BranchDto>(result));
        return Ok(new ApiResponse(400, "Failed to update branch"));
    }

    [HttpPost("add")]
    public async Task<ActionResult<BranchDto>> AddBranch(NewBranchDto newBranchDto)
    {
        var branch = _mapper.Map<Branch>(newBranchDto);
        _unitOfWork.Repository<Branch>().Add(branch);

        if (await _unitOfWork.SaveChanges()) return Ok(new ApiOkResponse<BranchDto>(_mapper.Map<BranchDto>(branch)));
        return Ok(new ApiResponse(400, "Failed to add branch"));
    }

    [HttpPost("update/{id:int}")]
    public async Task<ActionResult<BranchDto>> UpdateBranch(int id, UpdateBranchDto updateBranchDto)
    {
        var spec = new BranchWithAddressSpecification(id);
        var branch = await _unitOfWork.Repository<Branch>().GetEntityWithSpec(spec);

        if (branch == null) return Ok(new ApiResponse(404, "Branch not found"));
        _mapper.Map(updateBranchDto, branch);
        _unitOfWork.Repository<Branch>().Update(branch);

        if (await _unitOfWork.SaveChanges()) return Ok(new ApiOkResponse<BranchDto>(_mapper.Map<BranchDto>(branch)));
        return Ok(new ApiResponse(400, "Failed to add branch"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteBranch(int id)
    {
        var branch = await _unitOfWork.Repository<Branch>().GetByIdAsync(id);

        if (branch == null) return Ok(new ApiResponse(404, "Branch not found"));

        _unitOfWork.Repository<Branch>().Delete(branch);
        if (await _unitOfWork.SaveChanges()) return Ok(new ApiResponse(200, "deleted"));
        return Ok(new ApiResponse(400, "Failed to delete branch"));
    }

    [HttpGet("closest")]
    public async Task<ActionResult<List<BranchDto>>> GetSuggestedBranch(decimal latitude, decimal longitude)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "User not found"));

        var spec = new BranchWithAddressSpecification();
        var branches = await _unitOfWork.Repository<Branch>().ListWithSpecAsync(spec);
        branches = branches.OrderBy(branch =>
            _distanceService.GetDrivingDistanceAsync(branch.Address.Latitude, branch.Address.Longitude, latitude,
                longitude)).ToList();

        return Ok(new ApiOkResponse<List<BranchDto>>(_mapper.Map<List<BranchDto>>(branches)));
    }

    private async Task<AppUser?> _getUser()
    {
        var email = HttpContext.User.GetEmail();
        return await _unitOfWork.Repository<AppUser>().GetQueryable().FirstOrDefaultAsync(x => x.Email == email);
    }
}