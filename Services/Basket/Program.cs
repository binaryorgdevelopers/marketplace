using Authentication.Extensions;
using Basket;
using Identity.Services;
using Shared.Extensions;
using Shared.Redis;
using Shared.Serilog;
using Shared.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddServices()
    .AddCustomLogging()
    .AddCustomGrpcClient<AuthService.AuthServiceClient>()
    .AddRedis()
    .AddCustomSwagger()
    .Services
    .AddEndpointsApiExplorer()
    .AddControllers();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCustomMiddlewares();
app.MapControllers();

app.Run();