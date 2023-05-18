
namespace Shared.Abstraction.MediatR;

public interface IRequestHandler<in TRequest, TResult>
{
    ValueTask<TResult> HandleAsync(TRequest request, CancellationToken cancellationToken);
}