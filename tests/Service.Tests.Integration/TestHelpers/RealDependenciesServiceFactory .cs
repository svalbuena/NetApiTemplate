using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using NetApiTemplate.Api.Client;

namespace NetApiTemplate.Service.Tests.Integration.TestHelpers;

public sealed class RealDependenciesServiceFactory : WebApplicationFactory<Program>
{
    // The classes that contain test must be public. This class is used in the test classes constructor, so it needs to be public as well to maintain visibility consistency.
    // Since it's a public method, the code analysis interpret this might be invoked as a remote interface where nullability checks cannot be applied.
    // [NotNull] annotation is used to avoid a warning about builder being potentially null, as we never expose this method in a way that it could be null.
    protected override void ConfigureWebHost([NotNull] IWebHostBuilder builder) => builder.UseEnvironment(Environments.Development);

    public INetApiTemplateClient CreateApiClient() =>
        new NetApiTemplateClient(CreateClient())
        {
            // True so that test failures show ProblemDetails message on the test output
            ReadResponseAsString = true
        };
}
