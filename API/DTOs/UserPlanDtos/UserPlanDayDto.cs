﻿using AsparagusN.DTOs.AddressDtos;
using AsparagusN.DTOs.DrinksDtos;
using AsparagusN.Enums;

namespace AsparagusN.DTOs.UserPlanDtos;

public class UserPlanDayDto
{
    public int Id { get; set; }
    public DateTime Day { get; set; }
    public ICollection<UserSelectedDrinkDto> SelectedDrinks { get; set; }
    public ICollection<UserSelectedExtraOptionDto> SelectedExtraOptions { get; set; }
    public ICollection<UserSelectedMealDto> SelectedSnacks { get; set; }
    public ICollection<UserSelectedExtraOptionDto> SelectedSalads { get; set; }
    public ICollection<UserSelectedMealDto> SelectedMeals { get; set; } = new List<UserSelectedMealDto>();
    public string DayOrderStatus { get; set; }
    public int AdminDayId { get; set; }
    public bool IsHomeAddress { get; set; }

    public AddressDto DeliveryLocation { get; set; }
    public string DeliveryPeriod { get; set; }
    public decimal Fat { get; set; }
    public decimal Carb { get; set; }
    public decimal Protein { get; set; }
    public bool IsSubscriptionDay { get; set; } = true;

    public decimal Calories
    {
        get => Protein * 4 + Carb * 4 + Fat * 9;
    }
}