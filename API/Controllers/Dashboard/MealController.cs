using AsparagusN.DTOs.MealDtos;
using AsparagusN.Entities;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
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
    public async Task<ActionResult<List<MealDto>>> GetMealById(int id)
    {
        var spec = new MealWithIngredientsAdnAllergiesSpecification(id);
        var d = await _unitOfWork.Repository<Meal>().GetEntityWithSpec(spec);

        if (d == null) return Ok(new ApiResponse(404,"Meal not found"));

        return Ok(_mapper.Map<MealDto>(d));
    }

    [HttpGet("{branchId:int}/menu")]
    public async Task<ActionResult<List<MealDto>>> GetMenu(int branchId)
    {
        var spec = new MealWithIngredientsAdnAllergiesSpecification(branchId:branchId);
        var d = await _unitOfWork.Repository<Meal>().ListWithSpecAsync(spec);

        return Ok(_mapper.Map<List<MealDto>>(d));
    }

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

        _unitOfWork.Repository<Meal>().Update(meal);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<MealDto>(_mapper.Map<MealDto>(meal)));
        return Ok(new ApiResponse(400, "baaad"));
    }


    [HttpPost("add")]
    public async Task<ActionResult<MealDto>> AddMeal([FromForm] NewMealDto newMealDto)
    {
        var branch = await _unitOfWork.Repository<Branch>().GetByIdAsync(newMealDto.BranchId);

        if (branch == null) return Ok(new ApiResponse(404, "Branch not found"));
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
        meal.Branch = branch;

        var res = await _mediaService.AddPhotoAsync(newMealDto.ImageFile);
        if (!res.Success) return Ok(new ApiResponse(400, res.Message));

        meal.PictureUrl = res.Url;

        _unitOfWork.Repository<Meal>().Add(meal);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<MealDto>(_mapper.Map<MealDto>(meal)));
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
        foreach (var ingr in ingredientIds)
        {
            var item = await _unitOfWork.Repository<Ingredient>().GetByIdAsync(ingr.IngredientId);

            if (item == null)
                return false;
            meal.Ingredients.Add(new MealIngredient { Ingredient = item, Weight = ingr.Weight });
        }

        return true;
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var spec = new MealWithIngredientsAdnAllergiesSpecification(id);

        var meal = await _unitOfWork.Repository<Meal>().GetEntityWithSpec(spec);
        if (meal == null)
            return Ok(new ApiResponse(404,"meal not found"));
        _unitOfWork.Repository<Meal>().Delete(meal);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, "Failed to delete meal"));
    }
}