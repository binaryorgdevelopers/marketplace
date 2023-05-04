using Autofac;
using EventBus.Abstractions;
using Ordering.Application.Commands;
using Ordering.Application.Queries;
using Ordering.Domain.Abstractions;
using Ordering.Domain.AggregatesModel.BuyerAggregate;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Infrastructure.Idempotency;
using Ordering.Infrastructure.Repositories;
using Module = Autofac.Module;

namespace Ordering.Infrastructure;

public class ApplicationModules : Module
{
    public string QueryConnectionString { get; }

    public ApplicationModules(string connectionString)
    {
        QueryConnectionString = connectionString;
    }

    protected override void Load(ContainerBuilder builder)
    {
        // builder.Register(c => new OrderQueries(QueryConnectionString))
        //     .As<IOrderQueries>()
        //     .InstancePerLifetimeScope();

        builder.RegisterType<BuyerRepository>()
            .As<IBuyerRepository>()
            .InstancePerLifetimeScope();

        builder.RegisterType<OrderRepository>()
            .As<IOrderRepository>()
            .InstancePerLifetimeScope();

        builder.RegisterType<RequestManager>()
            .As<IRequestManager>()
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(typeof(CreateOrderCommandHandler).GetType().Assembly)
            .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        base.Load(builder);
    }
}