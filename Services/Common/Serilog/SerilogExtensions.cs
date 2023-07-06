using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Shared.Serilog;

public static partial class Extensions
{
    public static WebApplicationBuilder AddCustomLogging(this WebApplicationBuilder builder)
    {
        builder
            .Logging
            .ClearProviders();

        var logger = new LoggerConfiguration()
            .ReadFrom
            .Configuration(builder.Configuration)
            .CreateLogger();

        builder
            .Host
            .UseSerilog(logger);
        return builder;
    }
}