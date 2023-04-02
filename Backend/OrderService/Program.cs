using OrderService.Services;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddKafka(builder.Configuration)
    .AddHostedService<ConsumerService>();

var app = builder.Build();


app.MapGet("/", () => "Hello World!");

app.Run();