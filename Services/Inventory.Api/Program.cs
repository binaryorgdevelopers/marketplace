using Authentication.Exceptions;
using Inventory.Api.Extensions;
using Inventory.Api.Middleware;
using Marketplace.Application;
using Marketplace.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Shared.Extensions;
using AuthService = Inventory.Api.Authentication.AuthService;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    var ports = GetDefinedPorts(builder.Configuration);
    options.ListenLocalhost(ports.httpPort,
        listenOptions => { listenOptions.Protocols = HttpProtocols.Http1AndHttp2; });
    options.ListenLocalhost(ports.grpcPort, o => o.Protocols =
        HttpProtocols.Http2);
});

builder
    .AddCustomControllers()
    .RegisterLambda()
    .RegisterMediatR()
    .AddCustomAuthentication()
    .AddCustomSwagger()
    .AddCustomLogging()
    .AddRedis()
    .Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddSwaggerGen()
    .AddDatabase(builder.Configuration)
    .AddCustomGrpcServer<AuthService>();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCaching();
app.MapGrpcService<AuthService>();

app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapControllers();

app.Run();

(int httpPort, int grpcPort) GetDefinedPorts(IConfiguration configuration)
{
    var grpcPort = Convert.ToInt32(configuration.GetValue<string>("Grpc:PORT"));
    var port = configuration.GetValue("PORT", 2000);
    return (port, grpcPort);
}