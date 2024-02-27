using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.DTOs.CashierDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace AsparagusN.Controllers.Dashboard;

public class AdminCashiersController : BaseApiController
{
    private readonly IMediaService _mediaService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public AdminCashiersController(IMediaService mediaService, IUnitOfWork unitOfWork, IMapper mapper,
        UserManager<AppUser> userManager)
    {
        _mediaService = mediaService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpPost("add")]
    public async Task<ActionResult<AdminCashierDto>> AddCashier([FromForm] NewCashierDto newCashierDto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var branch = await _unitOfWork.Repository<Branch>().GetByIdAsync(newCashierDto.BranchId);

                if (branch == null)
                    return Ok(new ApiResponse(404, "Branch not found"));

                newCashierDto.Email = newCashierDto.Email.ToLower();

                var cashier = _mapper.Map<Cashier>(newCashierDto);

                var img = await _mediaService.AddPhotoAsync(newCashierDto.Image);
                if (!img.Success) return Ok(new ApiResponse(400, img.Message));

                cashier.PictureUrl = img.Url;
                cashier.Branch = branch;

                var cashierUser = new AppUser
                {
                    UserName = newCashierDto.Email,
                    Email = newCashierDto.Email
                };

                var result = await _userManager.CreateAsync(cashierUser, newCashierDto.Password);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return Ok(new ApiResponse(400,
                        result.Errors.Aggregate("", (error, identityError) => error + identityError.Description)));
                }

                IdentityResult roleResult = await _userManager.AddToRoleAsync(cashierUser, Roles.Cashier.GetDisplayName().ToLower());

                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new ApiResponse(400, "Failed to add roles"));
                }

                _unitOfWork.Repository<Cashier>().Add(cashier);

                if (await _unitOfWork.SaveChanges())
                {
                    await transaction.CommitAsync();
                    return Ok(new ApiOkResponse<AdminCashierDto>(_mapper.Map<AdminCashierDto>(cashier)));
                }

                await transaction.RollbackAsync();
                return Ok(new ApiResponse(400, "Failed to add cashier"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                await transaction.RollbackAsync();
                return Ok(new ApiResponse(400, "Exception happened.. failed to add cashier"));
                throw;
            }
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<AdminCashierDto>>> GetAllCashier()
    {
        try
        {
            var spec = new CashierWithBranchSpecification();
            var cashiers = await _unitOfWork.Repository<Cashier>().ListWithSpecAsync(spec);

            return Ok(new ApiOkResponse<List<AdminCashierDto>>(_mapper.Map<List<AdminCashierDto>>(cashiers)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(new ApiResponse(400, "Exception happened..."));
            throw;
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AdminCashierDto>> GetCashier(int id)
    {
        try
        {
            var spec = new CashierWithBranchSpecification(id);
            var cashier = await _unitOfWork.Repository<Cashier>().GetEntityWithSpec(spec);

            return Ok(cashier == null
                ? new ApiResponse(404, "Cashier not found")
                : new ApiOkResponse<AdminCashierDto>(_mapper.Map<AdminCashierDto>(cashier)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(new ApiResponse(400, "Exception happened..."));
            throw;
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AdminCashierDto>> UpdateCashier(int id, [FromForm] UpdateCashierDto updateCashierDto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var spec = new CashierWithBranchSpecification(id);
                var cashier = await _unitOfWork.Repository<Cashier>().GetEntityWithSpec(spec);

                if (cashier == null)
                    return Ok(new ApiResponse(404, "Cashier not found"));
                
                var cashierUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == cashier.Email);
                if (cashierUser == null) return Ok(new ApiResponse(404, "Cashier not found"));

                _mapper.Map(updateCashierDto, cashier);

                if (updateCashierDto.BranchId != null)
                {
                    var branch = await _unitOfWork.Repository<Branch>().GetByIdAsync(updateCashierDto.BranchId.Value);
                    if (branch == null)
                        return Ok(new ApiResponse(404, "Branch not found"));

                    cashier.Branch = branch;
                }
                
                if (updateCashierDto.Image != null)
                {
                    var img = await _mediaService.AddPhotoAsync(updateCashierDto.Image);
                    if (!img.Success) return Ok(new ApiResponse(400, img.Message));
                    cashier.PictureUrl = img.Url;
                }

                if (updateCashierDto.Password != null && !string.IsNullOrEmpty(updateCashierDto.Password.Trim()) && cashier.Password != updateCashierDto.Password)
                {

                    var token = await _userManager.GeneratePasswordResetTokenAsync(cashierUser);
                    var result = await _userManager.ResetPasswordAsync(cashierUser, token, updateCashierDto.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var r in result.Errors)
                        {
                            Console.WriteLine(r.Description);
                        }

                        await transaction.RollbackAsync();
                        return Ok(new ApiResponse(404, "Failed to update password"));
                    }

                    cashier.Password = updateCashierDto.Password;
                }

                
                if (updateCashierDto.Email != null && updateCashierDto.Email.ToLower( )!= cashier.Email.ToLower())
                {
                    var exist = await _userManager.Users.FirstOrDefaultAsync(x =>
                        x.Email.ToLower() == updateCashierDto.Email.ToLower());
                    if (exist != null)
                        return Ok(new ApiResponse(400, "Email already taken"));
                    
                    cashierUser.UserName = updateCashierDto.Email.ToLower();
                    cashierUser.Email = updateCashierDto.Email.ToLower();
                    cashierUser.NormalizedUserName = _userManager.NormalizeEmail(updateCashierDto.Email);
                }


                _unitOfWork.Repository<Cashier>().Update(cashier);

                if (await _unitOfWork.SaveChanges())
                {
                    await transaction.CommitAsync();
                    return Ok(new ApiOkResponse<AdminCashierDto>(_mapper.Map<AdminCashierDto>(cashier)));
                }

                await transaction.RollbackAsync();
                return Ok(new ApiResponse(400, "Failed to update  cashier"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await transaction.RollbackAsync();
                return Ok(new ApiResponse(400, "Exception happened.. failed to update cashier"));

                throw;
            }
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCashier(int id)
    {
        try
        {
            var cashier = await _unitOfWork.Repository<Cashier>().GetByIdAsync(id);

            if (cashier == null)
                return Ok(new ApiResponse(404, "Cashier not found"));

            _unitOfWork.Repository<Cashier>().Delete(cashier);

            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiResponse(200));
            return Ok(new ApiResponse(400, "Failed to delete cashier"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(new ApiResponse(400, "Exception happened.. failed to add cashier"));

            throw;
        }
    }
}