using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
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
        await SeedIngredients(context);
        await SeedAllergies(context);
        await SeedMeals(context);
        await SeedAdminPlans(context);
        await SeedZones(context);
        await SeedDrinks(context: context);
        await SeedExtraOptions(context);
        await SeedPlanTypes(context);
    }

    private static async Task SeedPlanTypes(DataContext context)
    {
        if (await context.PlanTypes.AnyAsync()) return;

        var plans = new List<PlanType>
        {
            new PlanType
            {
                Points = 50,
                PlanTypeE = PlanTypeEnum.LossWeight
            },

            new PlanType
            {
                Points = 30,
                PlanTypeE = PlanTypeEnum.MaintainWeight
            },

            new PlanType
            {
                Points = 40,
                PlanTypeE = PlanTypeEnum.FutureLeader
            }
        };

        await context.PlanTypes.AddRangeAsync(plans);
        await context.SaveChangesAsync();
    }

    private static async Task SeedAllergies(DataContext context)
    {
        if (await context.Allergies.AnyAsync()) return;

        var allergies = new List<Allergy>
        {
            new Allergy("عدس", "Lentil", "lentil.jpg"),
            new Allergy("قمح", "Wheat", "wheat.jpg"),
            new Allergy("بيض", "Egg", "egg.jpg"),
            new Allergy("مكسرات", "Nuts", "nuts.jpg"),
            new Allergy("سمك", "Fish", "fish.jpg"),
            new Allergy("فول الصويا", "Soy", "soy.jpg"),
            new Allergy("حليب", "Milk", "milk.jpg"),
            new Allergy("جلوتين", "Gluten", "gluten.jpg"),
            new Allergy("جوز الهند", "Coconut", "coconut.jpg"),
            new Allergy("فستق", "Peanut", "peanut.jpg")
        };

        await context.Allergies.AddRangeAsync(allergies);
        await context.SaveChangesAsync();
    }

    private static async Task SeedExtraOptions(DataContext context)
    {
        if (await context.ExtraOptions.AnyAsync()) return;

        context.ExtraOptions.Add(new ExtraOption
        {
            NameEnglish = "Caesar Salad", NameArabic = "سلطة السيزر", Price = 2.5m, Weight = 100,
            PictureUrl = "caesar_salad.jpg", OptionType = ExtraOptionType.Salad
        });
        context.ExtraOptions.Add(new ExtraOption
        {
            NameEnglish = "Mixed Nuts", NameArabic = "مكسرات مشكلة", Price = 1.8m, Weight = 80,
            PictureUrl = "mixed_nuts.jpg", OptionType = ExtraOptionType.Nuts
        });
        context.ExtraOptions.Add(new ExtraOption
        {
            NameEnglish = "Garlic Sauce", NameArabic = "صلصة الثوم", Price = 0.8m, Weight = 50,
            PictureUrl = "garlic_sauce.jpg", OptionType = ExtraOptionType.Sauce
        });
        // Add more extra options here
        await context.SaveChangesAsync();
    }


    private static async Task SeedDrinks(DataContext context)
    {
        if (await context.Drinks.AnyAsync()) return;

        context.Drinks.Add(new Drink
        {
            NameEnglish = "Orange Juice", NameArabic = "عصير البرتقال", Price = 2.5m, Volume = CapacityLevel.Medium,
            PictureUrl = "orange_juice.jpg"
        });
        context.Drinks.Add(new Drink
        {
            NameEnglish = "Coffee", NameArabic = "قهوة", Price = 3.0m, Volume = CapacityLevel.Small,
            PictureUrl = "coffee.jpg"
        });
        context.Drinks.Add(new Drink
        {
            NameEnglish = "Tea", NameArabic = "شاي", Price = 1.8m, Volume = CapacityLevel.Small, PictureUrl = "tea.jpg"
        });
        context.Drinks.Add(new Drink
        {
            NameEnglish = "Milkshake", NameArabic = "ميلك شيك", Price = 4.0m, Volume = CapacityLevel.Large,
            PictureUrl = "milkshake.jpg"
        });
        // Add more drinks here
        await context.SaveChangesAsync();
    }

    private static async Task SeedZones(DataContext context)
    {
        if (await context.Zones.AnyAsync()) return;

        var zones = new List<Zone>
        {
            new Zone { NameAR = "المنطقة الشمالية", NameEN = "North Zone" },
            new Zone { NameAR = "المنطقة الجنوبية", NameEN = "South Zone" },
            new Zone { NameAR = "المنطقة الغربية", NameEN = "West Zone" },
        };

        await context.Zones.AddRangeAsync(zones);
        await context.SaveChangesAsync();
    }

    private static async Task SeedRoles(RoleManager<AppRole> roleManager)
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

    private static async Task SeedAdminPlans(DataContext context)
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
            context.AdminPlans.Add(new AdminPlanDay
            {
                PlanType = PlanTypeEnum.MaintainWeight,
                AvailableDate = date
            });
            context.AdminPlans.Add(new AdminPlanDay
            {
                PlanType = PlanTypeEnum.LossWeight,
                AvailableDate = date
            });
            context.AdminPlans.Add(new AdminPlanDay
            {
                PlanType = PlanTypeEnum.FutureLeader,
                AvailableDate = date
            });
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedCategories(DataContext context)
    {
        if (await context.Categories.AnyAsync()) return;

        var categories = new List<Category>
        {
            new Category("Breakfast", "وجبة الفطور", "Morning meal options"),
            new Category("Lunch", "وجبة الغداء", "Midday meal options"),
            new Category("Dinner", "وجبة العشاء", "Evening meal options"),
            // Add more categories here
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }

    private static async Task SeedBranches(DataContext context)
    {
        if (await context.Branches.AnyAsync()) return;

        var branches = new List<Branch>
        {
            new Branch
            {
                NameEN = "Main Branch",
                NameAR = "الفرع الرئيسي",
                Address = new Location
                    { City = "Main City", StreetName = "Main Street", Longitude = 0.0m, Latitude = 0.0m }
            },
            new Branch
            {
                NameEN = "South Branch",
                NameAR = "الفرع الجنوبي",
                Address = new Location
                    { City = "South City", StreetName = "South Street", Longitude = 0.0m, Latitude = 0.0m }
            },
            new Branch
            {
                NameEN = "West Branch",
                NameAR = "الفرع الغربي",
                Address = new Location
                    { City = "West City", StreetName = "West Street", Longitude = 0.0m, Latitude = 0.0m }
            },
        };

        await context.Branches.AddRangeAsync(branches);
        await context.SaveChangesAsync();
    }

    private static async Task SeedMeals(DataContext context)
    {
        if (await context.Meals.AnyAsync()) return;

        // Fetch ingredients from the database
        var ingredients = await context.Ingredients.ToListAsync();

        var meals = new List<Meal>
        {
            new Meal
            {
                NameEN = "Chicken Curry",
                NameAR = "كاري الدجاج",
                DescriptionEN = "A delicious Indian-style chicken curry served with basmati rice.",
                DescriptionAR = "كاري دجاج هندي شهي مقدم مع أرز بسمتي.",
                Price = 25,
                Points = 8,
                PictureUrl = "chicken_curry.jpg",
                IsMealPlan = false,
                IsMainMenu = true,
                CategoryId = 1,
                Ingredients = new List<MealIngredient>
                {
                    new MealIngredient { Ingredient = ingredients.FirstOrDefault(i => i.Id == 2), Weight = 150 },
                    new MealIngredient { Ingredient = ingredients.FirstOrDefault(i => i.Id == 6), Weight = 100 },
                    new MealIngredient { Ingredient = ingredients.FirstOrDefault(i => i.Id == 1), Weight = 200 }
                },
                Allergies = new List<MealAllergy>
                {
                    new MealAllergy { AllergyId = 3 }, // Egg
                    new MealAllergy { AllergyId = 4 } // Nuts
                }
            },
            new Meal
            {
                NameEN = "Salmon Salad",
                NameAR = "سلطة السلمون",
                DescriptionEN = "Fresh salad with grilled salmon, spinach, tomatoes, and olive oil dressing.",
                DescriptionAR = "سلطة طازجة مع سلمون مشوي وسبانخ وطماطم وصلصة زيتون.",
                Price = 18,
                Points = 6,
                PictureUrl = "salmon_salad.jpg",
                IsMealPlan = true,
                IsMainMenu = false,
                CategoryId = 2,
                Ingredients = new List<MealIngredient>
                {
                    new MealIngredient { Ingredient = ingredients.FirstOrDefault(i => i.Id == 7), Weight = 180 },
                    new MealIngredient { Ingredient = ingredients.FirstOrDefault(i => i.Id == 3), Weight = 100 },
                    new MealIngredient { Ingredient = ingredients.FirstOrDefault(i => i.Id == 4), Weight = 80 },
                    new MealIngredient { Ingredient = ingredients.FirstOrDefault(i => i.Id == 5), Weight = 20 }
                },
                Allergies = new List<MealAllergy>
                {
                    new MealAllergy { AllergyId = 5 } // Fish
                }
            },
            new Meal
            {
                NameEN = "Chicken Caesar Salad",
                NameAR = "سلطة الدجاج القيصرية",
                DescriptionEN =
                    "Classic Caesar salad with grilled chicken breast, romaine lettuce, croutons, and Caesar dressing.",
                DescriptionAR = "سلطة السيزار الكلاسيكية مع صدر دجاج مشوي وخس رومين وكروتون وصلصة السيزار.",
                Price = 12.99m,
                Points = 5,
                PictureUrl = "chicken_caesar_salad.jpg",
                IsMealPlan = true,
                IsMainMenu = false,
                CategoryId = 2,
                Ingredients = new List<MealIngredient>
                {
                    new MealIngredient { Ingredient = ingredients.FirstOrDefault(i => i.Id == 7), Weight = 180 },
                    new MealIngredient { Ingredient = ingredients.FirstOrDefault(i => i.Id == 3), Weight = 100 },
                    new MealIngredient { Ingredient = ingredients.FirstOrDefault(i => i.Id == 4), Weight = 80 },
                    new MealIngredient { Ingredient = ingredients.FirstOrDefault(i => i.Id == 5), Weight = 20 }
                },
                Allergies = new List<MealAllergy>
                {
                    new MealAllergy { AllergyId = 1 } // Gluten
                }
            },
            new Meal
            {
                NameEN = "Vegetable Stir-Fry",
                NameAR = "ستر-فراي الخضار",
                DescriptionEN = "Assorted vegetables stir-fried in a savory sauce, served with rice or noodles.",
                DescriptionAR = "خضروات متنوعة مقلية بصلصة لذيذة، مقدمة مع أرز أو معكرونة.",
                Price = 10.49m,
                Points = 4,
                PictureUrl = "vegetable_stir_fry.jpg",
                IsMealPlan = false,
                IsMainMenu = true,
                CategoryId = 2,
                Ingredients = new List<MealIngredient>
                {
                    new MealIngredient { Ingredient = ingredients.FirstOrDefault(i => i.Id == 7), Weight = 180 },
                    new MealIngredient { Ingredient = ingredients.FirstOrDefault(i => i.Id == 3), Weight = 100 },
                    new MealIngredient { Ingredient = ingredients.FirstOrDefault(i => i.Id == 4), Weight = 80 },
                    new MealIngredient { Ingredient = ingredients.FirstOrDefault(i => i.Id == 5), Weight = 20 }
                },
                Allergies = new List<MealAllergy>
                {
                    new MealAllergy { AllergyId = 6 } // Soy
                }
            },
        };

        await context.Meals.AddRangeAsync(meals);
        await context.SaveChangesAsync();
    }

    private static async Task SeedIngredients(DataContext context)
    {
        if (await context.Ingredients.AnyAsync()) return;

        var ingredients = new List<Ingredient>
        {
            new Ingredient("Rice", "أرز", "Long grain rice", 200, 5, 1, 40, 0, 3, IngredientType.Carb),
            new Ingredient("Chicken", "دجاج", "Boneless chicken breast", 150, 15, 30, 0, 3, 0, IngredientType.Protein),
            new Ingredient("Spinach", "سبانخ", "Fresh spinach leaves", 100, 3, 2, 5, 0, 2, IngredientType.Carb),
            new Ingredient("Tomato", "طماطم", "Ripe tomatoes", 80, 2, 1, 4, 0, 1, IngredientType.Protein),
            new Ingredient("Olive Oil", "زيت زيتون", "Extra virgin olive oil", 20, 10, 0, 0, 10, 0,
                IngredientType.Soup),
            new Ingredient("Lentils", "عدس", "Dried lentils", 120, 3, 9, 20, 0, 8, IngredientType.Soup),
            new Ingredient("Salmon", "سلمون", "Fresh salmon fillet", 180, 20, 25, 0, 12, 0, IngredientType.Protein)
        };

        await context.Ingredients.AddRangeAsync(ingredients);
        await context.SaveChangesAsync();
    }
}