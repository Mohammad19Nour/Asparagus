using AsparagusN.Data.Entities;
using AsparagusN.DTOs;
using AsparagusN.DTOs.BasketDtos;
using AsparagusN.Entities;
using AsparagusN.Entities.Identity;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers;

public class BasketController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public BasketController(IMapper mapper, UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<CustomerBasketDto>> GetBasketForUser()
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "User not found"));


        var spec = new BasketSpecification(user.Id);
        var basket = await _unitOfWork.Repository<CustomerBasket>().GetEntityWithSpec(spec);

        if (basket == null)
        {
            basket = new CustomerBasket { Id = user.Id };
            _unitOfWork.Repository<CustomerBasket>().Add(basket);
            await _unitOfWork.SaveChanges();
        }

        return Ok(new ApiOkResponse<CustomerBasketDto>(_mapper.Map<CustomerBasketDto>(basket)));
    }

    [HttpPost]
    public async Task<ActionResult<CustomerBasketDto>> UpdateBasket(AddBasketItemDto basketItem)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "User not found"));

        var spec = new BasketSpecification(user.Id);
        var dbBasket = await _unitOfWork.Repository<CustomerBasket>().GetEntityWithSpec(spec);

        if (dbBasket == null)
        {
            var b = new CustomerBasket(user.Id);
            _unitOfWork.Repository<CustomerBasket>().Add(b);
            await _unitOfWork.SaveChanges();
            return Ok(new ApiResponse(400, "You don't have a this item"));
        }

        var mealSpec = new MenuMealsSpecification(basketItem.MealId);
        var meal = await _unitOfWork.Repository<Meal>().GetEntityWithSpec(mealSpec);

        if (meal == null) return Ok(new ApiException(404, "Meal not found"));

        var oldItem = dbBasket.Items.FirstOrDefault(x => x.MealId == basketItem.MealId);

        var created = false;
        if (oldItem == null)
        {
            oldItem = new BasketItem();
            _mapper.Map(meal, oldItem);
            dbBasket.Items.Add(oldItem);
            created = true;
        }

        _mapper.Map(basketItem, oldItem);
        _unitOfWork.Repository<CustomerBasket>().Update(dbBasket);
        if (!created)
            _unitOfWork.Repository<BasketItem>().Update(oldItem);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<CustomerBasketDto>(_mapper.Map<CustomerBasketDto>(dbBasket)));
        return Ok(new ApiResponse(400, "Failed to update basket"));
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteBasketItem(int mealId)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "User not found"));


        var spec = new BasketSpecification(user.Id);
        var dbBasket = await _unitOfWork.Repository<CustomerBasket>().GetEntityWithSpec(spec);
        var item = dbBasket?.Items.FirstOrDefault(x => x.MealId == mealId);

        if (dbBasket == null || item == null)
        {
            return Ok(new ApiResponse(400, "You don't have a this item"));
        }

        dbBasket.Items.Remove(item);
        _unitOfWork.Repository<CustomerBasket>().Update(dbBasket);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, "Failed to delete item"));
    }

    private async Task<AppUser?> _getUser()
    {
        var email = HttpContext.User.GetEmail();
        return await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}