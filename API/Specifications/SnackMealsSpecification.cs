using System.Linq.Expressions;
using AsparagusN.Data.Entities.Meal;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Specifications;

public class SnackMealsSpecification:BaseSpecification<Meal>
{
    public SnackMealsSpecification() : base(x=>!x.IsDeleted && x.IsMealPlan && x.Category.NameEN.ToLower().Contains("snacks"))
    {
        AddInclude(x=>x.Include(y=>y.Category));
    }
    public SnackMealsSpecification(bool isMenu) : base(x=>!x.IsDeleted && x.IsMainMenu && x.Category.NameEN.ToLower().Contains("snacks"))
    {
        AddInclude(x=>x.Include(y=>y.Category));
    }
}