using AWC.Shared.Kernel.Interfaces;

namespace AWC.Shared.Kernel.Base;

public abstract class AggregateRoot<T> : Entity<T>, IAggregateRoot
{
    private readonly List<DomainEvent> _domainEvents = [];

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents!.AsReadOnly();

    public void Raise(DomainEvent eventItem)
    {
        _domainEvents.Add(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
