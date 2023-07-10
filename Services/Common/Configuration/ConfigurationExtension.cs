using Microsoft.AspNetCore.Builder;

namespace Shared.Configuration;

public static class ConfigurationExtension
{
    private static string _env = "dev";

    public static string AddEnvironment(this WebApplicationBuilder builder)
    {
        try
        {
            // var config = new ConfigurationBuilder()
            //     .AddJsonFile("appsettings.json", false)
            //     .Build();
            _env = (builder.Configuration.GetSection("Environment").Value ??
                    Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))!;
            
        }
        catch (Exception)
        {
            _env = "dev";
            Console.WriteLine($"Couldn't read Environment or appsettings.{_env}.json");
        }
        // var app=builder

        return _env;
    }
}