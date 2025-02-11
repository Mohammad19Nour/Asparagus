﻿using AsparagusN.Data.Entities.Meal;
using AsparagusN.DTOs.AllergyDtos;
using AsparagusN.DTOs.IngredientDtos;
using AutoMapper.Configuration.Annotations;
using Newtonsoft.Json;

namespace AsparagusN.DTOs.MealDtos;

public class MealWithIngredientsDto
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
    public decimal PricePerCarb;
    public decimal PricePerProtein;
    public int CategoryId { get; set; }
    public ICollection<AllergyDto> Allergies { get; set; }
    public ICollection<MealIngredientDetailsDto>Ingredients { get; set; }
    public IngredientDto? SelectedCarb { get; set; }
}