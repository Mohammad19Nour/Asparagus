using System.ComponentModel.DataAnnotations;
using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.Meal;
using AsparagusN.DTOs;
using AsparagusN.DTOs.BasketDtos;
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

        var basketCreated = false;

        if (dbBasket == null)
        {
            dbBasket = new CustomerBasket(user.Id);
            basketCreated = true;
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
        if (!basketCreated)
            _unitOfWork.Repository<CustomerBasket>().Update(dbBasket);
        else
            _unitOfWork.Repository<CustomerBasket>().Add(dbBasket);
        if (!created)
            _unitOfWork.Repository<BasketItem>().Update(oldItem);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<CustomerBasketDto>(_mapper.Map<CustomerBasketDto>(dbBasket)));
        return Ok(new ApiResponse(400, "Failed to update basket"));
    }

    [HttpPost("quantity")]
    public async Task<ActionResult<CustomerBasketDto>> UpdateQuantity(int mealId,
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be positive value")]
        int newQuantity)
    {
        var user = await _getUser();
        if (user == null) return Ok(new ApiResponse(404, "User not found"));

        var spec = new BasketSpecification(user.Id);

        var dbBasket = await _unitOfWork.Repository<CustomerBasket>().GetEntityWithSpec(spec);


        if (dbBasket == null) return Ok(new ApiResponse(400, "meal not found in your basket"));

        var meal = dbBasket.Items.FirstOrDefault(x => x.MealId == mealId);

        if (meal == null) return Ok(new ApiException(404, "Meal not found"));
        meal.Quantity = newQuantity;
        _unitOfWork.Repository<BasketItem>().Update(meal);
        _unitOfWork.Repository<CustomerBasket>().Update(dbBasket);
        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<CustomerBasketDto>(_mapper.Map<CustomerBasketDto>(dbBasket)));
        return Ok(new ApiResponse(400, "Failed to update item"));
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