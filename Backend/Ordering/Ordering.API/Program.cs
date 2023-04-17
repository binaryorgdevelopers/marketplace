using Ordering.API.Extensions;
using Ordering.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder
    .AddCustomMvc()
    .Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .RegisterMassTransitServices();

builder.AddAutofacModules();

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
ServiceRegistrationExtensions.ConfigureEventBus(app);

public partial class Program
{
    public static string Namespace = typeof(Program).Namespace;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}