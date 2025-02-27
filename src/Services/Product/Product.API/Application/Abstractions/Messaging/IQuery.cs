using MediatR;
using AWC.Shared.Kernel.Utilities;

namespace Awc.Services.Product.Product.API.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;

