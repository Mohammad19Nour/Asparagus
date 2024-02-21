using AsparagusN.Data.Entities.Identity;
using AsparagusN.DTOs;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard;

public class AdminUsersController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AdminUsersController(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<AdminUserDto>>> GetUsers()
    {
        var spec = new CustomersSpecification();
        var users = (await _unitOfWork.Repository<AppUser>().ListWithSpecAsync(spec)).ToList();

        var mappedUsers = _mapper.Map<List<AdminUserDto>> (users);
        return Ok(new ApiOkResponse<List<AdminUserDto>>(mappedUsers));
    }
}