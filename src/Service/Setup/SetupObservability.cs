using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Serilog;
using Serilog.Events;

namespace NetApiTemplate.Service.Setup;

[ExcludeFromCodeCoverage]
internal static class ObservabilitySetup
{
    public static WebApplicationBuilder ConfigureObservability(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration.GetSection("Observability").Get<ObservabilityConfiguration>() ?? new ObservabilityConfiguration();
        builder
            .ConfigureLogging(configuration);
        builder.Services
            .AddHealthChecks();
        return builder;
    }

    public static WebApplication UseObservability(this WebApplication app)
    {
        app
            .UseProblemDetails()
            // https://github.com/serilog/serilog-aspnetcore#request-logging
            .UseSerilogRequestLogging();
        app
            .MapHealthChecks("/healthz");
        return app;
    }

    private static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder, ObservabilityConfiguration configuration)
    {
        Log.Logger = ConfigureLogger(configuration);

        builder.Logging
            .ClearProviders()
            .AddSerilog();

        builder.Host.UseSerilog(Log.Logger);

        builder.Services
            .AddProblemDetails(options =>
            {
                // Unhandled exceptions are already logged by Serilog.AspNetCore.RequestLoggingMiddleware
                options.ShouldLogUnhandledException = (context, exception, _) => false;
                // Exclude exception details on production
                options.IncludeExceptionDetails = (_, _) => !builder.Environment.IsProduction();

                // This kind of exception is sometimes thrown by the swagger/openapi validators and needs to be handled properly for consistent errors
                options.MapToStatusCode<BadHttpRequestException>(StatusCodes.Status400BadRequest);
                // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
                // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
                options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
            })
            .AddProblemDetailsConventions();

        return builder;
    }

    private static Serilog.Core.Logger ConfigureLogger(ObservabilityConfiguration observabilityConfig) =>
        new LoggerConfiguration()
            .MinimumLevel.Is(observabilityConfig.LogLevel)
            // https://github.com/serilog/serilog-aspnetcore#request-logging
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .Enrich.WithProperty("Application", observabilityConfig.ServiceName)
            .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception} {Properties:j}{NewLine}", formatProvider: CultureInfo.InvariantCulture)
            .CreateLogger();
}
