using MediatR;
using Shared.Models;

namespace Shared.Abstraction.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}