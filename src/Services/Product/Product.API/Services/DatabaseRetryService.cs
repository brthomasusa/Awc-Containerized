using Microsoft.Extensions.Options;
using Polly;
using Awc.Services.Product.Product.API.Infrastructure;

namespace Awc.Services.Product.Product.API.Services
{
    public sealed class DatabaseRetryService : IDatabaseRetryService
    {
        private readonly IAsyncPolicy _retryPolicy;
        private readonly IOptions<DatabaseReconnectSettings> _databaseReconnectSettings;
        readonly Serilog.ILogger log = Log.ForContext<DatabaseRetryService>();

        public DatabaseRetryService(IOptions<DatabaseReconnectSettings> settings)
        {
            _databaseReconnectSettings = settings;

            var retryPolicy = Policy
                .Handle<SqlException>()
                .WaitAndRetryAsync(
                    _databaseReconnectSettings.Value.RetryCount,
                    retryAttempt => TimeSpan.FromSeconds(_databaseReconnectSettings.Value.RetryWaitPeriodInSeconds),
                    onRetry: (ex, timeSpan, retryCount, context) =>
                    {
                        Log.Warning(ex, "Failed after maximum retries.: {ErrorMessage}", Helpers.GetInnerExceptionMessage(ex));
                    });

            var fallbackPolicy = Policy
                .Handle<SqlException>()
                .FallbackAsync(
                    fallbackAction: cancellationToken => Task.CompletedTask,
                    onFallbackAsync: async e =>
                    {
                        await Task.Run(() => Log.Error(e, "Failed after maximum retries.: {ErrorMessage}", Helpers.GetInnerExceptionMessage(e)));
                    });

            _retryPolicy = Policy.WrapAsync(fallbackPolicy, retryPolicy);
        }

        public async Task ExecuteWithRetryAsync(Func<Task> action)
        {
            var context = new Context();

            int attempt = 0;
            await _retryPolicy.ExecuteAsync(async (ctx) =>
            {
                attempt++;
                await action();
            }, context);
            Log.Information("Connection successfully reconnected at attempt {Attempt} at {CurrentTime}", attempt, DateTime.Now);
        }
    }
}