using System.Diagnostics.CodeAnalysis;
using Serilog.Events;

namespace NetApiTemplate.Service.Setup;

[ExcludeFromCodeCoverage]
internal sealed record ObservabilityConfiguration
{
    public string ServiceName { get; set; } = "unknown";
    public LogEventLevel LogLevel { get; set; } = LogEventLevel.Information;
}
