using System.Text.Json.Serialization;
using AsparagusN.Data;
using AsparagusN.Data.Repositories;
using AsparagusN.Errors;
using AsparagusN.Helpers;
using AsparagusN.Helpers.MappingProfiles;
using AsparagusN.Interfaces;
using AsparagusN.Services;
using AsparagusN.SignalR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace AsparagusN.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IPlanRecommendationService, PlanRecommendationService>();
        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddHostedService<BackgroundTask>();
        services.Configure<KestrelServerOptions>(options => { options.Limits.MaxRequestBodySize = null; });
        services.AddAutoMapper(typeof(CashierProfile),typeof(AppCouponProfile),typeof(BasketProfile),typeof(SnackProfile), typeof(UserPlanProfile), typeof(OrderProfile),
            typeof(DrinkProfile), typeof(DriverProfile), typeof(AdminPlanProfile), typeof(ExtraOptionsProfile),
            typeof(AddressProfile), typeof(BranchProfile), typeof(CategoryProfile), typeof(SomeProfile),
            typeof(UserProfile), typeof(MealProfile),
            typeof(IngredientProfile));
        services.AddSingleton<PresenceTracker>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IMediaService, MediaService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IOrderService, OrderService>();
        //services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddDbContext<DataContext>(opt => { opt.UseSqlServer(config.GetConnectionString("DefaultConnection")); });

      //  services.AddDbContext<DataContext>(opt => { opt.UseSqlite(config.GetConnectionString("DefaultConnection")); });
        services.AddSingleton<IConnectionMultiplexer>(c =>
        {
            var configuration = ConfigurationOptions.Parse(config.GetConnectionString("Redis"), true);
            return ConnectionMultiplexer.Connect(configuration);
        });
        services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
        services.Configure<ApiBehaviorOptions>(opt =>
        {
            opt.InvalidModelStateResponseFactory = actionContext =>
            {
                var errors = actionContext.ModelState.Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();

                foreach (var e in errors)
                {
                    Console.WriteLine(e);
                }
                var errorResponse = new ApiValidationErrorResponse
                {
                    Errors = errors
                };
                return new OkObjectResult(errorResponse);
            };
        });
        return services;
    }
}