using AsparagusN.Data.Config;
using AsparagusN.Data.Entities;
using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Data.Entities.MealPlan.AdminPlans;
using AsparagusN.Data.Entities.MealPlan.UserPlan;
using AsparagusN.Data.Entities.OrderAggregate;
using AsparagusN.Entities;
using AsparagusN.Entities.Identity;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Entities.OrderAggregate;
using AsparagusN.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AsparagusN.Data;

public class DataContext : IdentityDbContext<AppUser, AppRole, int,
    IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DataContext()
    {
    }

    public DbSet<AdminSelectedSnack> AdminSelectedSnacks { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Meal> Meals { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Allergy> Allergies { get; set; }
    public DbSet<MealIngredient> MealIngredients { get; set; }
    public DbSet<MealAllergy> MealAllergies { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<MediaUrl> MediaUrls { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<AdminSelectedMeal> AdminSelectedMeals { get; set; }
    public DbSet<AdminPlanDay> AdminPlans { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Zone> Zones { get; set; }
    public DbSet<ExtraOption> ExtraOptions { get; set; }
    public DbSet<Drink> Drinks { get; set; }
    public DbSet<AdminSelectedDrink> AdminSelectedDrinks { get; set; }
    public DbSet<AdminSelectedExtraOption> AdminSelectedExtraOptions { get; set; }
    public DbSet<UserPlan> UserPlans { get; set; }
    public DbSet<UserPlanDay> UserPlanDays { get; set; }
    public DbSet<UserSelectedMeal> UserSelectedMeals { get; set; }
    public DbSet<UserSelectedDrink> UserSelectedDrinks { get; set; }
    public DbSet<UserSelectedExtraOption> UserSelectedExtraOptions { get; set; }
    public DbSet<CustomerBasket> CustomerBaskets { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }
    public DbSet<PlanType> PlanTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Drink>().Property(x => x.Volume).HasConversion(x => x.ToString(),
            o => (CapacityLevel)Enum.Parse(typeof(CapacityLevel), o));

        builder.Entity<PlanType>().Property(x => x.PlanTypeE).HasConversion(x => x.ToString(),
            o => (PlanTypeEnum)Enum.Parse(typeof(PlanTypeEnum), o));

        builder.Entity<AppRole>().HasMany(ur => ur.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

        
        builder.Entity<BasketItem>().HasKey(x => new { x.CustomerBasketId, x.MealId });
        builder.Entity<AdminSelectedMeal>().HasKey(x => new { x.AdminPlanDayId, x.MealId });
        builder.Entity<CustomerBasket>().HasMany(x => x.Items).WithOne(c => c.CustomerBasket)
            .HasForeignKey(g => g.CustomerBasketId).OnDelete(DeleteBehavior.Cascade);

        builder.ApplyConfiguration(new DriverConfiguration());
        builder.ApplyConfiguration(new OrderConfiguration());
        builder.ApplyConfiguration(new OrderItemConfiguration());
        builder.ApplyConfiguration(new MealConfiguration());
        builder.ApplyConfiguration(new MealAllergyConfiguration());
        builder.ApplyConfiguration(new MealIngredientConfiguration());
        builder.ApplyConfiguration(new IngredientConfiguration());
        builder.ApplyConfiguration(new BranchConfiguration());
        builder.ApplyConfiguration(new AppUserConfiguration());
        builder.ApplyConfiguration(new AddressConfiguration());
        builder.ApplyConfiguration(new AdminPlanConfiguration());
        builder.ApplyConfiguration(new ExtraOptionsConfiguration());

        var sqlite = Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite";

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var properties = entityType.ClrType.GetProperties()
                .Where(p => p.PropertyType == typeof(decimal));

            foreach (var property in properties)
            {
                if (sqlite)
                    builder.Entity(entityType.Name).Property(property.Name)
                        .HasConversion<double>();
                else
                    builder.Entity(entityType.Name).Property(property.Name)
                        .HasPrecision(38, 12);
            }
        }
    }
}