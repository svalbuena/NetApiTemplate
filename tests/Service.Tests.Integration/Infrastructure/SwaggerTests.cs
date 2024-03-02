using System.Net;
using NetApiTemplate.Service.Tests.Integration.TestHelpers;

namespace NetApiTemplate.Service.Tests.Integration.Infrastructure;

public sealed class SwaggerTests : IClassFixture<RealDependenciesServiceFactory>
{
    private readonly RealDependenciesServiceFactory _factory;

    public SwaggerTests(RealDependenciesServiceFactory factory) => _factory = factory;

    [Fact]
    public async Task SwaggerUiRespondsOk()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync(new Uri("/swagger/index.html", UriKind.Relative));

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
