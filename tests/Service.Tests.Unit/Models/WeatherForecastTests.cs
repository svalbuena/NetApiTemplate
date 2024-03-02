using NetApiTemplate.Service.Models;

namespace NetApiTemplate.Service.Tests.Unit.Models;

public sealed class WeatherForecastTests
{
    [Theory]
    [InlineData(-100, -147)]
    [InlineData(-40, -39)]
    [InlineData(-5, 24)]
    [InlineData(0, 32)]
    [InlineData(15, 58)]
    [InlineData(36, 96)]
    [InlineData(100, 211)]
    public void CelsiusToFahrenheitConversionIsCorrect(int celsius, int expectedFahrenheit)
    {
        var actualFahrenheit = new WeatherForecast(DateOnly.MinValue, celsius, Summary: string.Empty).TemperatureF;

        actualFahrenheit.ShouldBe(expectedFahrenheit);
    }
}
