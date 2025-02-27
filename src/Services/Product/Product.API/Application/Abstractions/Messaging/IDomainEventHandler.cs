using MediatR;
using AWC.Shared.Kernel.Interfaces;

namespace Awc.Services.Product.Product.API.Application.Abstractions.Messaging;

public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent;
