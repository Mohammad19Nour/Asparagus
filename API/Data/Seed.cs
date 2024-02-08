using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Entities;
using AsparagusN.Entities.Identity;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AsparagusN.Data;

public static class Seed
{
    public static async Task SeedData(DataContext context, RoleManager<AppRole> roleManager)
    {
        await SeedRoles(roleManager);
        await SeedBranches(context);
        await SeedCategories(context);
        await SeedIngre(context);
        await SeedMeals(context);
        await SeedAdminPlans(context);
        await SeedZones(context);
        await SeedDrinks(context: context);
        await SeedExtraOptions(context);
    }
    private static async Task SeedExtraOptions(DataContext context)
    {
        if (await context.ExtraOptions.AnyAsync()) return;

        context.ExtraOptions.Add(new ExtraOption { NameEnglish = "Caesar Salad", NameArabic = "سلطة السيزر", Price = 2.5m, Weight = 100, PictureUrl = "caesar_salad.jpg", OptionType = ExtraOptionType.Salad });
        context.ExtraOptions.Add(new ExtraOption { NameEnglish = "Mixed Nuts", NameArabic = "مكسرات مشكلة", Price = 1.8m, Weight = 80, PictureUrl = "mixed_nuts.jpg", OptionType = ExtraOptionType.Nuts });
        context.ExtraOptions.Add(new ExtraOption { NameEnglish = "Garlic Sauce", NameArabic = "صلصة الثوم", Price = 0.8m, Weight = 50, PictureUrl = "garlic_sauce.jpg", OptionType = ExtraOptionType.Sauce });
        // Add more extra options here
        await context.SaveChangesAsync();
    }


    private static async Task SeedDrinks(DataContext context)
    {
        if (await context.Drinks.AnyAsync()) return;

        context.Drinks.Add(new Drink { NameEnglish = "Orange Juice", NameArabic = "عصير البرتقال", Price = 2.5m, Volume = CapacityLevel.Medium, PictureUrl = "orange_juice.jpg" });
        context.Drinks.Add(new Drink { NameEnglish = "Coffee", NameArabic = "قهوة", Price = 3.0m, Volume = CapacityLevel.Small, PictureUrl = "coffee.jpg" });
        context.Drinks.Add(new Drink { NameEnglish = "Tea", NameArabic = "شاي", Price = 1.8m, Volume = CapacityLevel.Small, PictureUrl = "tea.jpg" });
        context.Drinks.Add(new Drink { NameEnglish = "Milkshake", NameArabic = "ميلك شيك", Price = 4.0m, Volume = CapacityLevel.Large, PictureUrl = "milkshake.jpg" });
        // Add more drinks here
        await context.SaveChangesAsync();
    }

    private static async Task SeedZones(DataContext context)
    {
        if (await context.Zones.AnyAsync()) return;

        context.Zones.Add(new Zone { NameEN = "Test 1", NameAR = "سنسس 1" });
        context.Zones.Add(new Zone { NameEN = "Test 2", NameAR = "سن2" });
        await context.SaveChangesAsync();
    }

    public static async Task SeedRoles(RoleManager<AppRole> roleManager)
    {
        if (await roleManager.Roles.AnyAsync()) return;

        var roles = new List<AppRole>
        {
            new() { Name = "Admin" },
            new() { Name = "Driver" },
            new() { Name = "User" }
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }
    }

    public static async Task SeedMeals(DataContext context)
    {
        if (await context.Meals.AnyAsync()) return;

        for (int i = 0; i < 5; i++)
        {
            context.Meals.Add(new Meal
            {
                CategoryId = 1,
                NameEN = "ee",
                NameAR = "eww",
                DescriptionEN = "ddd",
                DescriptionAR = "gew",
                IsMainMenu = (i % 2 == 0),
                IsMealPlan = (i % 2 == 1),
                Ingredients = new List<MealIngredient>
                {
                    new() { IngredientId = 1 }
                },
                PictureUrl = "ew"
            });
        }

        await context.SaveChangesAsync();
    }

    public static async Task SeedAdminPlans(DataContext context)
    {
        if (await context.AdminPlans.AnyAsync()) return;

        var todayDay = DateTime.Now.DayOfWeek;
        var startDay = DateTime.Now;
        if (todayDay != DayOfWeek.Thursday)
        {
            while (startDay.DayOfWeek != DayOfWeek.Thursday)
            {
                startDay = startDay.AddDays(-1);
            }
        }

        for (var j = 0; j <= 8; j++)
        {
            var date = startDay.AddDays(j).Date;
            context.AdminPlans.Add(new AdminPlan
            {
                PlanType = PlanType.MaintainWeight,
                AvailableDate = date
            });
            context.AdminPlans.Add(new AdminPlan
            {
                PlanType = PlanType.LossWeight,
                AvailableDate = date
            });
            context.AdminPlans.Add(new AdminPlan
            {
                PlanType = PlanType.FutureLeader,
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
            NameEN = "branch 1",
            NameAR = "فرع 1",
            Address = new Location
            {
                City = "drr", StreetName = "aee", Longitude = 20, Latitude = 5
            }
        });
        context.Branches.Add(new Branch
        {
            NameEN = "branch 2",
            NameAR = "فرع 2",
            Address = new Location
            {
                City = "drr", StreetName = "aee", Longitude = 20, Latitude = 5
            }
        });
        context.Branches.Add(new Branch
        {
            NameEN = "branch 3",
            NameAR = "فرع 3",
            Address = new Location
            {
                City = "drr", StreetName = "aee", Longitude = 20, Latitude = 5
            }
        });
        await context.SaveChangesAsync();
    }

    public static async Task SeedCategories(DataContext context)
    {
        if (await context.Categories.AnyAsync()) return;

        context.Categories.Add(new Category { NameAR = "ara 1", NameEN = "eng 1", Description = "arrr" });
        context.Categories.Add(new Category { NameAR = "ara 2", NameEN = "eng 2", Description = "arrr" });
        context.Categories.Add(new Category { NameAR = "ara 3", NameEN = "eng 3", Description = "arrr" });
        await context.SaveChangesAsync();
    }

    public static async Task SeedIngre(DataContext context)
    {
        if (await context.Ingredients.AnyAsync()) return;
        context.Ingredients.Add(new Ingredient { NameAR = "EAE", NameEN = "WFWe", Price = 15 });
        await context.SaveChangesAsync();
    }
}