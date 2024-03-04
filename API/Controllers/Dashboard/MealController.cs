using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Meal;
using AsparagusN.DTOs;
using AsparagusN.DTOs.MealDtos;
using AsparagusN.DTOs.PackageDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Extensions;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AsparagusN.Specifications.UserSpecifications;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard;

public class MealController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediaService _mediaService;

    public MealController(IUnitOfWork unitOfWork, IMapper mapper, IMediaService mediaService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediaService = mediaService;
        
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MealWithIngredientsDto>> GetMealById(int id)
    {
        var spec = new MealWithIngredientsAdnAllergiesSpecification(id);
        var d = await _unitOfWork.Repository<Meal>().GetEntityWithSpec(spec);

        if (d == null) return Ok(new ApiResponse(404, "Meal not found"));

        return Ok(new ApiOkResponse<MealWithIngredientsDto>(_mapper.Map<MealWithIngredientsDto>(d)));
    }
    [HttpGet("menu")]
    public async Task<ActionResult<List<MealWithIngredientsDto>>> GetMenu()
    {
        var spec = new MealWithIngredientsAdnAllergiesSpecification();
        var d = (await _unitOfWork.Repository<Meal>().ListWithSpecAsync(spec)).ToList();

        return Ok(new ApiOkResponse<List<MealWithIngredientsDto>>(_mapper.Map<List<MealWithIngredientsDto>>(d)));
    }

    [HttpGet("snacks")]
    public async Task<ActionResult<IReadOnlyList<SnackDto>>> GetSnacks()
    {
        var spec = new SnackMealsSpecification();
        var meals = await _unitOfWork.Repository<Meal>().ListWithSpecAsync(spec);

        // Map each Meal entity to a SnackDto object
        var snackDtos = meals.Select(meal => _mapper.Map<SnackDto>(meal)).ToList();

        return Ok(new ApiOkResponse<IReadOnlyList<SnackDto>>(snackDtos));
    }
    [Authorize(Roles = nameof(DashboardRoles.Menu) + ","+nameof(Roles.Admin))]

    [HttpPut("update/{id:int}")]
    public async Task<ActionResult> Update(int id, [FromForm] UpdateMealDto updateMealDto)
    {
        var spec = new MealWithIngredientsAdnAllergiesSpecification(id);
        var meal = await _unitOfWork.Repository<Meal>().GetEntityWithSpec(spec);

        if (meal == null)
            return Ok(new ApiResponse(404, "Meal not found"));
        _mapper.Map(updateMealDto, meal);

        if (updateMealDto.Ingredients != null)
        {
            meal.Ingredients.Clear();
            bool ok = await _addIngredients(updateMealDto.Ingredients, meal);
            if (!ok)
                return Ok(new ApiResponse(404, "Ingredient not found"));
        }

        if (updateMealDto.Allergies != null)
        {
            meal.Allergies.Clear();
            bool ok = await _addAllergies(updateMealDto.Allergies, meal);

            if (!ok)
                return Ok(new ApiResponse(404, "Allergies not found"));
        }

        if (updateMealDto.ImageFile != null)
        {
            var res = await _mediaService.AddPhotoAsync(updateMealDto.ImageFile);
            if (!res.Success) return Ok(new ApiResponse(400, res.Message));

            meal.PictureUrl = res.Url;
        }

        if (updateMealDto.CategoryId != null)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(updateMealDto.CategoryId.Value);
            if (category == null) return Ok(new ApiResponse(404, "Category not found"));

            meal.Category = category;
        }

        _unitOfWork.Repository<Meal>().Update(meal);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<MealWithIngredientsDto>(_mapper.Map<MealWithIngredientsDto>(meal)));
        return Ok(new ApiResponse(400, "baaad"));
    }
    [Authorize(Roles = nameof(DashboardRoles.Menu) + ","+nameof(Roles.Admin))]


    [HttpPost("add")]
    public async Task<ActionResult<MealWithIngredientsDto>> AddMeal([FromForm] NewMealDto newMealDto)
    {
        var category = await _unitOfWork.Repository<Category>().GetByIdAsync(newMealDto.CategoryId);

        if (category == null) return Ok(new ApiResponse(404, "category not found"));


        if (newMealDto is { IsMealPlan: false, IsMainMenu: false })
        {
            return Ok(new ApiResponse(400, "You should specify meal plan or menu item"));
        }

        var meal = _mapper.Map<Meal>(newMealDto);


        bool ok = await _addIngredients(newMealDto.Ingredients, meal);

        if (!ok)
            return Ok(new ApiResponse(404, "Ingredient not found"));

        if (newMealDto.Allergies != null)
        {
            ok = await _addAllergies(newMealDto.Allergies, meal);

            if (!ok)
                return Ok(new ApiResponse(404, "Allergies not found"));
        }

        meal.Category = category;

        var res = await _mediaService.AddPhotoAsync(newMealDto.ImageFile);
        if (!res.Success) return Ok(new ApiResponse(400, res.Message));

        meal.PictureUrl = res.Url;

        _unitOfWork.Repository<Meal>().Add(meal);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<MealWithIngredientsDto>(_mapper.Map<MealWithIngredientsDto>(meal)));
        return Ok(new ApiResponse(400, "baaad"));
    }

    private async Task<bool> _addAllergies(List<int> allergyIds, Meal meal)
    {
        foreach (var id in allergyIds)
        {
            var item = await _unitOfWork.Repository<Allergy>().GetByIdAsync(id);

            if (item == null)
                return false;

            meal.Allergies.Add(new MealAllergy { Allergy = item });
        }

        return true;
    }

    private async Task<bool> _addIngredients(List<MealIngredientDto> ingredientIds, Meal meal)
    {
        var mg = new List<MealIngredient>();
        foreach (var ingr in ingredientIds)
        {
            var item = await _unitOfWork.Repository<Ingredient>().GetByIdAsync(ingr.IngredientId);

            if (item == null)
                return false;
            mg.Add(new MealIngredient { Ingredient = item, Weight = ingr.Weight });
        }

        meal.Ingredients = mg;
        return true;
    }
    [Authorize(Roles = nameof(DashboardRoles.Menu) + ","+nameof(Roles.Admin))]

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var spec = new MealWithIngredientsAdnAllergiesSpecification(id);

        var meal = await _unitOfWork.Repository<Meal>().GetEntityWithSpec(spec);
        if (meal == null)
            return Ok(new ApiResponse(404, "meal not found"));
        _unitOfWork.Repository<Meal>().Delete(meal);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, "Failed to delete meal"));
    }
}