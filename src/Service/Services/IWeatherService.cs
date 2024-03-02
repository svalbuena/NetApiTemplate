using NetApiTemplate.Service.Models;

namespace NetApiTemplate.Service.Services;

public interface IWeatherService
{
    IEnumerable<WeatherForecast> GetWeatherForecast(CancellationToken cancellationToken = default);
}
