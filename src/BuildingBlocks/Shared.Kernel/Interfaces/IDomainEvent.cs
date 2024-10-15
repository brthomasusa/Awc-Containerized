using MediatR;

namespace AWC.Shared.Kernel.Interfaces;

public interface IDomainEvent : INotification
{
    public Guid Id { get; init; }
}
