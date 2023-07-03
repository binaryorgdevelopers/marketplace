using Authentication.Extensions;
using Marketplace.Application;
using Marketplace.Infrastructure;
using Shared.Extensions;
using Identity.Services;
using Inventory.Api;
using Shared.Swagger;

var builder = WebApplication.CreateBuilder(args);


builder
    .AddCustomGrpcPorts()
    .AddCustomControllers()
    .AddCustomGrpcClient<AuthService.AuthServiceClient>()
    .RegisterLambda()
    .RegisterMediatR()
    .AddCustomSwagger()
    .AddRedis()
    .AddServices()
    .Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddDatabase(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCaching();
app.UseCustomMiddlewares();
app.MapControllers();

app.Run();