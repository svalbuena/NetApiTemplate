using System.Security.Cryptography;
using NetApiTemplate.Service.Models;

namespace NetApiTemplate.Service.Services;

internal sealed class WeatherService : IWeatherService
{
    private static readonly string[] Summaries =
        [
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching"
        ];

    public IEnumerable<WeatherForecast> GetWeatherForecast(CancellationToken cancellationToken = default) =>
        Enumerable
            .Range(1, 5)
            .Select(index =>
                new WeatherForecast
                (
                    Date: DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC: RandomNumberGenerator.GetInt32(-20, 55),
                    Summary: Summaries[RandomNumberGenerator.GetInt32(Summaries.Length)]
                )
            );
}
