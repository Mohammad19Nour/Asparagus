using AsparagusN.DTOs.BranchDtos;
using AsparagusN.Entities;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard;

public class BranchController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BranchController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
}