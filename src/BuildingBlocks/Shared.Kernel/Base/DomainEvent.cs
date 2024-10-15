#pragma warning disable S2094

using MediatR;
using AWC.Shared.Kernel.Interfaces;

namespace AWC.Shared.Kernel.Base;

public abstract record DomainEvent(Guid Id) : IDomainEvent;
