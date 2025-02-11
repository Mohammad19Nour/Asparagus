﻿using System.Text.Json.Serialization;
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
        services.AddScoped<ICustomSubscriptionService, CustomSubscriptionService>();
        services.AddScoped<IUserPlanOrderService, UserPlanOrderService>();
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IStatisticService, StatisticService>();
        services.AddScoped<IPlanRecommendationService, PlanRecommendationService>();
        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddHostedService<BackgroundGiftService>();
        services.AddHostedService<BackgroundTask>();
        services.Configure<KestrelServerOptions>(options => { options.Limits.MaxRequestBodySize = null; });
        services.AddAutoMapper(typeof(EmployeesProfile),typeof(SubscriptionProfile),typeof(BundleProfile), typeof(CarProfile), typeof(PackageProfile), typeof(ReportProfile),
            typeof(CashierProfile), typeof(AppCouponProfile), typeof(BasketProfile), typeof(SnackProfile),
            typeof(UserPlanProfile), typeof(OrderProfile),
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
        //services.AddDbContext<DataContext>(opt => { opt.UseSqlite(config.GetConnectionString("SqliteConnection")); });
       
        services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
        services.Configure<ApiBehaviorOptions>(opt =>
        {
            opt.InvalidModelStateResponseFactory = actionContext =>
            {
                var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors.Select(y => new 
                    {
                        Field = x.Key,
                        Message = y.ErrorMessage
                    }))
                    .ToArray();

                var tmp = errors.Select(c => $"{c.Field}: {c.Message}").ToArray();
                var re = string.Join(", ", tmp);
                
                var errorResponse = new 
                {
                    Errors = errors
                };

                return new OkObjectResult(new ApiResponse(400,re));
            };
        });
        return services;
    }
}