using MediatR;
using AWC.Shared.Kernel.Utilities;

namespace Awc.Services.Product.Product.API.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
