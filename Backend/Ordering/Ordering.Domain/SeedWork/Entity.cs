using MediatR;

namespace Ordering.Domain.SeedWork;

public abstract class Entity
{
    private int? _requestedHashCode;

    public virtual int Id { get; protected set; }

    private List<INotification> _domainEvents;

    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(INotification eventItem)
    {
        _domainEvents = _domainEvents ?? new List<INotification>();
        _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(INotification eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }

    public void ClearDomainEvents() => _domainEvents.Clear();

    public bool IsTransient() => Id == default;

    public override int GetHashCode()
    {
        if (IsTransient())
        {
            _requestedHashCode = this.Id.GetHashCode() ^ 31;
            return _requestedHashCode.Value;
        }
        else
            return base.GetHashCode();
    }

    public static bool operator ==(Entity left, Entity right)
    {
        if (Object.Equals(left, null))
            return (Object.Equals(right, null)) ? true : false;
        else
            return left.Equals(right);
    }

    public static bool operator !=(Entity? left, Entity? right)
    {
        return !(left == right);
    }
}