using System.Data.Common;

namespace Awc.Services.Company.API.Infrastructure
{
    public class SqlConnectionHealthCheck(string connectionString) : IHealthCheck
    {
        public string ConnectionString { get; } = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        public string TestQuery { get; } = "SELECT 1";

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            await using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    await connection.OpenAsync(cancellationToken);

                    if (TestQuery != null)
                    {
                        var command = connection.CreateCommand();
                        command.CommandText = TestQuery;

                        await command.ExecuteNonQueryAsync(cancellationToken);
                    }
                }
                catch (DbException ex)
                {
                    return new HealthCheckResult(status: context.Registration.FailureStatus, exception: ex);
                }
            }

            return HealthCheckResult.Healthy();
        }
    }
}