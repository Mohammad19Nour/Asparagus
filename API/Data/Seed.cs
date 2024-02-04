using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Entities;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Data;

public static class Seed
{
    public static async Task SeedData(DataContext context)
    {
        await SeedBranches(context);
        await SeedCategories(context);
        await SeedIngre(context);
        await SeedMeals(context);
        await SeedAdminPlans(context);

    }

    public static async Task SeedMeals(DataContext context)
    {
        if (await context.Meals.AnyAsync()) return;

        for (int i = 0; i < 5; i++)
        {
            
        context.Meals.Add(new Meal
        {
            BranchId = 1,
            CategoryId = 1,
            NameEN = "ee",
            NameAR = "eww",
            DescriptionEN = "ddd",
            DescriptionAR = "gew",
            IsMainMenu = (i % 2 == 0),
            IsMealPlan = (i % 2 == 1),
            Ingredients = new List<MealIngredient>
            {
                new() {IngredientId = 1}
            },
            PictureUrl = "ew"
        });
        }
        await context.SaveChangesAsync();
    }

    public static async Task SeedAdminPlans(DataContext context)
    {
        if (await context.AdminPlans.AnyAsync()) return;

        for (int j = 0; j <= 6; j++)
        {
            var date = DateTime.Now.AddDays(j).Date;
            context.AdminPlans.Add(new AdminPlan
            {
                PlanType = MealPlanType.MaintainWeight,
                AvailableDate = date
            });
        }

        await context.SaveChangesAsync();
    }

    public static async Task SeedBranches(DataContext context)
    {
        if (await context.Branches.AnyAsync()) return;

        context.Branches.Add(new Branch
        {
            Name = "branch 1",
            Address = new Address
            {
                City = "drr", BuildingName = "t", StreetName = "aee", ApartmentNumber = 20, Longitude = 20, Latitude = 5
            }
        });
        context.Branches.Add(new Branch
        {
            Name = "branch 2",
            Address = new Address
            {
                City = "drr", BuildingName = "t", StreetName = "aee", ApartmentNumber = 20, Longitude = 20, Latitude = 5
            }
        });
        context.Branches.Add(new Branch
        {
            Name = "branch 3",
            Address = new Address
            {
                City = "drr", BuildingName = "t", StreetName = "aee", ApartmentNumber = 20, Longitude = 20, Latitude = 5
            }
        });
        await context.SaveChangesAsync();
    }

    public static async Task SeedCategories(DataContext context)
    {
        if (await context.Categories.AnyAsync()) return;

        context.Categories.Add(new Category{NameAR = "ara 1" , NameEN = "eng 1",Description = "arrr"});
        context.Categories.Add(new Category{NameAR = "ara 2" , NameEN = "eng 2",Description = "arrr"});
        context.Categories.Add(new Category{NameAR = "ara 3" , NameEN = "eng 3",Description = "arrr"});
        await context.SaveChangesAsync();
    }

    public static async Task SeedIngre(DataContext context)
    {
        if (await context.Ingredients.AnyAsync()) return;
        context.Ingredients.Add(new Ingredient { NameAR = "EAE",NameEN = "WFWe",});
        await context.SaveChangesAsync();
    }
    
}