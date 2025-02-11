﻿using AsparagusN.Data.Entities;
using AsparagusN.DTOs;
using AsparagusN.DTOs.CategoryDtos;
using AsparagusN.Enums;
using AsparagusN.Errors;
using AsparagusN.Interfaces;
using AsparagusN.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsparagusN.Controllers;

public class CategoryController : BaseApiController
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CategoryController> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _logger.LogInformation("Eeefe");
    }


    [Authorize(Roles = nameof(DashboardRoles.Category) + "," + nameof(Roles.Admin))]
    [HttpPost("add-category")]
    public async Task<ActionResult<CategoryDto>> AddCategory(NewCategoryDto dto)
    {
        var category = _mapper.Map<Category>(dto);
        _unitOfWork.Repository<Category>().Add(category);

        if (await _unitOfWork.SaveChanges())
            return
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
if (category == null)
       return Ok( new ApiResponse(404, messageEN: "category not found"));

Console.WriteLine(category.Meals.Count);
        category.Meals = category.Meals.Where(x => x.IsMainMenu).ToList();
        return Ok( new ApiOkResponse<CategoryDto>(_mapper.Map<CategoryDto>(category)));
    }

    [Authorize(Roles = nameof(DashboardRoles.Category) + "," + nameof(Roles.Admin))]
    [HttpPut("update/{categoryId:int}")]
    public async Task<ActionResult<CategoryDto>> UpdateCategory(int categoryId, [FromBody] UpdateCategoryDto dto)
    {
        var category = await _unitOfWork.Repository<Category>().GetByIdAsync(categoryId);

        if (category == null) return Ok(new ApiResponse(404, messageEN: "category not found"));

        _mapper.Map(dto, category);
        _unitOfWork.Repository<Category>().Update(category);

        if (await _unitOfWork.SaveChanges())
            return Ok(new ApiOkResponse<CategoryDto>
                (_mapper.Map<CategoryDto>(category)));

        return Ok(new ApiResponse(400, messageEN: "Failed to update category"));
    }

    [Authorize(Roles = nameof(DashboardRoles.Category) + "," + nameof(Roles.Admin))]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);

        if (category == null) return Ok(new ApiResponse(404, messageEN: "category not found"));

        _unitOfWork.Repository<Category>().Delete(category);

        if (await _unitOfWork.SaveChanges()) return Ok(new ApiResponse(200));
        return Ok(new ApiResponse(400, messageEN: "Failed to delete category"));
    }
}