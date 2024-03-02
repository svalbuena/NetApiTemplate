using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Net.Http.Headers;

namespace NetApiTemplate.Service.Setup;

[ExcludeFromCodeCoverage]
internal static class SwaggerSetup
{
    private const string ApiSpecRoute = "swagger";
    private const string ApiSpecFilename = "api-spec.json";

    public static IApplicationBuilder UseSwaggerUi(this WebApplication app)
    {
        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = ApiSpecRoute;
            options.SwaggerEndpoint(ApiSpecFilename, "v1");
            options.DocumentTitle = "NetApiTemplate Service";
        });
        // Expose the OpenAPI JSON that is copied into the build folder from the NSwagDefinition project reference
        app.MapGet(
            $"/{ApiSpecRoute}/{ApiSpecFilename}",
            async httpContext =>
            {
                // This ensures that the JSON definition file is never cached and so we always get fresh content
                // (whether to display the UI or when someone requests the file directly)
                httpContext.Response.Headers[HeaderNames.CacheControl] = CacheControlHeaderValue.NoCacheString;
                var fullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!, ApiSpecFilename);
                using var fileStream = File.OpenRead(fullPath);
                await fileStream.CopyToAsync(httpContext.Response.Body);
            }
        );
        app.MapGet("/", () => Results.Redirect($"/{ApiSpecRoute}", permanent: true));
        return app;
    }
}
