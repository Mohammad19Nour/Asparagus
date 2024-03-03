using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Meal;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.DTOs;
using AsparagusN.DTOs.AdminPlanDtos;
using AsparagusN.DTOs.DrinksDtos;
using AsparagusN.DTOs.ExtraOptionDtos;
using AsparagusN.DTOs.MealDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Helpers;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AsparagusN.Specifications.AdminPlanSpecifications;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Controllers.Dashboard.PlanControllers;

public partial class PlanController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PlanController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<PlanDetailsDto>> GetPlan(PlanTypeEnum planType)
    {
        var plan = await _unitOfWork.Repository<PlanType>().GetQueryable()
            .Where(x => x.PlanTypeE == planType)
            .FirstOrDefaultAsync();

        if (plan == null) return Ok(new ApiResponse(404, "Plan not found"));
        var result = new PlanDetailsDto();

        result.Points = plan.Points;
        result.PlanType = Enum.GetName(typeof(PlanTypeEnum), planType);

        var specDays = new AdminPlanSpecification(planType);

        var days = await _unitOfWork.Repository<AdminPlanDay>().ListWithSpecAsync(specDays);
        result.Drinks = await _getDrinks(planType);
        result.ExtraOptionDtos = await _getExtraOptions(planType, null);
        result.Snacks = await _getSnacks(planType);
        result.Days = _mapper.Map<List<AdminPlanDayDto>>(days);
        if (PlanTypeEnum.CustomMealPlan == planType)
        {
            int c = 0;
            foreach (var day in result.Days)
            {
                day.Meals = await _getAdminSelectedMeals(days[c]);
                c++;
            }
        }


        var response = new ApiOkResponse<PlanDetailsDto>(result);
        return Ok(response);
    }


    [HttpGet("day")]
    public async Task<ActionResult<AdminPlanDayDto>> GetPlanDay([FromQuery] int dayId)
    {
        var spec = new AdminPlanSpecification(dayId);

        var plan = await _unitOfWork.Repository<AdminPlanDay>().GetEntityWithSpec(spec);

        if (plan == null) return Ok(new ApiResponse(404, messageEN: "Plan not found"));
        var res = _mapper.Map<AdminPlanDayDto>(plan);

        if (plan.PlanType == PlanTypeEnum.CustomMealPlan)
            res.Meals = await _getAdminSelectedMeals(plan);

        return Ok(new ApiOkResponse<AdminPlanDayDto>(res));
    }

    private async Task<List<MealWithIngredientsDto>> _getAdminSelectedMeals(AdminPlanDay planDay)
    {
        var meals = await _unitOfWork.Repository<Meal>().ListAllAsync();
        meals = meals.Where(meal => planDay.Meals.All(d => d.MealId != meal.Id) && meal.IsMealPlan).ToList();

        return _mapper.Map<List<MealWithIngredientsDto>>(meals);

    }

    [HttpPut("updatePoint")]
    public async Task<ActionResult<AdminPlanDayDto>> UpdatePlanDayPoints([FromQuery] PlanTypeEnum planType,
        [FromQuery] int updatedPoints)
    {
        if (updatedPoints < 0) return Ok(new ApiResponse(400, messageEN: "Points should be positive"));

        var plan = await _unitOfWork.Repository<PlanType>().GetQueryable().Where(x => x.PlanTypeE == planType)
            .FirstOrDefaultAsync();

        if (plan == null) return Ok(new ApiResponse(404, messageEN: "Plan not found"));

        plan.Points = updatedPoints;

        _unitOfWork.Repository<PlanType>().Update(plan);
        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<PlanType>(_mapper.Map<PlanType>(plan)));
        return Ok(new ApiResponse(400, messageEN: "Failed to update"));
    }

    [HttpGet("plan-menu")]
    public async Task<ActionResult<List<MealWithIngredientsDto>>> GetMenuMealPlan()
    {
        var spec = new MealWithIngredientsAdnAllergiesSpecification(includeIngredients: true, mealPlansOnly: true);
        var d = await _unitOfWork.Repository<Meal>().ListWithSpecAsync(spec);

        return Ok(new ApiOkResponse<List<MealWithIngredientsDto>>(_mapper.Map<List<MealWithIngredientsDto>>(d)));
    }

    private async Task<bool> _addExtraOption(List<int> optionIds, PlanTypeEnum typeEnum)
    {
        var alreadySelectedExtras = await _unitOfWork.Repository<AdminSelectedExtraOption>().ListAllAsync();
        alreadySelectedExtras = alreadySelectedExtras.Where(x => x.PlanTypeEnum == typeEnum).ToList();

        foreach (var id in optionIds)
        {
            var item = alreadySelectedExtras.FirstOrDefault(x => x.ExtraOptionId == id);

            if (item != null) continue;

            var extra = await _unitOfWork.Repository<ExtraOption>().GetByIdAsync(id);
            _unitOfWork.Repository<AdminSelectedExtraOption>().Add(new AdminSelectedExtraOption
            {
                ExtraOption = extra,
                PlanTypeEnum = typeEnum
            });
        }

        return true;
    }


    private async Task<bool> _addDrinks(List<int> drinkIds, PlanTypeEnum typeEnum)
    {
        foreach (var id in drinkIds)
        {
            var spec = new AdminSelectedDrinksSpecification(typeEnum, id: id);
            var item = await _unitOfWork.Repository<AdminSelectedDrink>().GetEntityWithSpec(spec);
            if (item != null) continue;

            var drink = await _unitOfWork.Repository<Drink>().GetByIdAsync(id);
            _unitOfWork.Repository<AdminSelectedDrink>().Add(new AdminSelectedDrink
            {
                Drink = drink,
                PlanTypeEnum = typeEnum
            });
        }

        return true;
    }

    private async Task<(bool ok, string message)> _check(List<int> ids, bool isDrink)
    {
        foreach (var id in ids)
        {
            if (isDrink)
            {
                var drink = await _unitOfWork.Repository<Drink>().GetByIdAsync(id);
                if (drink == null) return (false, $"drink with id = {id} not exist");
            }
            else
            {
                var extra = await _unitOfWork.Repository<ExtraOption>().GetByIdAsync(id);
                if (extra == null) return (false, $"extra with id = {id} not exist");
            }
        }

        return (true, "ok");
    }

    private async Task<List<ExtraOptionDto>> _getExtraOptions(PlanTypeEnum planTypeEnum, ExtraOptionType? optionType)
    {
        if (planTypeEnum == PlanTypeEnum.CustomMealPlan)
        {
            var ext = await _unitOfWork.Repository<ExtraOption>().ListAllAsync();
            return _mapper.Map<List<ExtraOptionDto>>(ext);
        }

        var spec = new AdminSelectedExtrasAndSaladsSpecification(planTypeEnum);
        if (optionType != null)
            spec = new AdminSelectedExtrasAndSaladsSpecification(optionType.Value, planTypeEnum);
        var drinks = await _unitOfWork.Repository<AdminSelectedExtraOption>()
            .ListWithSpecAsync(spec);
        return _mapper.Map<List<ExtraOptionDto>>(drinks);
    }

    private async Task<List<DrinkDto>> _getDrinks(PlanTypeEnum planTypeEnum)
    {
        if (planTypeEnum == PlanTypeEnum.CustomMealPlan)
        {
            var drks = await _unitOfWork.Repository<Drink>().ListAllAsync();
            return _mapper.Map<List<DrinkDto>>(drks);
        }

        var spec = new AdminSelectedDrinksSpecification(planTypeEnum);
        var drinks = await _unitOfWork.Repository<AdminSelectedDrink>()
            .ListWithSpecAsync(spec);

        var drinksDto = _mapper.Map<List<DrinkDto>>(drinks);
        return drinksDto;
    }

    private async Task<List<SnackDto>> _getSnacks(PlanTypeEnum planTypeEnum)
    {
        if (planTypeEnum == PlanTypeEnum.CustomMealPlan)
        {
            var sp = new SnackMealsSpecification();
            var snks = await _unitOfWork.Repository<Meal>().ListWithSpecAsync(sp);
            return _mapper.Map<List<SnackDto>>(snks);
        }

        var spec = new AdminSelectedSnacksSpecification(planTypeEnum);
        var drinks = await _unitOfWork.Repository<AdminSelectedSnack>()
            .ListWithSpecAsync(spec);

        var snacksDto = _mapper.Map<List<SnackDto>>(drinks);
        return snacksDto;
    }
}