using AsparagusN.Data.Entities.Meal;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AutoMapper;

namespace AsparagusN.Helpers;

public static class HelperFunctions
{
    public static DateTime WeekStartDay()
    {
        var todayDay = DateTime.Now.DayOfWeek;
        var startDay = DateTime.Now.Date;
        var add = -1;
        if (todayDay is DayOfWeek.Friday or DayOfWeek.Thursday)
            add = 1;

        while (startDay.DayOfWeek != DayOfWeek.Saturday)
        {
            startDay = startDay.AddDays(add);
        }

        return startDay;
    }

    public static DateTime WeekEndDay()
    {
        Console.WriteLine(WeekStartDay());
        Console.WriteLine(WeekStartDay().AddDays(6));
        return WeekStartDay().AddDays(6);
    }

    public static List<DateTime> getDatesOfCurrentWeek()
    {
        var result = new List<DateTime>();
        var startDay = DateTime.Now;

        while (startDay.DayOfWeek != DayOfWeek.Saturday)
        {
            startDay = startDay.AddDays(-1);
        }

        result.Add(startDay);

        while (startDay.DayOfWeek != DayOfWeek.Friday)
        {
            startDay = startDay.AddDays(1);
            result.Add(startDay);
        }

        return result;
    }
    
    public static void CalcNewPropertyForCarb(UserSelectedMeal meal, Ingredient newCarb)
    {
       // Console.WriteLine(meal.Carbs);
       // Console.WriteLine(meal.Fats);
       // Console.WriteLine(meal.Fibers);
       // Console.WriteLine(meal.Protein);
        meal.Carbs -= meal.ChangedCarb.Carb;
        meal.Fats -= meal.ChangedCarb.Fat;
        meal.Protein -= meal.ChangedCarb.Protein;
        meal.Fibers -= meal.ChangedCarb.Fiber;

       // Console.WriteLine("\n");
       // Console.WriteLine(meal.Carbs);
       // Console.WriteLine(meal.Fats);
       // Console.WriteLine(meal.Fibers);
        //Console.WriteLine(meal.Protein);
        
        meal.ChangedCarb.Carb = newCarb.Carb;
        meal.ChangedCarb.Protein = newCarb.Protein;
        meal.ChangedCarb.Fiber = newCarb.Fiber;
        meal.ChangedCarb.Fat = newCarb.Fat;
        meal.ChangedCarb.NameAR = newCarb.NameAR;
        meal.ChangedCarb.NameEN = newCarb.NameEN;
        
        meal.Carbs += meal.ChangedCarb.Carb;
        meal.Fats += meal.ChangedCarb.Fat;
        meal.Protein += meal.ChangedCarb.Protein;
        meal.Fibers += meal.ChangedCarb.Fiber;
        
    }

    public static void CalcNewPropertyForCarbOfMeal(UserMealCarb carbSelected, decimal carbWeight, decimal ingredientWeight)
    {
        var percent = carbWeight / ingredientWeight;
        carbSelected.Protein *= percent;
        carbSelected.Fat *= percent;
        carbSelected.Carb *= percent;
        carbSelected.Fiber *= percent;
    }
}