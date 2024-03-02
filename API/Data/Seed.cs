using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Data.Entities.Meal;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.DTOs.UserPlanDtos;
using AsparagusN.Enums;
using AsparagusN.Helpers;
using AsparagusN.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace AsparagusN.Data;

public static class Seed
{
    public static async Task SeedData(DataContext context, RoleManager<AppRole> roleManager,
        UserManager<AppUser> userManager, ISubscriptionService subscriptionService, IMapper mapper)
    {
        await SeedRoles(roleManager);
        await SeedAdminPlans(context);
        await SeedAllergies(context);
        await SeedCategories(context);
        await SeedBranches(context);
        await SeedIngredients(context);
        await SeedDrinks(context);
        await SeedExtraOptions(context);
        await SeedZones(context);
        await SeedMeals(context);
        await SeedAdminSelectedDrinks(context);
        await SeedAdminSelectedExtraOptions(context);
        await SeedAdminSelectedMeals(context);
        await SeedAdminSelectedSnacks(context);
        await SeedPlanTypes(context);
        await SeedAdminSelectedCarbs(context);
        await SeedUsers(userManager);
        await SeedDrivers(context, userManager);
        await SeedCashiers(context, userManager);
        await SeedOrders(context, mapper);
        await SeedGifts(context);
        await SeedCar(context);
        await SeedBundles(context);
        await SeedSubscription(context, subscriptionService);
        await SeedAssign(context);
    }

