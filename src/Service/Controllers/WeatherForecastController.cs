using Microsoft.AspNetCore.Mvc;
using NetApiTemplate.Api.Server;
using NetApiTemplate.Service.Services;

namespace NetApiTemplate.Service.Controllers;

[ApiController]
public sealed class WeatherForecastController : WeatherForecastControllerBase
{
    private readonly IWeatherService _weatherService;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(IWeatherService weatherService, ILogger<WeatherForecastController> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }

    public override async Task<ActionResult<GetWeatherForecastResponse>> GetWeatherForecast(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting weather forecast");
        var forecasts = _weatherService.GetWeatherForecast(cancellationToken);
        var response = new GetWeatherForecastResponse
        {
            Forecasts = forecasts.Select(forecast => new WeatherForecast
            {
                Date = forecast.Date.ToDateTime(TimeOnly.MinValue),
                TemperatureC = forecast.TemperatureC,
                TemperatureF = 32 + (int)(forecast.TemperatureC / 0.5556),
                Summary = forecast.Summary
            })
        };
        return await Task.FromResult(response);
    }
}
