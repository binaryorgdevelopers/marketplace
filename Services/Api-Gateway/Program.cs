using Authentication.Exceptions;
using Shared.Configuration;
using Shared.Serilog;

var builder = WebApplication.CreateBuilder(args);

var env = builder.AddEnvironment();
Host
    .CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((_, config) => config.AddJsonFile($"appsettings.{env}.json"));

builder
    .AddCustomLogging();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
var app = builder.Build();

app.MapReverseProxy();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapGet("/", () => "Api gateway");


app.Run();