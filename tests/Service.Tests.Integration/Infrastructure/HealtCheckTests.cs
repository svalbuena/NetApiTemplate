using System.Net;
using NetApiTemplate.Service.Tests.Integration.TestHelpers;

namespace NetApiTemplate.Service.Tests.Integration.Infrastructure;

public sealed class HealthCheckTests : IClassFixture<RealDependenciesServiceFactory>
{
    private readonly RealDependenciesServiceFactory _factory;

    public HealthCheckTests(RealDependenciesServiceFactory factory) => _factory = factory;

    [Fact]
    public async Task HealthCheckRespondsOk()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(new Uri("/healthz", UriKind.Relative));

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync()).ShouldBe("Healthy");
    }
}
