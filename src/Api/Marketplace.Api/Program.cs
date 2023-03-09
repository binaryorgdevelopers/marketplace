using Marketplace.Api.Extensions;
using Marketplace.Application;
using Marketplace.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder
    .RegisterLambda()
    .RegisterMediatR()
    .AddJwt()
    .AddCustomSwagger()
    .AddCustomLogging()
    .AddRedis().Services
    .AddInfrastructure()
    .AddApplication()
    .AddSwaggerGen()
    .AddDatabase(builder.Environment.IsDevelopment(), builder.Configuration);


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