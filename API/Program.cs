using System.Text.Json.Serialization;
using AsparagusN.Data;
using AsparagusN.Data.Entities.Identity;
using AsparagusN.Extensions;
using AsparagusN.Helpers;
using AsparagusN.Helpers.MappingProfiles;
using AsparagusN.Interfaces;
using AsparagusN.Middleware;
using AsparagusN.SignalR;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5257", "http://*:5257");
// Add services to the container.
/*
string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "log.txt");
var logger = new LoggerConfiguration()
    .MinimumLevel.Information() 
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
*/
builder.Services.AddControllers().AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.Converters.Add(new RoundedNumberConverter(2));
    opt.SerializerSettings.Converters.Add(new PictureUrlConverter(builder.Configuration["ApiUrl"]));
    opt.SerializerSettings.Converters.Add(new TimeSpanConverter());
    opt.SerializerSettings.Converters.Add(new DateTimeConverter());
    //   opt.JsonSerializerOptions.Converters.Add();
});
builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed((_) => true);
}));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);
builder.Services.AddSwaggerAuthorization();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("hubs/presence");
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var roleContext = services.GetRequiredService<RoleManager<AppRole>>();
    var userContext = services.GetRequiredService<UserManager<AppUser>>();
    var mapper = services.GetRequiredService<IMapper>();
    var subscriptionService = services.GetRequiredService<ISubscriptionService>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context, roleContext,userContext,subscriptionService,mapper);
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed((_) => true));

app.Run();