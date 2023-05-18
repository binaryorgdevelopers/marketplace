namespace Ordering.Infrastructure.EventBus.Producers;

public interface IProducer<in TIn, TOut>
{
    Task<TOut> Handle(TIn request);
}