    private static async Task SeedAssign(DataContext context)
    {
        var drivers = await context.Drivers.ToListAsync();
        var days = await context.UserPlanDays.ToListAsync();

        int p = 1;
        int id = 1;
        int tmp = days.Count / drivers.Count;
        int x = 1;
        foreach (var day in days)
        {
            if (x + 10 == days.Count) break;

            day.DriverId = id;
            day.Priority = p;
            day.DayOrderStatus = (x % 2 == 0) ? PlanOrderStatus.Delivered : PlanOrderStatus.Ready;
            p++;
            if (p > tmp)
            {
                p = 1;
                id++;
            }

            x++;
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedSubscription(DataContext context, ISubscriptionService subscriptionService)
    {
        if (await context.UserPlans.AnyAsync()) return;
        var users = await context.Users.Where(x => x.IsNormalUser).ToListAsync();

        var bundles = await context.Bundles.ToListAsync();
        foreach (var user in users)
        {
            foreach (var bundle in bundles)
            {
                foreach (PlanTypeEnum plan in Enum.GetValues(typeof(PlanTypeEnum)))
                {
                    if (plan == PlanTypeEnum.CustomMealPlan) continue;

                    var sub = new NewSubscriptionDto
                    {
                        Duration = bundle.Duration,
                        NumberOfMealPerDay = bundle.MealsPerDay,
                        NumberOfSnacks = bundle.Duration * bundle.MealsPerDay,
                        DeliveryCity = "Test",
                        PlanType = plan
                    };
                    var result = await subscriptionService.CreateSubscriptionAsync(sub, user);
                }
            }
        }
    }


    private static async Task SeedBundles(DataContext context)
    {
        if (await context.Bundles.AnyAsync()) return;
        for (int i = 1; i <= 3; i++)
        {
            foreach (int d in Enum.GetValues(typeof(SubscriptionDuration)))
            {
                context.Bundles.Add(new Bundle
                {
                    Duration = d,
                    MealsPerDay = i,
                    Price = i * d * 3
                });
            }
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedCar(DataContext context)
    {
        if (await context.Cars.AnyAsync()) return;

        context.Cars.Add(new Car());
        await context.SaveChangesAsync();
    }

    private static async Task SeedGifts(DataContext context)
    {
        if (await context.GiftSelections.AnyAsync()) return;
        for (var j = 1; j <= 12; j++)
        {
            var newOne = new GiftSelection
            {
                Month = j,
                MonthName = new DateTime(2024, j, 1).ToString("MMMMM")
            };
            context.GiftSelections.Add(newOne);
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedDrivers(DataContext context, UserManager<AppUser> userManager)
    {
        if (await context.Drivers.AnyAsync()) return;
        var drivers = new List<Driver>
        {
            new Driver
            {
                Name = "John Doe",
                PhoneNumber = "1234567890",
                Email = "john@exampdle.com",
                Password = "string",
                IsActive = true,
                ZoneId = 3, // Example zone ID
                PictureUrl = "https://example.com/picture.jpg",
                Period = Period.Day
            },
            new Driver
            {
                Name = "Jane Smith",
                PhoneNumber = "0987654321",
                Email = "jane@examplee.com",
                Password = "string",
                IsActive = true,
                ZoneId = 5, // Example zone ID
                PictureUrl = "https://example.com/picture2.jpg",
                Period = Period.Night
            },
            new Driver
            {
                Name = "Alice Johnson",
                PhoneNumber = "5551234567",
                Email = "alice@examplee.com",
                Password = "string",
                IsActive = true,
                ZoneId = 2, // Example zone ID
                PictureUrl = "https://example.com/alice.jpg",
                Period = Period.Day
            },
            new Driver
            {
                Name = "Bob Smith",
                PhoneNumber = "5559876543",
                Email = "bob@examplee.com",
                Password = "string",
                IsActive = true,
                ZoneId = 4, // Example zone ID
                PictureUrl = "https://example.com/bob.jpg",
                Period = Period.Night
            }
        };
        foreach (var cashier in drivers)
        {
            var user = new AppUser
            {
                Email = cashier.Email,
                UserName = cashier.Email,
                FullName = cashier.Name,
                EmailConfirmed = true,
            };
            await userManager.CreateAsync(user, "string");
            await userManager.AddToRoleAsync(user, Roles.Driver.GetDisplayName());
        }

        await context.Drivers.AddRangeAsync(drivers);
    }

    private static async Task SeedCashiers(DataContext context, UserManager<AppUser> userManager)
    {
        if (await context.Cashiers.AnyAsync()) return;
        var cashiers = new List<Cashier>
        {
            new Cashier
            {
                Name = "John Doe",
                PhoneNumber = "1234567890",
                Email = "john@example.com",
                Password = "string",
                IsActive = true,
                BranchId = 3,
                PictureUrl = "https://example.com/picture.jpg",
                Period = Period.Day
            },
            new Cashier
            {
                Name = "Jane Smith",
                PhoneNumber = "0987654321",
                Email = "jane@example.com",
                Password = "string",
                IsActive = true,
                BranchId = 3, // Example zone ID
                PictureUrl = "https://example.com/picture2.jpg",
                Period = Period.Night
            },
            new Cashier
            {
                Name = "Alice Johnson",
                PhoneNumber = "5551234567",
                Email = "alice@example.com",
                Password = "string",
                IsActive = true,
                BranchId = 2, // Example zone ID
                PictureUrl = "https://example.com/alice.jpg",
                Period = Period.Day
            },
            new Cashier
            {
                Name = "Bob Smith",
                PhoneNumber = "5559876543",
                Email = "bob@example.com",
                Password = "string",
                IsActive = true,
                BranchId = 1, // Example zone ID
                PictureUrl = "https://example.com/bob.jpg",
                Period = Period.Night
            }
        };

        foreach (var cashier in cashiers)
        {
            var user = new AppUser
            {
                Email = cashier.Email,
                UserName = cashier.Email,
                FullName = cashier.Name,
                EmailConfirmed = true,
            };
            await userManager.CreateAsync(user, "string");
            await userManager.AddToRoleAsync(user, Roles.Cashier.GetDisplayName());
        }

        await context.Cashiers.AddRangeAsync(cashiers);
    }

    private static decimal CalculatePrice(SubscriptionDuration duration, int numberOfMeals)
    {
        return duration switch
        {
            SubscriptionDuration.FiveDays => 50.00m * numberOfMeals,
            SubscriptionDuration.SevenDays => 70.00m * numberOfMeals,
            SubscriptionDuration.FifteenDays => 120.00m * numberOfMeals,
            SubscriptionDuration.ThirtyDays => 200.00m * numberOfMeals,
            _ => throw new ArgumentOutOfRangeException(nameof(duration), "Unknown duration")
        };
    }

    private static async Task SeedRoles(RoleManager<AppRole> roleManager)
    {
        if (await roleManager.Roles.AnyAsync()) return;

        var roles = new List<AppRole>
        {
            new() { Name = Roles.Admin.GetDisplayName() },
            new() { Name = Roles.Driver.GetDisplayName() },
            new() { Name = Roles.User.GetDisplayName() },
            new() { Name = Roles.Cashier.GetDisplayName() },
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        var rr = Enum.GetNames(typeof(DashboardRoles)).ToList();
        foreach (var role in rr)
        {
            await roleManager.CreateAsync(new AppRole { Name = role });
        }
    }

    private static async Task SeedUsers(UserManager<AppUser> userManager)
    {
        if (await userManager.Users.AnyAsync()) return;

        var user = new AppUser
        {
            Email = "admin",
            UserName = "admin",
            FullName = "admin",
            EmailConfirmed = true,
        };
        await userManager.CreateAsync(user, "string");
        await userManager.AddToRoleAsync(user, Roles.Admin.GetDisplayName());
        user = new AppUser
        {
            Email = "u@u.u",
            UserName = "u@u.u",
            FullName = "user",
            EmailConfirmed = true,
            LoyaltyPoints = 152362,
            IsNormalUser = true,
            HomeAddress = new Address
            {
                City = "Test",
                BuildingName = "ttt",
                StreetName = "gfdd",
                ApartmentNumber = 250
            }
        };
        await userManager.CreateAsync(user, "string");
        await userManager.AddToRoleAsync(user, Roles.User.GetDisplayName());
        user = new AppUser
        {
            Email = "ud@u.u",
            UserName = "du@u.u",
            FullName = "duser",
            EmailConfirmed = true,
            LoyaltyPoints = 152362,
            IsNormalUser = true, HomeAddress = new Address
            {
                City = "Test",
                BuildingName = "ttt",
                StreetName = "gfdd",
                ApartmentNumber = 250
            }
        };
        await userManager.CreateAsync(user, "string");
        await userManager.AddToRoleAsync(user, Roles.User.GetDisplayName());
        user = new AppUser
        {
            Email = "ud@u.vvu",
            UserName = "du@u.vvu",
            FullName = "duser",
            EmailConfirmed = true,
            LoyaltyPoints = 152362,
            IsNormalUser = true, HomeAddress = new Address
            {
                City = "Test",
                BuildingName = "ttt",
                StreetName = "gfdd",
                ApartmentNumber = 250
            }
        };
        await userManager.CreateAsync(user, "string");
        await userManager.AddToRoleAsync(user, Roles.User.GetDisplayName());
        user = new AppUser
        {
            Email = "ud@u.ssu",
            UserName = "du@u.ssu",
            FullName = "duser",
            EmailConfirmed = true,
            LoyaltyPoints = 152362,
            IsNormalUser = true, HomeAddress = new Address
            {
                City = "Test",
                BuildingName = "ttt",
                StreetName = "gfdd",
                ApartmentNumber = 250
            }
        };
        await userManager.CreateAsync(user, "string");
        await userManager.AddToRoleAsync(user, Roles.User.GetDisplayName());
    }

    private static async Task SeedPrices(DataContext context)
    {
        return;
        if (await context.PlanPrices.AnyAsync()) return;
        foreach (SubscriptionDuration duration in Enum.GetValues(typeof(SubscriptionDuration)))
        {
            for (var j = 1; j <= 3; j++)
            {
                context.PlanPrices.Add(new PlanPrice
                    { Duration = duration, NumberOfMealsPerDay = 3, Price = CalculatePrice(duration, j) }
                );
            }
        }
    }

    private static async Task SeedAllergies(DataContext context)
    {
        if (await context.Allergies.AnyAsync()) return;

        var allergies = new List<Allergy>
        {
            new Allergy("عدس", "Lentil", "images/lentil.jpg"),
            new Allergy("قمح", "Wheat", "images/wheat.jpg"),
            new Allergy("بيض", "Egg", "images/egg.jpg"),
            new Allergy("مكسرات", "Nuts", "images/nuts.jpg"),
            new Allergy("سمك", "Fish", "images/fish.jpg"),
            new Allergy("فول الصويا", "Soy", "images/soy.jpg"),
            new Allergy("حليب", "Milk", "images/milk.jpg"),
            new Allergy("جلوتين", "Gluten", "images/gluten.jpg"),
            new Allergy("جوز الهند", "Coconut", "images/coconut.jpg"),
            new Allergy("فستق", "Peanut", "images/peanut.jpg")
        };

        await context.Allergies.AddRangeAsync(allergies);
        await context.SaveChangesAsync();
    }

    private static async Task SeedCategories(DataContext _context)
    {
        if (await _context.Categories.AnyAsync()) return;

        var random = new Random();
        var categories = new List<Category>
        {
            new Category("Main Course", "الطبق الرئيسي", "Description for main courses"),
            new Category("Appetizers", "المقبلات", "Description for appetizers"),
            new Category("Salads", "السلطات", "Description for salads"),
            new Category("Desserts", "الحلويات", "Description for desserts"),
            new Category("Drinks", "المشروبات", "Description for drinks"),
            new Category("Snacks", "المقبلات", "Description for sides")
        };

        await _context.Categories.AddRangeAsync(categories);
        await _context.SaveChangesAsync();
    }

    private static async Task SeedBranches(DataContext _context)
    {
        if (await _context.Branches.AnyAsync()) return;

        var random = new Random();
        var branches = new List<Branch>
        {
            new Branch
            {
                NameEN = "Branch 1", NameAR = "الفرع 1",
                Address = new Location
                {
                    City = "City 1", StreetName = "Street 1", Latitude = Convert.ToDecimal(random.NextDouble()),
                    Longitude = Convert.ToDecimal(random.NextDouble())
                }
            },
            new Branch
            {
                NameEN = "Branch 2", NameAR = "الفرع 2",
                Address = new Location
                {
                    City = "City 2", StreetName = "Street 2", Latitude = Convert.ToDecimal(random.NextDouble()),
                    Longitude = Convert.ToDecimal(random.NextDouble())
                }
            },
            new Branch
            {
                NameEN = "Branch 3", NameAR = "الفرع 3",
                Address = new Location
                {
                    City = "City 3", StreetName = "Street 3", Latitude = Convert.ToDecimal(random.NextDouble()),
                    Longitude = Convert.ToDecimal(random.NextDouble())
                }
            },
            new Branch
            {
                NameEN = "Branch 4", NameAR = "الفرع 4",
                Address = new Location
                {
                    City = "City 4", StreetName = "Street 4", Latitude = Convert.ToDecimal(random.NextDouble()),
                    Longitude = Convert.ToDecimal(random.NextDouble())
                }
            },
            new Branch
            {
                NameEN = "Branch 5", NameAR = "الفرع 5",
                Address = new Location
                {
                    City = "City 5", StreetName = "Street 5", Latitude = Convert.ToDecimal(random.NextDouble()),
                    Longitude = Convert.ToDecimal(random.NextDouble())
                }
            },
            new Branch
            {
                NameEN = "Branch 6", NameAR = "الفرع 6",
                Address = new Location
                {
                    City = "City 6", StreetName = "Street 6", Latitude = Convert.ToDecimal(random.NextDouble()),
                    Longitude = Convert.ToDecimal(random.NextDouble())
                }
            },
            new Branch
            {
                NameEN = "Branch 7", NameAR = "الفرع 7",
                Address = new Location
                {
                    City = "City 7", StreetName = "Street 7", Latitude = Convert.ToDecimal(random.NextDouble()),
                    Longitude = Convert.ToDecimal(random.NextDouble())
                }
            }
        };

        await _context.Branches.AddRangeAsync(branches);
        await _context.SaveChangesAsync();
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

    private static async Task SeedIngredients(DataContext context)
    {
        if (await context.Ingredients.AnyAsync()) return;

        var random = new Random();
        var ingredients = new List<Ingredient>
        {
            new Ingredient("Tomato", "طماطم", "Info about Tomato", random.Next(50, 200),
                (decimal)random.NextDouble() * 10, (decimal)random.NextDouble() * 50, (decimal)random.NextDouble() * 5,
                (decimal)random.NextDouble() * 5, (decimal)random.NextDouble() * 2, (IngredientType)random.Next(3)),
            new Ingredient("Chicken Breast", "صدر دجاج", "Info about Chicken Breast", random.Next(50, 200),
                (decimal)random.NextDouble() * 10, (decimal)random.NextDouble() * 50, (decimal)random.NextDouble() * 5,
                (decimal)random.NextDouble() * 5, (decimal)random.NextDouble() * 2, (IngredientType)random.Next(3)),
            new Ingredient("Rice", "أرز", "Info about Rice", random.Next(50, 200), (decimal)random.NextDouble() * 10,
                (decimal)random.NextDouble() * 50, (decimal)random.NextDouble() * 5, (decimal)random.NextDouble() * 5,
                (decimal)random.NextDouble() * 2, (IngredientType)random.Next(3)),
            new Ingredient("Broccoli", "بروكلي", "Info about Broccoli", random.Next(50, 200),
                (decimal)random.NextDouble() * 10, (decimal)random.NextDouble() * 50, (decimal)random.NextDouble() * 5,
                (decimal)random.NextDouble() * 5, (decimal)random.NextDouble() * 2, (IngredientType)random.Next(3)),
            new Ingredient("Salmon", "سمك السلمون", "Info about Salmon", random.Next(50, 200),
                (decimal)random.NextDouble() * 10, (decimal)random.NextDouble() * 50, (decimal)random.NextDouble() * 5,
                (decimal)random.NextDouble() * 5, (decimal)random.NextDouble() * 2, (IngredientType)random.Next(3)),
            new Ingredient("Spinach", "سبانخ", "Info about Spinach", random.Next(50, 200),
                (decimal)random.NextDouble() * 10, (decimal)random.NextDouble() * 50, (decimal)random.NextDouble() * 5,
                (decimal)random.NextDouble() * 5, (decimal)random.NextDouble() * 2, (IngredientType)random.Next(3)),
            new Ingredient("Potato", "بطاطا", "Info about Potato", random.Next(50, 200),
                (decimal)random.NextDouble() * 10, (decimal)random.NextDouble() * 50, (decimal)random.NextDouble() * 5,
                (decimal)random.NextDouble() * 5, (decimal)random.NextDouble() * 2, (IngredientType)random.Next(3)),
        };

        await context.Ingredients.AddRangeAsync(ingredients);
        await context.SaveChangesAsync();
    }


    private static async Task SeedExtraOptions(DataContext context)
    {
        if (await context.ExtraOptions.AnyAsync()) return;

        var random = new Random();
        var extraOptions = new List<ExtraOption>
        {
            // Nuts
            new ExtraOption
            {
                NameEnglish = "Peanuts", NameArabic = "فول سوداني", Price = 3.99m, Weight = 30,
                PictureUrl = "images/peanuts.jpg", OptionType = ExtraOptionType.Nuts
            },
            new ExtraOption
            {
                NameEnglish = "Almonds", NameArabic = "لوز", Price = 4.99m, Weight = 40,
                PictureUrl = "images/almonds.jpg",
                OptionType = ExtraOptionType.Nuts
            },
            new ExtraOption
            {
                NameEnglish = "Cashews", NameArabic = "كاجو", Price = 5.99m, Weight = 50,
                PictureUrl = "images/cashews.jpg",
                OptionType = ExtraOptionType.Nuts
            },
            // Sauces
            new ExtraOption
            {
                NameEnglish = "Barbecue Sauce", NameArabic = "صوص الشواء", Price = 2.49m, Weight = 25,
                PictureUrl = "images/bbq_sauce.jpg", OptionType = ExtraOptionType.Sauce
            },
            new ExtraOption
            {
                NameEnglish = "Soy Sauce", NameArabic = "صوص الصويا", Price = 1.99m, Weight = 20,
                PictureUrl = "images/soy_sauce.jpg", OptionType = ExtraOptionType.Sauce
            },
            // Salads
            new ExtraOption
            {
                NameEnglish = "Caesar Salad", NameArabic = "سلطة السيزار", Price = 6.49m, Weight = 60,
                PictureUrl = "images/caesar_salad.jpg", OptionType = ExtraOptionType.Salad
            },
            new ExtraOption
            {
                NameEnglish = "Greek Salad", NameArabic = "سلطة يونانية", Price = 5.49m, Weight = 55,
                PictureUrl = "images/greek_salad.jpg", OptionType = ExtraOptionType.Salad
            },
            // Mixed
            new ExtraOption
            {
                NameEnglish = "Mixed Nuts", NameArabic = "مكسرات مشكلة", Price = 12.99m, Weight = 120,
                PictureUrl = "images/mixed_nuts.jpg", OptionType = ExtraOptionType.Nuts
            },
            new ExtraOption
            {
                NameEnglish = "Spicy Sauce", NameArabic = "صوص حار", Price = 3.99m, Weight = 35,
                PictureUrl = "images/spicy_sauce.jpg", OptionType = ExtraOptionType.Sauce
            }
        };

        await context.ExtraOptions.AddRangeAsync(extraOptions);
        await context.SaveChangesAsync();
    }


    private static async Task SeedDrinks(DataContext _context)
    {
        if (await _context.Drinks.AnyAsync()) return;

        var random = new Random();

        var drinks = new List<Drink>
        {
            new Drink
            {
                NameEnglish = "Water", NameArabic = "ماء", Price = (decimal)random.NextDouble() * 5,
                Volume = (CapacityLevel)random.Next(3), PictureUrl = "images/water.jpg"
            },
            new Drink
            {
                NameEnglish = "Orange Juice", NameArabic = "عصير البرتقال", Price = (decimal)random.NextDouble() * 10,
                Volume = (CapacityLevel)random.Next(3), PictureUrl = "images/orange_juice.jpg"
            },
            new Drink
            {
                NameEnglish = "Cola", NameArabic = "كولا", Price = (decimal)random.NextDouble() * 5,
                Volume = (CapacityLevel)random.Next(3), PictureUrl = "images/cola.jpg"
            },
            new Drink
            {
                NameEnglish = "Coffee", NameArabic = "قهوة", Price = (decimal)random.NextDouble() * 10,
                Volume = (CapacityLevel)random.Next(3), PictureUrl = "images/coffee.jpg"
            },
            new Drink
            {
                NameEnglish = "Tea", NameArabic = "شاي", Price = (decimal)random.NextDouble() * 5,
                Volume = (CapacityLevel)random.Next(3), PictureUrl = "images/tea.jpg"
            },
            new Drink
            {
                NameEnglish = "Mango Shake", NameArabic = "شيك المانجو", Price = (decimal)random.NextDouble() * 10,
                Volume = (CapacityLevel)random.Next(3), PictureUrl = "images/mango_shake.jpg"
            },
            new Drink
            {
                NameEnglish = "Lemonade", NameArabic = "عصير الليمون", Price = (decimal)random.NextDouble() * 5,
                Volume = (CapacityLevel)random.Next(3), PictureUrl = "images/lemonade.jpg"
            }
        };

        await _context.Drinks.AddRangeAsync(drinks);
        await _context.SaveChangesAsync();
    }


    private static async Task SeedZones(DataContext _context)
    {
        if (await _context.Zones.AnyAsync()) return;

        var random = new Random();
        var zones = new List<Zone>
        {
            new Zone { NameEN = "Zone 1", NameAR = "المنطقة 1" },
            new Zone { NameEN = "Zone 2", NameAR = "المنطقة 2" },
            new Zone { NameEN = "Zone 3", NameAR = "المنطقة 3" },
            new Zone { NameEN = "Zone 4", NameAR = "المنطقة 4" },
            new Zone { NameEN = "Zone 5", NameAR = "المنطقة 5" },
            new Zone { NameEN = "Zone 6", NameAR = "المنطقة 6" },
            new Zone { NameEN = "Zone 7", NameAR = "المنطقة 7" }
        };

        await _context.Zones.AddRangeAsync(zones);
        await _context.SaveChangesAsync();
    }

    private static async Task SeedAdminPlans(DataContext context)
    {
        var todayDay = DateTime.Now.DayOfWeek;

        if (await context.AdminPlans.AnyAsync())
        {
            if (todayDay != DayOfWeek.Friday && todayDay != DayOfWeek.Thursday)
                return;
        }

        var startDay = HelperFunctions.WeekStartDay();

        context.AdminPlans.RemoveRange(context.AdminPlans);
        for (var j = 0; j <= 6; j++)
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
                PictureUrl = "images/chicken_curry.jpg",
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
                PictureUrl = "images/salmon_salad.jpg",
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
                PictureUrl = "images/chicken_caesar_salad.jpg",
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
                PictureUrl = "images/vegetable_stir_fry.jpg",
                IsMealPlan = false,
                IsMainMenu = true,
                CategoryId = 6,
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
            new Meal
            {
                NameEN = "Salmon Salad",
                NameAR = "سلطة السلمون",
                DescriptionEN = "Fresh salad with grilled salmon, spinach, tomatoes, and olive oil dressing.",
                DescriptionAR = "سلطة طازجة مع سلمون مشوي وسبانخ وطماطم وصلصة زيتون.",
                Price = 18,
                Points = 6,
                PictureUrl = "images/salmon_salad.jpg",
                IsMealPlan = true,
                IsMainMenu = false,
                CategoryId = 3,
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
        };

        await context.Meals.AddRangeAsync(meals);
        await context.SaveChangesAsync();
    }

    private static async Task SeedAdminSelectedSnacks(DataContext _context)
    {
        if (await _context.AdminSelectedSnacks.AnyAsync()) return;

        var random = new Random();
        var adminSelectedSnacks = new List<AdminSelectedSnack>
        {
            new AdminSelectedSnack { SnackId = 4, PlanTypeEnum = PlanTypeEnum.LossWeight },
            new AdminSelectedSnack { SnackId = 3, PlanTypeEnum = PlanTypeEnum.LossWeight },
            new AdminSelectedSnack { SnackId = 2, PlanTypeEnum = PlanTypeEnum.MaintainWeight },

            new AdminSelectedSnack { SnackId = 1, PlanTypeEnum = PlanTypeEnum.MaintainWeight },
            new AdminSelectedSnack { SnackId = 5, PlanTypeEnum = PlanTypeEnum.FutureLeader },
        };

        await _context.AdminSelectedSnacks.AddRangeAsync(adminSelectedSnacks);
        await _context.SaveChangesAsync();
    }

    private static async Task SeedAdminSelectedCarbs(DataContext _context)
    {
        if (await _context.AdminSelectedCarbs.AnyAsync()) return;

        var random = new Random();
        var adminSelectedCarbs = new List<AdminSelectedCarb>
        {
            new AdminSelectedCarb { CarbId = random.Next(1, 7), PlanTypeEnum = PlanTypeEnum.MaintainWeight },
            new AdminSelectedCarb { CarbId = random.Next(1, 7), PlanTypeEnum = PlanTypeEnum.LossWeight },
            new AdminSelectedCarb { CarbId = random.Next(1, 7), PlanTypeEnum = PlanTypeEnum.LossWeight },
            new AdminSelectedCarb { CarbId = random.Next(1, 7), PlanTypeEnum = PlanTypeEnum.MaintainWeight },
            new AdminSelectedCarb { CarbId = random.Next(1, 7), PlanTypeEnum = PlanTypeEnum.MaintainWeight },
            new AdminSelectedCarb { CarbId = random.Next(1, 7), PlanTypeEnum = PlanTypeEnum.FutureLeader },
            new AdminSelectedCarb { CarbId = random.Next(1, 7), PlanTypeEnum = PlanTypeEnum.FutureLeader }
        };

        await _context.AdminSelectedCarbs.AddRangeAsync(adminSelectedCarbs);
        await _context.SaveChangesAsync();
    }

    private static async Task SeedAdminSelectedDrinks(DataContext _context)
    {
        if (await _context.AdminSelectedDrinks.AnyAsync()) return;

        var random = new Random();
        var adminSelectedDrinks = new List<AdminSelectedDrink>
        {
            new AdminSelectedDrink { DrinkId = 1, PlanTypeEnum = PlanTypeEnum.FutureLeader },
            new AdminSelectedDrink { DrinkId = 2, PlanTypeEnum = PlanTypeEnum.FutureLeader },
            new AdminSelectedDrink { DrinkId = 3, PlanTypeEnum = PlanTypeEnum.LossWeight },
            new AdminSelectedDrink { DrinkId = 4, PlanTypeEnum = PlanTypeEnum.LossWeight },
            new AdminSelectedDrink { DrinkId = 5, PlanTypeEnum = PlanTypeEnum.MaintainWeight },
            new AdminSelectedDrink { DrinkId = 6, PlanTypeEnum = PlanTypeEnum.MaintainWeight },
            new AdminSelectedDrink { DrinkId = 7, PlanTypeEnum = PlanTypeEnum.LossWeight }
        };

        await _context.AdminSelectedDrinks.AddRangeAsync(adminSelectedDrinks);
        await _context.SaveChangesAsync();
    }

    private static async Task SeedAdminSelectedExtraOptions(DataContext _context)
    {
        if (await _context.AdminSelectedExtraOptions.AnyAsync()) return;

        var random = new Random();
        var adminSelectedExtraOptions = new List<AdminSelectedExtraOption>
        {
            new AdminSelectedExtraOption
                { ExtraOptionId = 1, PlanTypeEnum = PlanTypeEnum.LossWeight },
            new AdminSelectedExtraOption
                { ExtraOptionId = 2, PlanTypeEnum = PlanTypeEnum.LossWeight },
            new AdminSelectedExtraOption
                { ExtraOptionId = 3, PlanTypeEnum = PlanTypeEnum.FutureLeader },
            new AdminSelectedExtraOption
                { ExtraOptionId = 4, PlanTypeEnum = PlanTypeEnum.MaintainWeight },
            new AdminSelectedExtraOption
                { ExtraOptionId = 1, PlanTypeEnum = PlanTypeEnum.FutureLeader },
            new AdminSelectedExtraOption
                { ExtraOptionId = 5, PlanTypeEnum = PlanTypeEnum.FutureLeader },
            new AdminSelectedExtraOption
                { ExtraOptionId = 7, PlanTypeEnum = PlanTypeEnum.MaintainWeight }
        };

        await _context.AdminSelectedExtraOptions.AddRangeAsync(adminSelectedExtraOptions);
        await _context.SaveChangesAsync();
    }

    private static async Task SeedAdminSelectedMeals(DataContext _context)
    {
        if (await _context.AdminSelectedMeals.AnyAsync()) return;

        var random = new Random();
        var adminSelectedMeals = new List<AdminSelectedMeal>();
        var days = await _context.AdminPlans.ToListAsync();

        int cnt = 1;

        foreach (var day in days)
        {
            day.Meals.Add(new AdminSelectedMeal { MealId = cnt++ });
            if (cnt == 5) cnt = 1;

            day.Meals.Add(new AdminSelectedMeal { MealId = cnt++ });
            if (cnt == 5) cnt = 1;
            day.Meals.Add(new AdminSelectedMeal { MealId = cnt++ });
            if (cnt == 5) cnt = 1;
        }

        await _context.AdminSelectedMeals.AddRangeAsync(adminSelectedMeals);
        await _context.SaveChangesAsync();
    }

    private static async Task SeedOrders(DataContext context, IMapper mapper)
    {
        try
        {
            if (await context.Orders.AnyAsync()) return;
            var users = await context.Users.Where(x => x.IsNormalUser).ToListAsync();
            var meals = await context.Meals.ToListAsync();

            for (int j = 1; j <= 3; j++)
            {
                var items = new List<OrderItem>();

                for (int k = 1; k <= 2; k++)
                {
                    var item = new OrderItem();
                    var mealOrder = mapper.Map<MealItemOrdered>(meals[k]);
                    item.OrderedMeal = mealOrder;
                    item.Quantity = k + j;
                    item.Price = meals[k].Price;
                    items.Add(item);
                }

                foreach (var user in users)
                {
                    var order = new Order
                    {
                        BuyerEmail = user.Email,
                        BuyerPhoneNumber = user.PhoneNumber,
                        Items = items,

                        OrderDate = DateTime.Now,
                        BranchId = j,
                        Subtotal = items.Sum(c => c.Price * c.Quantity),
                        Status = OrderStatus.Pending,
                        PaymentType = PaymentType.Cash,
                        PointsPrice = 0,
                        GainedPoints = 0,
                        BuyerId = user.Id
                    };
                    await context.Orders.AddAsync(order);
                }
            }

            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}