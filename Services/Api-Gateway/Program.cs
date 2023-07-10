using Authentication.Exceptions;
using Authentication.Extensions;
using Shared.Serilog;
using Shared.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddCustomLogging();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
var app = builder.Build();

app.MapReverseProxy();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapGet("/", () => "Api gateway");


app.Run();