using Basket;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder
    .AddServices()
    .AddGrpc()
    .AddRedis()
    .Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();