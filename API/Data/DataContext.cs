using AsparagusN.Data.Additions;
using AsparagusN.Data.Config;
using AsparagusN.Data.Entities.MealPlan.Admin;
using AsparagusN.Entities;
using AsparagusN.Entities.Identity;
using AsparagusN.Entities.MealPlan;
using AsparagusN.Entities.OrderAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AsparagusN.Data;

public class DataContext : IdentityDbContext<AppUser,AppRole,int,
    IdentityUserClaim<int>,AppUserRole,IdentityUserLogin<int>, IdentityRoleClaim<int>,IdentityUserToken<int>>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }
    public DataContext()
    {
        
    }

    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Order> Orders{ get; set; }
    public DbSet<Meal> Meals { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Allergy> Allergies { get; set; }
    public DbSet<MealIngredient> MealIngredients { get; set; }
    public DbSet<MealAllergy> MealAllergies { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<MediaUrl> MediaUrls { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Drink> Drinks { get; set; }
    public DbSet<AdminSelectedMeal> AdminSelectedMeals { get; set; }
    public DbSet<AdminPlan> AdminPlans { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
       
        base.OnModelCreating(builder);
       
        
        builder.Entity<AppRole>().HasMany(ur => ur.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();
        builder.Entity<DrinkItem>().HasKey(x => new { x.AdminPlanId, x.DrinkId });
        
        
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