using Basket;
// using Discount.gRPC.Protos;
using Shared.Extensions.gRPC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder
    .AddServices()
    .AddRedis()
    .Services
    // .AddCustomGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(builder.Configuration)
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers();
builder.Services
    .AddAuthentication()
    .AddJwtBearer("Bearer", config =>
    {
        config.Authority = "http://localhost:1111/";

        config.Audience = "Inventory";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();