using MediatR;
using AWC.Shared.Kernel.Utilities;

namespace Awc.Services.Product.Product.API.Application.Abstractions.Messaging
{
    public interface ICommand : IRequest<Result>;

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
}