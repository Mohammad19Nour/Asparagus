using AsparagusN.Data.Entities;
using AsparagusN.DTOs;
using AsparagusN.DTOs.CategoryDtos;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers;

public class CategoryController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpPost("add-category")]
    public async Task<ActionResult<CategoryDto>> AddCategory(NewCategoryDto dto)
    {
        var category = _mapper.Map<Category>(dto);
        _unitOfWork.Repository<Category>().Add(category);

        if (await _unitOfWork.SaveChanges()) return 
            Ok(new ApiOkResponse<CategoryDto>(_mapper.Map<CategoryDto>(category)));

        return Ok(new ApiResponse(400, messageEN: "Failed to save category"));
    }
    
    [HttpGet("categories")]
    public async Task<ActionResult<ICollection<CategoryDto>>> GetAllCategory()
    {
        var res = await _unitOfWork.Repository<Category>().ListAllAsync();
        return Ok(new ApiOkResponse<ICollection<CategoryDto>>(_mapper.Map<ICollection<CategoryDto>>(res)));
    }

    [HttpGet("{categoryId:int}")]
    public async Task<ActionResult<CategoryDto>> GetCategoryById(int categoryId)
    {
        var spec = new CategoryWithMealSpecification(categoryId);
        var category = await _unitOfWork.Repository<Category>().GetEntityWithSpec(spec);

        return Ok(category == null ? new ApiResponse(404, messageEN:"category not found") : new ApiOkResponse<CategoryDto>(_mapper.Map<CategoryDto>(category)));
    }

    [HttpPut("update/{categoryId:int}")]
    public async Task<ActionResult<CategoryDto>> UpdateCategory( int categoryId,[FromBody] UpdateCategoryDto dto)
    {
        var category = await _unitOfWork.Repository<Category>().GetByIdAsync(categoryId);

        if (category == null) return Ok(new ApiResponse(404, messageEN:"category not found"));

        _mapper.Map(dto,category);
        _unitOfWork.Repository<Category>().Update(category);

        if (await _unitOfWork.SaveChanges()) return Ok(new ApiOkResponse<CategoryDto>
            (_mapper.Map<CategoryDto>(category)));

        return Ok(new ApiResponse(400, messageEN:"Failed to update category"));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);

        if (category == null) return Ok(new ApiResponse(404, messageEN:"category not found"));

        _unitOfWork.Repository<Category>().Delete(category);

        if (await _unitOfWork.SaveChanges()) return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, messageEN:"Failed to delete category"));
    }
    
}