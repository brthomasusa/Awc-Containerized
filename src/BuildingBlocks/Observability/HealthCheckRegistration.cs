using Awc.BuildingBlocks.Observability.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Awc.BuildingBlocks.Observability
{
    public static class HealthCheckRegistration
    {
        public static void AddHealthChecks(this IServiceCollection services, ObservabilityOptions observabilityOptions)
        {
            services.AddHealthChecks()
                // Add a health check for a SQL Server database
                .AddCheck(
                    $"{observabilityOptions.ServiceName} database-check",
                    new SqlConnectionHealthCheck(observabilityOptions.DbConnectionString),
                    HealthStatus.Unhealthy,
                    ["companydb"]);            
        }        
    }
}