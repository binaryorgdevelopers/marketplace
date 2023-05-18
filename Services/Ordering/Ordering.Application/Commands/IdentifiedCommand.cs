using MediatR;

namespace Ordering.Application.Commands;

public class IdentifiedCommand<T, R> : IRequest<R> where T : IRequest<R>
{
    public T Command { get; }
    public Guid Id { get; }

    public IdentifiedCommand(Guid id, T command)
    {
        Id = id;
        Command = command;
    }
}