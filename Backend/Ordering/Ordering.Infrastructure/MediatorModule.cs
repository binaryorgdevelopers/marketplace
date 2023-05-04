using System.Reflection;
using Autofac;
using FluentValidation;
using MediatR;
using Ordering.Application.Behaviours;
using Ordering.Application.Commands;
using Ordering.Application.DomainEventHandlers.OrderStartedEvent;
using Ordering.Application.Validations;

namespace Ordering.Infrastructure;

public class MediatorModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();

        //Register all Command classes (they implement IRequestHandler)
        builder.RegisterAssemblyTypes(typeof(CreateOrderCommand).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>));

        //Register the DomainEventHandler classes (they implement INotificationHandler<>) in assembly holding the Domain Events
        builder.RegisterAssemblyTypes(typeof(ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler)
                .GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(INotificationHandler<>));

        // Register the Command's Validators (Validators based on FluentValidation library)
        builder
            .RegisterAssemblyTypes(typeof(CreateOrderCommandValidator).GetTypeInfo().Assembly)
            .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
            .AsImplementedInterfaces();

        builder.RegisterGeneric(typeof(LoggingBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
        builder.RegisterGeneric(typeof(ValidationBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
        // builder.RegisterGeneric(typeof(TransactionBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
    }
}