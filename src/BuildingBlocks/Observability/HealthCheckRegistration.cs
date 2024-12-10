using Awc.BuildingBlocks.Observability.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Awc.BuildingBlocks.Observability
{
    public static class HealthCheckRegistration
    {
        public static void ConfigureHealthChecks(this IServiceCollection services, ObservabilityOptions observabilityOptions)
        {
            services.AddHealthChecks()
                .AddCheck(observabilityOptions.ServiceName, () => HealthCheckResult.Healthy(), tags: ["live"])
                .AddSqlServer(
                    observabilityOptions.DbConnectionString,
                    healthQuery: "select 1",
                    name: $"{observabilityOptions.ServiceName} database-check",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: ["ready"]
                );
        }        
    }
}