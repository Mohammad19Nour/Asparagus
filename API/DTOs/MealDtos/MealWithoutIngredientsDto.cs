using AsparagusN.DTOs.AllergyDtos;
using AutoMapper.Configuration.Annotations;
using Newtonsoft.Json;

namespace AsparagusN.DTOs.MealDtos;

public class MealWithoutIngredientsDto
{
    public int Id { get; set; }
    public string NameEN { get; set; }
    public string NameAR { get; set; }
    public string DescriptionEN { get; set; }
    public string DescriptionAR { get; set; }
    public decimal Price { get; set; }
    public int Points { get; set; }
    public string PictureUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Protein;
    public decimal Carbs;
    public decimal Fats;
    public decimal Fibers;
    public decimal Calories;
    public int CategoryId { get; set; }
    public IEnumerable<AllergyDto> Allergies { get; set; }
   
}