using AsparagusN.DTOs.AllergyDtos;
using AutoMapper.Configuration.Annotations;
using Newtonsoft.Json;

namespace AsparagusN.DTOs.MealDtos;

public class MealWithIngredientsDto : MealWithoutIngredientsDto
{  
    public IEnumerable<MealIngredientDetailsDto>Ingredients { get; set; }
}