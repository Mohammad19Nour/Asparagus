﻿using AsparagusN.Data.Additions;
using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.DTOs;
using AsparagusN.DTOs.AdminPlanDtos;
using AsparagusN.DTOs.MealDtos;
using AsparagusN.Entities;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard;

public class PlanController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PlanController(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet("days")]
    public async Task<ActionResult> GetAllPlans()
    {
        var plans = await _unitOfWork.Repository<AdminPlan>().ListAllAsync();
        var groupedPlans = plans.GroupBy(plan => plan.PlanType)
            .ToDictionary(g => g.Key,
                g => g.Select(x => new PlanDayDto{Id = x.Id,Day = x.AvailableDate}).ToList());
       
      
        var response = new ApiOkResponse<Dictionary<MealPlanType, List<PlanDayDto>>>(groupedPlans);
        return Ok(response);
    }
    [HttpGet("day")]
    public async Task<ActionResult<AdminPlanDto>> GetPlanDay([FromQuery] int dayId)
    {
        var spec = new AdminPlanSpecification(dayId) ;
        
        var plan = await _unitOfWork.Repository<AdminPlan>().GetEntityWithSpec(spec);

        if (plan == null) return Ok(new ApiResponse(404, "Plan not found"));
        
       // var response = new ApiOkResponse<Dictionary<MealPlanType, List<PlanDayDto>>>(groupedPlans);
        return Ok(new ApiOkResponse<AdminPlanDto>(_mapper.Map<AdminPlanDto>(plan)));
    }
    
    [HttpGet("plan-menu")]
    public async Task<ActionResult<List<MealDto>>> GetMenuMealPlan()
    {
        var spec = new MealWithIngredientsAdnAllergiesSpecification(includeIngredients:true,mealPlansOnly:true);
        var d = await _unitOfWork.Repository<Meal>().ListWithSpecAsync(spec);

        return Ok(_mapper.Map<List<MealDto>>(d));
    }

    [HttpPost("additems/{dayId:int}")]
    public async Task<ActionResult<AdminPlanDto>> AddItems(NewAdminPlanDto newAdminPlanDto, int dayId)
    {
        var ok = true ;

        var spec = new AdminPlanSpecification(dayId) ;
        
        var plan = await _unitOfWork.Repository<AdminPlan>().GetEntityWithSpec(spec);

        if (plan == null) return Ok(new ApiResponse(404, "Plan not found"));

        if (newAdminPlanDto.MealIds != null)
        {
           // return Ok(plan);
             ok = await  _addMeals(newAdminPlanDto.MealIds, plan);

        }
        if (newAdminPlanDto.DrinkIds != null)
        {
            ok &= await  _addDrinks(newAdminPlanDto.DrinkIds, plan);
        }
        if (newAdminPlanDto.SaladIds != null)
        {
            
        }
        if (newAdminPlanDto.SauceIds != null)
        {
            
        }
        if (newAdminPlanDto.NutIds != null)
        {
            
        }
        
        _unitOfWork.Repository<AdminPlan>().Update(plan);
        await _unitOfWork.SaveChanges();

        if (!ok) return Ok(new ApiResponse(404, "not found"));

        return Ok(new ApiOkResponse<AdminPlanDto>(_mapper.Map<AdminPlanDto>(plan)));
    }
    
    [HttpDelete("deleteItems/{dayId:int}")]
    public async Task<ActionResult<AdminPlanDto>> DeleteItems(NewAdminPlanDto newAdminPlanDto, int dayId)
    {
        var ok = true ;

        var spec = new AdminPlanSpecification(dayId) ;
        
        var plan = await _unitOfWork.Repository<AdminPlan>().GetEntityWithSpec(spec);

        if (plan == null) return Ok(new ApiResponse(404, "Plan not found"));

        if (newAdminPlanDto.MealIds != null)
        {
            ok = await  _addMeals(newAdminPlanDto.MealIds, plan);

        }
        if (newAdminPlanDto.DrinkIds != null)
        {
            ok &= await  _addDrinks(newAdminPlanDto.DrinkIds, plan);
        }
        if (newAdminPlanDto.SaladIds != null)
        {
            
        }
        if (newAdminPlanDto.SauceIds != null)
        {
            
        }
        if (newAdminPlanDto.NutIds != null)
        {
            
        }
        
        _unitOfWork.Repository<AdminPlan>().Update(plan);
        await _unitOfWork.SaveChanges();

        if (!ok) return Ok(new ApiResponse(404, "not found"));

        return Ok(new ApiOkResponse<AdminPlanDto>(_mapper.Map<AdminPlanDto>(plan)));
    }


    private async Task<bool> _addDrinks(List<int> drinkIds, AdminPlan plan)
    {
        foreach (var id in drinkIds)
        {
            var spec = new DrinkItemSpecification(id, plan.Id);
            var drink = await _unitOfWork.Repository<Drink>().GetByIdAsync(id);
           
            if (drink == null) return false;
           // plan.Drinks.Add(new DrinkItem{Drink = drink});
        }

        return true;
    }

    private async Task<bool> _addMeals(List<int> mealIds, AdminPlan plan)
    {
        foreach (var id in mealIds)
        {
            var meal = await _unitOfWork.Repository<Meal>().GetByIdAsync(id);
          
            if (meal == null) return false ;
            
            if (plan.Meals.Any(x => x.MealId == meal.Id)) continue;
            plan.Meals.Add(new AdminSelectedMeal{MealId = meal.Id});
        }
        return true;
    }
}