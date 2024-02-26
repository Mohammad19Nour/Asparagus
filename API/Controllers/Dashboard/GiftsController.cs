using System.ComponentModel.DataAnnotations;
using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Meal;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers.Dashboard;

public class GiftsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GiftsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpPost("{month:int}")]
    public async Task<ActionResult> AddOrUpdateGiftToMonth(
        [Range(1, 12, ErrorMessage = "Month should be a number between 1-12")]
        int month, int mealId)
    {
        var gifts = await _unitOfWork.Repository<GiftSelection>().ListAllAsync();

        var meal = await _unitOfWork.Repository<Meal>().GetByIdAsync(mealId);

        if (meal == null) return Ok(new ApiResponse(400, "Meal not found"));

        var gift = gifts.FirstOrDefault(x => x.Month == month);

        var exist = true;

        if (gift == null)
        {
            exist = false;
            gift = new GiftSelection
            {
                Meal = meal,
                Month = month
            };
            _unitOfWork.Repository<GiftSelection>().Add(gift);
        }
        else
        {
            gift.Meal = meal;
            _unitOfWork.Repository<GiftSelection>().Update(gift);
        }

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200, "Added successfully"));
        return Ok(new ApiResponse(400, "Failed to add gift"));
    }

    [HttpDelete("{month:int}")]
    public async Task<ActionResult> DeleteGiftToMonth(
        [Range(1, 12, ErrorMessage = "Month should be a number between 1-12")]
        int month)
    {
        var gifts = await _unitOfWork.Repository<GiftSelection>().ListAllAsync();

        var gift = gifts.FirstOrDefault(x => x.Month == month);

        if (gift == null || gift.MealId == null)
            return Ok(new ApiResponse(400, "This month has no meal"));

        gift.MealId = null;
        _unitOfWork.Repository<GiftSelection>().Update(gift);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200, "Deleted successfully"));
        return Ok(new ApiResponse(400, "Failed to delete gift"));
    }

    [HttpGet]
    public async Task<ActionResult> GetGifts()
    {
        var gifts = await _unitOfWork.Repository<GiftSelection>().GetQueryable().Include(y => y.Meal).ToListAsync();

        return Ok(new ApiOkResponse<object>(gifts));
    }
}