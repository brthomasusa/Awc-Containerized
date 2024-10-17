using MediatR;
using AWC.Shared.Kernel.Utilities;

namespace Awc.Dapr.Services.Company.API.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;

