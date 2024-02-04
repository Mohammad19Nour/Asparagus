using System.Text.Json.Serialization;
using AsparagusN.Data;
using AsparagusN.Entities.Identity;
using AsparagusN.Extensions;
using AsparagusN.Helpers;
using AsparagusN.Middleware;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new RoundedNumberConverter(2));
    }); /*
    builder.Services.AddMvc().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        // Other options as needed
    });*/
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
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var roleContext = services.GetRequiredService<RoleManager<AppRole>>();
    await Seed.SeedData(context,roleContext);
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