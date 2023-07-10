using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Shared.Configuration;

public static class ConfigurationExtension
{
    private static string _env = "dev";

    public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
    {
        try
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();
            _env = (config.GetSection("Environment").Value ??
                    Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))!;
            
        }
        catch (Exception)
        {
            _env = "dev";
            Console.WriteLine($"Couldn't read Environment or appsettings.{_env}.json");
        }
        // var app=builder

        return builder;
    }
}