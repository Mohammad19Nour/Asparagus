using AsparagusN.Data.Entities;
using AsparagusN.DTOs.CouponDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace AsparagusN.Controllers;

[Authorize]
public class CouponController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CouponController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<CouponDto>>> GetAll()
    {
        var coupons = await _unitOfWork.Repository<AppCoupon>().ListAllAsync();
        return Ok(new ApiOkResponse<List<CouponDto>>(_mapper.Map<List<CouponDto>>(coupons.ToList())));
    }

    [HttpPost]
    public async Task<ActionResult<CouponDto>> AddCoupon(NewCouponDto newCoupon)
    {
        var exist = (await _unitOfWork.Repository<AppCoupon>().GetQueryable().Where(x => x.Code == newCoupon.Code)
            .FirstOrDefaultAsync());

        if (exist != null)
            return Ok(new ApiResponse(400, "Coupon already exist"));

        if (newCoupon.Type == AppCouponType.Percent && newCoupon.Value > 100)
            return Ok(new ApiResponse(400, "Value should be not greater than 100"));
        var coupon = _mapper.Map<AppCoupon>(newCoupon);
        _unitOfWork.Repository<AppCoupon>().Add(coupon);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<CouponDto>(_mapper.Map<CouponDto>(coupon)));
        return Ok(new ApiResponse(400, "Failed to add coupon"));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CouponDto>> UpdateCoupon(int id, UpdateCouponDto dto)

    {
        var coupon = await _unitOfWork.Repository<AppCoupon>().GetByIdAsync(id);

        if (coupon == null)
            return Ok(new ApiResponse(404, "Coupon not found"));

        if (dto.Type == AppCouponType.Percent && dto.Value > 100)
            return Ok(new ApiResponse(400, "Value should be not greater than 100"));
        _mapper.Map(dto, coupon);
        _unitOfWork.Repository<AppCoupon>().Update(coupon);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<CouponDto>(_mapper.Map<CouponDto>(coupon)));
        return Ok(new ApiResponse(400, "Failed to update coupon"));
    }
[Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCoupon(int id)
    {
        var coupon = await _unitOfWork.Repository<AppCoupon>().GetByIdAsync(id);

        if (coupon == null)
            return Ok(new ApiResponse(404, "Coupon not found"));
        _unitOfWork.Repository<AppCoupon>().Delete(coupon);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, "Failed to update coupon"));
    }

    [HttpGet("check")]
    public async Task<ActionResult<bool>> CheckCoupon(string code)
    {
        var exist = (await _unitOfWork.Repository<AppCoupon>().GetQueryable().Where(x => x.Code == code)
            .FirstOrDefaultAsync());

        if (exist == null)
            return Ok(new ApiResponse(400, "Coupon not exist"));


        return Ok(new ApiOkResponse<CouponDto>(_mapper.Map<CouponDto>(exist)));
    }
}