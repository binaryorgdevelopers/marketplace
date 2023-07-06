using Authentication.Extensions;
using Identity;
using Identity.gRPC;
using Shared.Extensions;
using Shared.Redis;
using Shared.Serilog;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddCustomGrpcPorts()
    .AddServices()
    .AddCustomLogging()
    .AddRepositories()
    .AddRedis()
    .AddCustomDbContext()
    .AddCustomAuthentication()
    .AddCustomGrpcServer<AuthGrpcService>()
    .AddOptions();
builder.Services
    .AddEndpointsApiExplorer()
    .AddControllers();


var app = builder.Build();

app.UseHttpsRedirection();
app.UseCustomMiddlewares();
app.UseAuthorization();
app.MapControllers();
app.MapGrpcService<AuthGrpcService>();

app.Run();