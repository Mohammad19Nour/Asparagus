using AsparagusN.Data.Entities.Meal;
using AsparagusN.DTOs.IngredientDtos;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers.Dashboard;

public class IngredientController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public IngredientController(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<IngredientDto>>> GetIngredients()
    {
        try
        {
            var spec = new IngredientNotDeletedSpecification();
            var ingredients = await _unitOfWork.Repository<Ingredient>().ListWithSpecAsync(spec);
            return Ok(new ApiOkResponse<ICollection<IngredientDto>>
                (_mapper.Map<ICollection<IngredientDto>>(ingredients)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<IngredientDto>> GetIngredientById(int id)
    {
        try
        {
            var spec = new IngredientNotDeletedSpecification(id);
            var ingredient = await _unitOfWork.Repository<Ingredient>().GetEntityWithSpec(spec);
            return Ok(ingredient == null ? new ApiResponse(404, "Ingredient not found") : new ApiOkResponse<IngredientDto>(_mapper.Map<IngredientDto>(ingredient)));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost("add")]
    public async Task<ActionResult<IngredientDto>> AddNewIngredient(NewIngredientDto newIngredientDto)
    {
        try
        {
            var ingredient = _mapper.Map<Ingredient>(newIngredientDto);
            _unitOfWork.Repository<Ingredient>().Add(ingredient);

            if (await _unitOfWork.SaveChanges())
            {
                return Ok(new ApiOkResponse<IngredientDto>(_mapper.Map<IngredientDto>(ingredient)));
            }

            return Ok(new ApiResponse(400, "Failed to add ingredient"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<IngredientDto>> UpdateIngredient(int id, UpdateIngredientDto updateIngredientDto)
    {
        try
        {
            var ingredient = await _unitOfWork.Repository<Ingredient>().GetByIdAsync(id);

            if (ingredient == null || ingredient.IsDeleted) return Ok(new ApiResponse(404,"Ingredient not found"));

            _mapper.Map(updateIngredientDto,ingredient);
            
            _unitOfWork.Repository<Ingredient>().Update(ingredient);

            if (await _unitOfWork.SaveChanges())
                return
                    Ok(new ApiOkResponse<IngredientDto>(_mapper.Map<IngredientDto>(ingredient)));
            return Ok(new ApiResponse(400, "Failed to update ingredient"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteIngredient(int id)
    {
        try
        {
            var ingredient = await _unitOfWork.Repository<Ingredient>().GetByIdAsync(id);
            if (ingredient == null || ingredient.IsDeleted) return Ok(new ApiResponse(404, "Ingredient not found"));
         
            ingredient.IsDeleted = true;
            _unitOfWork.Repository<Ingredient>().Delete(ingredient);

            if (await _unitOfWork.SaveChanges()) return Ok();
            return Ok(new ApiResponse(400, "Failed to delete "));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}