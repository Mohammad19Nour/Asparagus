using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.DTOs;
using AsparagusN.DTOs.AdditionDtos;
using AsparagusN.DTOs.AdminPlanDtos;
using AsparagusN.DTOs.DrinksDtos;
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

    public PlanController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet("{id:int}/days")]
    public async Task<ActionResult> GetAllPlans(int id)
    {
        var plans = await _unitOfWork.Repository<AdminPlan>().ListAllAsync();
        var groupedPlans = plans.GroupBy(plan => plan.PlanType)
            .ToDictionary(g => g.Key,
                g => g.Select(x => new PlanDayDto { Id = x.Id, Day = x.AvailableDate, Points = x.Points }).ToList());


        var response = new ApiOkResponse<Dictionary<PlanType, List<PlanDayDto>>>(groupedPlans);
        return Ok(response);
    }


    [HttpGet("day")]
    public async Task<ActionResult<AdminPlanDto>> GetPlanDay([FromQuery] int dayId)
    {
        var spec = new AdminPlanSpecification(dayId);

        var plan = await _unitOfWork.Repository<AdminPlan>().GetEntityWithSpec(spec);

        if (plan == null) return Ok(new ApiResponse(404, messageEN: "Plan not found"));

        // var response = new ApiOkResponse<Dictionary<MealPlanType, List<PlanDayDto>>>(groupedPlans);
        return Ok(new ApiOkResponse<AdminPlanDto>(_mapper.Map<AdminPlanDto>(plan)));
    }

    [HttpPut("day")]
    public async Task<ActionResult<AdminPlanDto>> UpdatePlanDayPoints([FromQuery] int dayId,
        [FromQuery] int updatedPoints)
    {
        if (updatedPoints < 0) return Ok(new ApiResponse(400, messageEN: "Points should be positive"));

        var plan = await _unitOfWork.Repository<AdminPlan>().GetByIdAsync(dayId);

        if (plan == null) return Ok(new ApiResponse(404, messageEN: "Plan not found"));

        plan.Points = updatedPoints;
        _unitOfWork.Repository<AdminPlan>().Update(plan);
        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<AdminPlanDto>(_mapper.Map<AdminPlanDto>(plan)));
        return Ok(new ApiResponse(400, messageEN: "Failed to update"));
    }

    [HttpGet("plan-menu")]
    public async Task<ActionResult<List<MealWithoutIngredientsDto>>> GetMenuMealPlan()
    {
        var spec = new MealWithIngredientsAdnAllergiesSpecification(includeIngredients: true, mealPlansOnly: true);
        var d = await _unitOfWork.Repository<Meal>().ListWithSpecAsync(spec);

        return Ok(new ApiOkResponse<List<MealWithoutIngredientsDto>>(_mapper.Map<List<MealWithoutIngredientsDto>>(d)));
    }

    [HttpPost("addMeals/{dayId:int}")]
    public async Task<ActionResult<AdminPlanDto>> AddMeal(NewAdminPlanDto newAdminPlanDto, int dayId)
    {
        var ok = true;

        var spec = new AdminPlanSpecification(dayId);

        var plan = await _unitOfWork.Repository<AdminPlan>().GetEntityWithSpec(spec);

        if (plan == null) return Ok(new ApiResponse(404, messageEN: "Plan not found"));


        ok = await _addMeals(newAdminPlanDto.MealIds, plan);

        _unitOfWork.Repository<AdminPlan>().Update(plan);
        await _unitOfWork.SaveChanges();

        if (!ok) return Ok(new ApiResponse(404, messageEN: "not found"));

        return Ok(new ApiOkResponse<AdminPlanDto>(_mapper.Map<AdminPlanDto>(plan)));
    }

    [HttpDelete("deleteMeal/{dayId:int},{mealId:int}")]
    public async Task<ActionResult<AdminPlanDto>> DeleteItems(int dayId, int mealId)
    {
        var spec = new AdminPlanSpecification(dayId);

        var plan = await _unitOfWork.Repository<AdminPlan>().GetEntityWithSpec(spec);

        if (plan == null) return Ok(new ApiResponse(404, messageEN: "Plan not found"));

        var meal = plan.Meals.FirstOrDefault(x => x.MealId == mealId);

        if (meal == null)
            return Ok(new ApiResponse(404, "meal not found"));
        plan.Meals.Remove(meal);

        _unitOfWork.Repository<AdminPlan>().Update(plan);
        await _unitOfWork.SaveChanges();

        return Ok(new ApiOkResponse<AdminPlanDto>(_mapper.Map<AdminPlanDto>(plan)));
    }

    [HttpPost("addDrink")]
    public async Task<ActionResult> AddExtra([FromBody] List<int> ids, PlanType planType)
    {
        if (planType == PlanType.CustomMealPlan) return Ok(new ApiResponse(404, "Plan type not found"));

        var ok = await _check(ids, true);
        if (!ok)
        {
            return Ok(new ApiResponse(404, "Drink option not found"));
        }

        await _addDrinks(ids, planType);
        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, "Failed to add drink"));
    }

    [HttpGet("drinks")]
    public async Task<ActionResult<List<DrinkDto>>> GetDrinks(PlanType planType)
    {
        var spec = new AdminSelectedDrinksSpecification(planType);
        var drinks = await _unitOfWork.Repository<AdminSelectedDrink>()
            .ListWithSpecAsync(spec);

        return Ok(new ApiOkResponse<List<DrinkDto>>(_mapper.Map<List<DrinkDto>>(drinks)));
    }

    [HttpDelete("deleteDrink/{id:int}")]
    public async Task<ActionResult> DeleteDrink(int id)
    {
        var drink = await _unitOfWork.Repository<AdminSelectedDrink>().GetByIdAsync(id);

        if (drink == null)
            return Ok(new ApiResponse(404, "Drink not found"));

        _unitOfWork.Repository<AdminSelectedDrink>().Delete(drink);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, "Failed to delete drink"));
    }

    [HttpPost("addExtra")]
    public async Task<ActionResult> AddDrink([FromBody] List<int> ids, PlanType planType)
    {
        if (planType == PlanType.CustomMealPlan) return Ok(new ApiResponse(404, "Plan type not found"));

        var ok = await _check(ids, false);
        if (!ok)
        {
            return Ok(new ApiResponse(404, "Extra not found"));
        }

        await _addExtraOption(ids, planType);
        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, "Failed to add extras"));
    }

    [HttpGet("extra")]
    public async Task<ActionResult<List<ExtraOptionDto>>> GetExtras(PlanType planType)
    {
        var spec = new AdminSelectedExtraOptionSpecification(planType);
        var drinks = await _unitOfWork.Repository<AdminSelectedExtraOption>()
            .ListWithSpecAsync(spec);

        return Ok(new ApiOkResponse<List<ExtraOptionDto>>(_mapper.Map<List<ExtraOptionDto>>(drinks)));
    }

    [HttpDelete("deleteextra/{id:int}")]
    public async Task<ActionResult> DeleteExtra(int id)
    {
        var extra = await _unitOfWork.Repository<AdminSelectedExtraOption>().GetByIdAsync(id);

        if (extra == null)
            return Ok(new ApiResponse(404, "Extra not found"));

        _unitOfWork.Repository<AdminSelectedExtraOption>().Delete(extra);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, "Failed to delete extra"));
    }


    private async Task<bool> _addExtraOption(List<int> optionIds, PlanType type)
    {
        foreach (var id in optionIds)
        {
            var spec = new AdminSelectedExtraOptionSpecification(type, id: id);
            var item = await _unitOfWork.Repository<AdminSelectedExtraOption>().GetEntityWithSpec(spec);
            if (item != null) continue;

            var extra = await _unitOfWork.Repository<ExtraOption>().GetByIdAsync(id);
            _unitOfWork.Repository<AdminSelectedExtraOption>().Add(new AdminSelectedExtraOption
            {
                ExtraOption = extra,
                PlanType = type
            });
        }

        return true;
    }

    private async Task<bool> _addMeals(List<int> mealIds, AdminPlan plan)
    {
        foreach (var id in mealIds)
        {
            var meal = await _unitOfWork.Repository<Meal>().GetByIdAsync(id);

            if (meal is not { IsMealPlan: true }) return false;

            if (plan.Meals.Any(x => x.MealId == meal.Id)) continue;
            plan.Meals.Add(new AdminSelectedMeal { MealId = meal.Id });
        }

        return true;
    }


    private async Task<bool> _addDrinks(List<int> drinkIds, PlanType type)
    {
        foreach (var id in drinkIds)
        {
            var spec = new AdminSelectedDrinksSpecification(type, id: id);
            var item = await _unitOfWork.Repository<AdminSelectedDrink>().GetEntityWithSpec(spec);
            if (item != null) continue;

            var drink = await _unitOfWork.Repository<Drink>().GetByIdAsync(id);
            _unitOfWork.Repository<AdminSelectedDrink>().Add(new AdminSelectedDrink
            {
                Drink = drink,
                PlanType = type
            });
        }

        return true;
    }

    private async Task<bool> _check(List<int> ids, bool isDrink)
    {
        foreach (var id in ids)
        {
            if (isDrink)
            {
                var drink = await _unitOfWork.Repository<Drink>().GetByIdAsync(id);
                if (drink == null) return false;
            }
            else
            {
                var extra = await _unitOfWork.Repository<ExtraOption>().GetByIdAsync(id);
                if (extra == null) return false;
            }
        }

        return true;
    }
}