using AsparagusN.DTOs.MealDtos;

namespace AsparagusN.DTOs.CategoryDtos;

public class CategoryDto : NewCategoryDto
{
   public int Id { get; set; }
   public List<MealWithIngredientsDto> Meals { get; set; }
}