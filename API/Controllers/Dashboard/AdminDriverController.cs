using AsparagusN.DTOs.DriverDtos;
using AsparagusN.Entities;
using AsparagusN.Entities.Identity;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard;

public class AdminDriverController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public AdminDriverController(IUnitOfWork unitOfWork,IMapper mapper,UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpPost("add")]
    public async Task<ActionResult<DriverDto>> AddDriver(NewDriverDto newDriverDto)
    {
        try
        {
            var zone = await _unitOfWork.Repository<Zone>().GetByIdAsync(newDriverDto.ZoneId);
            
            if (zone == null)
                return Ok(new ApiResponse(404,"Zone not found"));
            
            var driver = _mapper.Map<Driver>(newDriverDto);
            driver.Zone = zone;
            newDriverDto.UserName = newDriverDto.UserName.ToLower();
            
            var driverUser = new AppUser
            {
                UserName = newDriverDto.UserName,
                Email = newDriverDto.UserName
            };

            var result = await _userManager.CreateAsync(driverUser, newDriverDto.Password);
            if (!result.Succeeded) return Ok(new ApiResponse(400, "Failed"));
            IdentityResult roleResult =  await _userManager.AddToRoleAsync(driverUser, "Driver");
            if (!roleResult.Succeeded) return BadRequest(new ApiResponse(400, "Failed to add roles"));
            
            _unitOfWork.Repository<Driver>().Add(driver);

            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiOkResponse<DriverDto>(_mapper.Map<DriverDto>(driver)));
            return Ok(new ApiResponse(400,"Failed to add driver"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(new ApiResponse(400,"Exception happened.. failed to add driver"));
            throw;
        }
    }
    [HttpGet]
    public async Task<ActionResult<List<DriverDto>>> GetAllDriver()
    {
        try
        {
            var spec = new DriverSpecification();
            var drivers = await _unitOfWork.Repository<Driver>().ListWithSpecAsync(spec);

            return Ok(new ApiOkResponse<List<DriverDto>>(_mapper.Map<List<DriverDto>>(drivers)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(new ApiResponse(400,"Exception happened..."));
            throw;
        }
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DriverDto>> GetDriver(int id)
    {
        try
        {
            var spec = new DriverSpecification(id);
            var driver = await _unitOfWork.Repository<Driver>().GetEntityWithSpec(spec);

            return Ok(driver == null ? new ApiResponse(404, "Driver not found") : new ApiOkResponse<DriverDto>(_mapper.Map<DriverDto>(driver)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(new ApiResponse(400,"Exception happened..."));
            throw;
        }
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult<DriverDto>> UpdateDriver(int id,UpdateDriverDto updateDriverDto)
    {
        try
        {
            var spec = new DriverSpecification(id);
            var driver = await _unitOfWork.Repository<Driver>().GetEntityWithSpec(spec);

            if (driver == null)
                return Ok(new ApiResponse(404, "Driver not found"));
            
            _mapper.Map(updateDriverDto, driver);
            
            if (updateDriverDto.ZoneId != null)
            {
                var zone = await _unitOfWork.Repository<Zone>().GetByIdAsync(updateDriverDto.ZoneId.Value);
                if (zone == null)
                    return Ok(new ApiResponse(404, "Zone not found"));
                
                driver.Zone = zone;
            }

            _unitOfWork.Repository<Driver>().Update(driver);
            
            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiOkResponse<DriverDto>(_mapper.Map<DriverDto>(driver)));
            return Ok(new ApiResponse(400,"Failed to update  driver"));

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(new ApiResponse(400,"Exception happened.. failed to add driver"));

            throw;
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteDriver(int id)
    {
        try
        {
            var driver = await _unitOfWork.Repository<Driver>().GetByIdAsync(id);

            if (driver == null)
                return Ok(new ApiResponse(404, "Driver not found"));
            
            _unitOfWork.Repository<Driver>().Delete(driver);
            
            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiResponse(200));
            return Ok(new ApiResponse(400,"Failed to delete driver"));

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(new ApiResponse(400,"Exception happened.. failed to add driver"));

            throw;
        }
    }
}