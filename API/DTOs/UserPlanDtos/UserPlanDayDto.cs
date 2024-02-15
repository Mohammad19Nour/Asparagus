using AsparagusN.DTOs.DrinksDtos;
using AsparagusN.Enums;

namespace AsparagusN.DTOs.UserPlanDtos;

public class UserPlanDayDto
{
    public int Id { get; set; }
    public DateTime Day { get; set; }
    public ICollection<UserSelectedDrinkDto> SelectedDrinks { get; set; }
    public ICollection<UserSelectedExtraOptionDto> SelectedExtraOptions { get; set; }
    public ICollection<UserSelectedMealDto> SelectedMeals { get; set; } = new List<UserSelectedMealDto>();
    public string DayOrderStatus { get; set; }
}