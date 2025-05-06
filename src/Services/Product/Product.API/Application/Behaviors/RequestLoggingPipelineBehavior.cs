using MediatR;
using Serilog.Context;

namespace Awc.Services.Product.Product.API.Application.Behaviors
{
    internal sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>(ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class
        where TResponse : Result
    {
        private readonly ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> _logger = logger;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            string requestName = typeof(TRequest).Name;

            _logger.LogInformation("Processing request {RequestName}", requestName);

            TResponse result = await next();

            if (result.IsSuccess)
            {
                _logger.LogInformation("Completed request {RequestName}", requestName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    if (result.Error.Message.Contains("Not Found:", StringComparison.OrdinalIgnoreCase))
                        _logger.LogWarning("Completed request {RequestName} with warning: {ErrorMessage} ", requestName, result.Error.Message);
                    else
                        _logger.LogError("Completed request {RequestName} with error: {ErrorMessage} ", requestName, result.Error.Message);
                }
            }

            return result;
        }
    }
}