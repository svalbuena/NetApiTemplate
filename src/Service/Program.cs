using NetApiTemplate.Api.Server;
using NetApiTemplate.Service.Controllers;
using NetApiTemplate.Service.Services;
using NetApiTemplate.Service.Setup;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureObservability();
builder.Services
    .AddSingleton<IWeatherService, WeatherService>()
    .AddSingleton<WeatherForecastControllerBase, WeatherForecastController>()
    .AddControllers();

var app = builder.Build();
app
    .UseObservability()
    .UseSwaggerUi()
    .UseStaticFiles()
    .UseHttpsRedirection()
    .UseAuthorization();
app.MapControllers();

app.Run();


// Make the implicit Program class public so test projects can access it, we need to use the partial modifier to modify the existing implicit Program class
public sealed partial class Program { }
