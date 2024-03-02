using NetApiTemplate.Service.Tests.Integration.TestHelpers;

namespace NetApiTemplate.Service.Tests.Integration.EndToEnd;

public sealed class WeatherForecastTests : IClassFixture<RealDependenciesServiceFactory>
{
    private readonly RealDependenciesServiceFactory _factory;

    public WeatherForecastTests(RealDependenciesServiceFactory factory) => _factory = factory;

    [Fact]
    public async Task GetWeatherForecastWorks()
    {
        var client = _factory.CreateApiClient();

        var response = await client.GetWeatherForecastAsync();

        response.Forecasts.ShouldNotBeEmpty();
    }
}
