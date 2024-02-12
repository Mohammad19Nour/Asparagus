using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
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
using Microsoft.EntityFrameworkCore;

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

    [HttpGet]
    public async Task<ActionResult<PlanDetailsDto>> GetPlan(PlanTypeEnum planType)
    {
        var plan = await _unitOfWork.Repository<PlanType>().GetQueryable().Where(x => x.PlanTypeE == planType)
            .FirstOrDefaultAsync();

        if (plan == null) return Ok(new ApiResponse(404, "Plan not found"));
        var result = new PlanDetailsDto();

        result.Points = plan.Points;
        result.PlanType = Enum.GetName(typeof(PlanTypeEnum), planType);

        var specDays = new AdminPlanSpecification(planType);

        var days = await _unitOfWork.Repository<AdminPlanDay>().ListWithSpecAsync(specDays);
        result.Drinks = await _getDrinks(planType);
        result.ExtraOptionDtos = await _getExtraOptions(planType);
        result.Days = _mapper.Map<List<AdminPlanDayDto>>(days);


        var response = new ApiOkResponse<PlanDetailsDto>(result);
        return Ok(response);
    }

    [HttpGet("day")]
    public async Task<ActionResult<AdminPlanDayDto>> GetPlanDay([FromQuery] int dayId)
    {
        var spec = new AdminPlanSpecification(dayId);

        var plan = await _unitOfWork.Repository<AdminPlanDay>().GetEntityWithSpec(spec);

        if (plan == null) return Ok(new ApiResponse(404, messageEN: "Plan not found"));

        return Ok(new ApiOkResponse<AdminPlanDayDto>(_mapper.Map<AdminPlanDayDto>(plan)));
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
    public async Task<ActionResult<List<MealWithoutIngredientsDto>>> GetMenuMealPlan()
    {
        var spec = new MealWithIngredientsAdnAllergiesSpecification(includeIngredients: true, mealPlansOnly: true);
        var d = await _unitOfWork.Repository<Meal>().ListWithSpecAsync(spec);

        return Ok(new ApiOkResponse<List<MealWithoutIngredientsDto>>(_mapper.Map<List<MealWithoutIngredientsDto>>(d)));
    }

    [HttpPost("addMeals/{dayId:int}")]
    public async Task<ActionResult<AdminPlanDayDto>> AddMeal(NewAdminPlanDto newAdminPlanDto, int dayId)
    {
        var ok = true;

        var spec = new AdminPlanSpecification(dayId);

        var plan = await _unitOfWork.Repository<AdminPlanDay>().GetEntityWithSpec(spec);

        if (plan == null) return Ok(new ApiResponse(404, messageEN: "Plan not found"));

        ok = await _addMeals(newAdminPlanDto.MealIds, plan);

        _unitOfWork.Repository<AdminPlanDay>().Update(plan);
        await _unitOfWork.SaveChanges();

        if (!ok) return Ok(new ApiResponse(404, messageEN: "not found"));

        return Ok(new ApiOkResponse<AdminPlanDayDto>(_mapper.Map<AdminPlanDayDto>(plan)));
    }

    [HttpDelete("deleteMeal/{dayId:int},{mealId:int}")]
    public async Task<ActionResult<AdminPlanDayDto>> DeleteItems(int dayId, int mealId)
    {
        var spec = new AdminPlanSpecification(dayId);

        var plan = await _unitOfWork.Repository<AdminPlanDay>().GetEntityWithSpec(spec);

        if (plan == null) return Ok(new ApiResponse(404, messageEN: "Plan not found"));

        var meal = plan.Meals.FirstOrDefault(x => x.MealId == mealId);

        if (meal == null)
            return Ok(new ApiResponse(404, "meal not found"));
        plan.Meals.Remove(meal);

        _unitOfWork.Repository<AdminPlanDay>().Update(plan);
        await _unitOfWork.SaveChanges();

        return Ok(new ApiOkResponse<AdminPlanDayDto>(_mapper.Map<AdminPlanDayDto>(plan)));
    }

    [HttpPost("addDrink")]
    public async Task<ActionResult> AddDrink(DrinkIdsDto ids, PlanTypeEnum planTypeEnum)
    {
        if (planTypeEnum == PlanTypeEnum.CustomMealPlan) return Ok(new ApiResponse(404, "Plan type not found"));

        var (ok, message) = await _check(ids.DrinkIds, true);
        if (!ok)
        {
            return Ok(new ApiResponse(404, message));
        }

        await _addDrinks(ids.DrinkIds, planTypeEnum);
        if (_unitOfWork.HasChanges())
        {
            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiResponse(200));
            return Ok(new ApiResponse(400, "Failed to add drink"));
        }

        return Ok(new ApiResponse(400, "already exist"));
    }

    [HttpGet("drinks")]
    public async Task<ActionResult<List<DrinkDto>>> GetDrinks(PlanTypeEnum planTypeEnum)
    {
        return Ok(new ApiOkResponse<List<DrinkDto>>(await _getDrinks(planTypeEnum)));
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
    public async Task<ActionResult> AddExtra(ExtraIdsDto ids, PlanTypeEnum planTypeEnum)
    {
        if (planTypeEnum == PlanTypeEnum.CustomMealPlan) return Ok(new ApiResponse(404, "Plan type not found"));

        var (ok, message) = await _check(ids.ExtraIds, false);
        if (!ok)
        {
            return Ok(new ApiResponse(404, message));
        }

        await _addExtraOption(ids.ExtraIds, planTypeEnum);
        if (_unitOfWork.HasChanges())
        {
            if (await _unitOfWork.SaveChanges())
                return Ok(new ApiResponse(200));
            return Ok(new ApiResponse(400, "Failed to add extras"));
        }

        return Ok(new ApiResponse(400, "already exist"));
    }

    [HttpGet("extra")]
    public async Task<ActionResult<List<ExtraOptionDto>>> GetExtras(PlanTypeEnum planTypeEnum)
    {
        return Ok(new ApiOkResponse<List<ExtraOptionDto>>(await _getExtraOptions(planTypeEnum)));
    }


    [HttpDelete("deleteExtra/{id:int}")]
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

    [HttpPost("addSnack")]
    public async Task<ActionResult> AddSnack(SnackIdsDto ids, PlanTypeEnum planTypeEnum)
    {
        if (planTypeEnum == PlanTypeEnum.CustomMealPlan)
            return Ok(new ApiResponse(400, "Can't add to this plan type"));

        var selectedSnackIds =
            (await _unitOfWork.Repository<AdminSelectedSnack>().ListAllAsync()).Select(x => x.SnackId);
        ids.SnackIds = ids.SnackIds.Where(x => !selectedSnackIds.Contains(x)).ToList();

        var spec = new SnackMealsSpecification();
        var snacks = await _unitOfWork.Repository<Meal>().ListWithSpecAsync(spec);
        var snackIds = snacks.Select(x => x.Id).ToList();
        var notExistSnackIds = ids.SnackIds.Where(x => !snackIds.Contains(x)).ToList();

        if (notExistSnackIds.Count > 0)
            return Ok(new ApiResponse(404, $"snacks with ids: {notExistSnackIds.ToList()} not exist"));

        if (ids.SnackIds.Count == 0) return Ok(new ApiResponse(400, "0 snacks added. maybe it already exist"));

        var res = new List<AdminSelectedSnack>();
        foreach (var toAdd in ids.SnackIds)
        {
            var snk = snacks.First(x => x.Id == toAdd);
            var a = new AdminSelectedSnack { SnackId = toAdd, PlanTypeEnum = planTypeEnum, Snack = snk };
            _unitOfWork.Repository<AdminSelectedSnack>().Add(a);

            res.Add(a);
        }

        await _unitOfWork.SaveChanges();

        return Ok(new ApiOkResponse<List<SnackDto>>(_mapper.Map<List<SnackDto>>(res)));
    }

    [HttpGet("snacks")]
    public async Task<ActionResult<List<SnackDto>>> GetSnacks(PlanTypeEnum planTypeEnum)
    {
        var snacks = (await _unitOfWork.Repository<AdminSelectedSnack>().GetQueryable()
            .Where(x => x.PlanTypeEnum == planTypeEnum).Include(x => x.Snack)
            .ToListAsync());

        return Ok(new ApiOkResponse<List<SnackDto>>(_mapper.Map<List<SnackDto>>(snacks)));
    }

    [HttpDelete("deleteSnack/{id:int}")]
    public async Task<ActionResult> DeleteSnack(int id)
    {
        var snack = await _unitOfWork.Repository<AdminSelectedSnack>().GetByIdAsync(id);
        if (snack == null) return Ok(new ApiResponse(404, "Snack not found"));

        _unitOfWork.Repository<AdminSelectedSnack>().Delete(snack);

        if (await _unitOfWork.SaveChanges()) return Ok(new ApiResponse(200, "deleted"));

        return Ok(new ApiResponse(400, "Failed to delete snack"));
    }


    private async Task<bool> _addExtraOption(List<int> optionIds, PlanTypeEnum typeEnum)
    {
        foreach (var id in optionIds)
        {
            var spec = new AdminSelectedExtraOptionSpecification(typeEnum, id: id);
            var item = await _unitOfWork.Repository<AdminSelectedExtraOption>().GetEntityWithSpec(spec);
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

    private async Task<bool> _addMeals(List<int> mealIds, AdminPlanDay planDay)
    {
        foreach (var id in mealIds)
        {
            var meal = await _unitOfWork.Repository<Meal>().GetByIdAsync(id);

            Console.WriteLine(id);
            if (meal is not { IsMealPlan: true }) return false;

            if (planDay.Meals.Any(x => x.MealId == meal.Id)) continue;
            planDay.Meals.Add(new AdminSelectedMeal { MealId = meal.Id });
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

    private async Task<List<ExtraOptionDto>> _getExtraOptions(PlanTypeEnum planTypeEnum)
    {
        var spec = new AdminSelectedExtraOptionSpecification(planTypeEnum);
        var drinks = await _unitOfWork.Repository<AdminSelectedExtraOption>()
            .ListWithSpecAsync(spec);
        return _mapper.Map<List<ExtraOptionDto>>(drinks);
    }

    private async Task<List<DrinkDto>> _getDrinks(PlanTypeEnum planTypeEnum)
    {
        var spec = new AdminSelectedDrinksSpecification(planTypeEnum);
        var drinks = await _unitOfWork.Repository<AdminSelectedDrink>()
            .ListWithSpecAsync(spec);

        var drinksDto = _mapper.Map<List<DrinkDto>>(drinks);
        return drinksDto;
    }
}