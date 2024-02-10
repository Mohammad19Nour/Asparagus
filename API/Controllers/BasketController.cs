using AsparagusN.Data.Entities;
using AsparagusN.DTOs;
using AsparagusN.DTOs.BasketDtos;
using AsparagusN.Entities;
using AsparagusN.Entities.Identity;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers;

public class BasketController : BaseApiController
{
    private readonly IBasketRepository _basketRepository;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public BasketController(IBasketRepository basketRepository, IMapper mapper, UserManager<AppUser> userManager)
    {
        _basketRepository = basketRepository;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<CustomerBasket?>> GetBasketForUser()
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "User not found"));

        var basket = await _basketRepository.GetBasketAsync(user.Id);
        return Ok(new ApiOkResponse<CustomerBasket?>(basket));
    }

    [HttpPost]
    public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "User not found"));

        basket.Id = user.Id;
        var cnt = basket.Items.Select(x => x.MealId).Distinct();
        if (cnt.Count() != basket.Items.Count)
            return Ok(new ApiResponse(400, "Duplicate meal"));
        
        var customerBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
        var updatedBasket = await _basketRepository.UpdateBasket(customerBasket);
        if (updatedBasket == null) return Ok(new ApiResponse(400, "Basket not updated"));
        return Ok(new ApiOkResponse<CustomerBasket>(updatedBasket));
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteBasket()
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "User not found"));

        await _basketRepository.DeleteBasket(user.Id);
        return Ok(new ApiResponse(200));
    }

    private async Task<AppUser?> _getUser()
    {
        var email = HttpContext.User.GetEmail();
        return await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}