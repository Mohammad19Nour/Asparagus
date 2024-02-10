using AsparagusN.DTOs;
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

    public BasketController(IBasketRepository basketRepository,IMapper mapper,UserManager<AppUser>userManager)
    {
        _basketRepository = basketRepository;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<CustomerBasket?>> GetBasketForUser()
    {
        var email = HttpContext.User.GetEmail();
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);

        if (user == null) return Ok(new ApiResponse(404, "User not found"));
        
        var basket = await _basketRepository.GetBasketAsync(user.Id);
        return Ok(basket);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
    {
        var customerBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
        var updatedBasket = await _basketRepository.UpdateBasket(customerBasket);
        return Ok(updatedBasket);
    }

    [HttpDelete]
    public async Task DeleteBasket(int basketId)
    {
        await _basketRepository.DeleteBasket(basketId);
    }
}