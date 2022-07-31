﻿namespace RestaurantAPI
{
    public class WeatherForecastService : IWeatherForecastService
    {
        public IEnumerable<WeatherForecast> Get(int result, int min, int max)
        {
            return Enumerable.Range(1, result).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(min, max),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
    }
}
