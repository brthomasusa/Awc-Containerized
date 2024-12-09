#pragma warning disable CA1861

using Awc.Services.Company.API.Application.Behaviors;
using Awc.Services.Company.API.Services;
using AWC.Shared.Kernel.Guards;
using Serilog.Sinks.OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

namespace Awc.Services.Company.API.Extentions
{
    public static class ProgramExtensions
    {
        private const string AppName = "Company API Service";
        private const string serviceName = "companyApi";

        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                .WriteTo.OpenTelemetry(options => {
                    options.Endpoint = "http://localhost:4317";
                    options.Protocol = OtlpProtocol.Grpc; 
                    options.ResourceAttributes = new Dictionary<string, object>
                    {
                        ["service.name"] = serviceName,
                        ["index"] = 10,
                        ["flag"] = true,
                        ["value"] = 3.14
                    };                   
                })
                .ReadFrom.Configuration(ctx.Configuration));
        }

        public static void ConfigureOpenTelemetry(this IServiceCollection services)
        {
            services.AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService(serviceName))
                .WithMetrics(metrics =>
                    metrics
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddOtlpExporter()
                )
                .WithTracing(tracing =>
                    tracing
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddSqlClientInstrumentation()
                        .AddEntityFrameworkCoreInstrumentation()
                        .AddConsoleExporter()
                        .AddOtlpExporter(opts => opts.Endpoint = new Uri("http://localhost:4317"))
                );            
        }

        public static void ConfigureHealthChecks(this IServiceCollection services)
        {
            string? connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__CompanyDb");
            Guard.Against.NullOrEmpty(connectionString!);

            services.AddHealthChecks()
                .AddCheck("Company API", () => HealthCheckResult.Healthy(), tags: new[] { "live" })
                .AddSqlServer(
                    connectionString!,
                    healthQuery: "select 1",
                    name: "Company API database-check",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] { "ready" }
                );
        }


        public static void AddCustomSwagger(this IServiceCollection services) =>
            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo { Title = $"Adventure Works Cycles - {AppName}", Version = "v1" })
            );

        public static void UseCustomSwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{AppName} V1"));
        }

        public static void AddMediatr(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(Program).Assembly);
                config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
                config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            });
        }

        public static void AddMappings(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(ServerAssembly.Instance);
            config.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
        }

        public static void AddCustomDatabase(this WebApplicationBuilder builder)
        {
            string? connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__CompanyDb");
            Guard.Against.NullOrEmpty(connectionString!);

            builder.Services.AddDbContext<CompanyDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    x => x.UseHierarchyId()
                )
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
            );

            builder.Services.AddSingleton<DapperContext>(_ => new DapperContext(connectionString!));

            builder.Services.AddScoped<ICompanyService, CompanyService>();

            // builder.Services.AddMemoryCache();
            // builder.Services.AddSingleton<ICacheService, CacheService>();

        }
    }
}