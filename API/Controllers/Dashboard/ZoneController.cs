using AsparagusN.Data.Entities;
using AsparagusN.DTOs.ZoneDtos;
using AsparagusN.Enums;
using Microsoft.AspNetCore.Authorization;

namespace AsparagusN.Controllers.Dashboard;
using DTOs.ZoneDtos;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = nameof(DashboardRoles.DeliveryZone) + ","+nameof(Roles.Admin))]
public class ZoneController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ZoneController(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpPost("add")]
    public async Task<ActionResult<ZoneDto>> AddZone(NewZoneDto newZoneDto)
    {
        try
        {
            var zone = _mapper.Map<Zone>(newZoneDto);
            
            _unitOfWork.Repository<Zone>().Add(zone);

            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiOkResponse<ZoneDto>(_mapper.Map<ZoneDto>(zone)));
            return Ok(new ApiResponse(400,"Failed to add zone"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(new ApiResponse(400,"Exception happened.. failed to add zone"));
            throw;
        }
    }
    [HttpGet]
    public async Task<ActionResult<List<ZoneDto>>> GetAllDriver()
    {
        try
        {
            var spec = new ZoneWithDriversSpecification();
            var zones = await _unitOfWork.Repository<Zone>().ListWithSpecAsync(spec);

            return Ok(new ApiOkResponse<List<ZoneDto>>(_mapper.Map<List<ZoneDto>>(zones)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(new ApiResponse(400,"Exception happened..."));
            throw;
        }
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ZoneDto>> GetZone(int id)
    {
        try
        {
            var spec = new ZoneWithDriversSpecification(id);
            var zone = await _unitOfWork.Repository<Zone>().GetEntityWithSpec(spec);

            return Ok(zone == null ? new ApiResponse(404, "Zone not found") 
                : new ApiOkResponse<ZoneDto>(_mapper.Map<ZoneDto>(zone)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(new ApiResponse(400,"Exception happened..."));
            throw;
        }
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ZoneDto>> UpdateZone(int id,UpdateZoneDto updateZoneDto)
    {
        try
        {
            var spec = new ZoneWithDriversSpecification(id);
            var zone = await _unitOfWork.Repository<Zone>().GetEntityWithSpec(spec);

            if (zone == null)
                return Ok(new ApiResponse(404, "Driver not found"));
            
            _mapper.Map(updateZoneDto, zone);

            _unitOfWork.Repository<Zone>().Update(zone);
            
            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiOkResponse<ZoneDto>(_mapper.Map<ZoneDto>(zone)));
            return Ok(new ApiResponse(400,"Failed to update  zone"));

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(new ApiResponse(400,"Exception happened.. failed to update zone"));

            throw;
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteZone(int id)
    {
        try
        {
            var zone = await _unitOfWork.Repository<Zone>().GetByIdAsync(id);

            if (zone == null)
                return Ok(new ApiResponse(404, "Zone not found"));
            
            _unitOfWork.Repository<Zone>().Delete(zone);
            
            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiResponse(200));
            return Ok(new ApiResponse(400,"Failed to delete zone"));

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(new ApiResponse(400,"Exception happened.. failed to delete zone"));

            throw;
        }
    }
}