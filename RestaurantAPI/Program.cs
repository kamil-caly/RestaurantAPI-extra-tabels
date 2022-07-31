using RestaurantAPI;
using RestaurantAPI.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IWeatherForecastService, WeatherForecastService>(); // przyjmuje 2 typy generyczne
builder.Services.AddDbContext<RestaurantDbContext>();
builder.Services.AddScoped<RestaurantSeeder>();



var app = builder.Build(); /// kazda metoda wywolywana na app to middleware

var scope = app.Services.CreateScope();

/// start seed restaurants
var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
seeder.Seed();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); /// zapytanie automatycznie przekierowane na adres http

/// app.UseAuthorization(); ///

app.MapControllers();

app.Run();
