using Basket;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddServices()
    .AddRedis()
    .Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers();
AddCustomLogging(builder);

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://localhost:1111";
        options.Audience = "basket";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new 
            TokenValidationParameters
            {
                ValidateAudience = false
            };
    });

// .AddCookie()
// .AddOpenIdConnect(options =>
// {
//     options.Authority = "http://localhost:1111"; // URL of the identity server
//     options.ClientId = "basket";
//     options.ClientSecret = "basket";
//     options.ResponseType = "code";
//     options.Scope.Add("basket");
//     options.RequireHttpsMetadata = false;
// });

builder.Services.AddHttpClient();

var app = builder.Build();

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

void AddCustomLogging(WebApplicationBuilder builder)
{
    builder.Logging.ClearProviders();
    var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
    builder.Host.UseSerilog(logger);
}