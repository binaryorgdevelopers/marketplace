using FluentValidation;
using Marketplace.Application.Abstractions.Messaging;
using Marketplace.Application.Commands.Command.Authentication;
using Marketplace.Application.Common.Messages.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblies(assembly));

        services.AddValidatorsFromAssembly(assembly);
        return services;
    }
}