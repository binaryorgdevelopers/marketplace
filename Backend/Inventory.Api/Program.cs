using Inventory.Api.Extensions;
using Marketplace.Application;
using Marketplace.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder
    .AddCustomControllers()
    .RegisterLambda()
    .RegisterMediatR()
    .AddJwt()
    .AddCustomSwagger()
    .AddCustomLogging()
    .AddRedis().Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddSwaggerGen()
    .AddDatabase(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCaching();

app.UseCustomMiddlewares();

app.MapControllers();

app.Run();