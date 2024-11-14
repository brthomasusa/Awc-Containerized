#pragma warning disable CA1861

using Awc.Services.Company.API.Application.Behaviors;
using Awc.Services.Company.API.Services;
using AWC.Shared.Kernel.Guards;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Awc.Services.Company.API
{
    public static class ProgramExtensions
    {
        private const string AppName = "Company API Service";

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


        public static void AddCustomSwagger(this WebApplicationBuilder builder) =>
            builder.Services.AddSwaggerGen(c =>
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

        public static void AddPersistence(this IServiceCollection services)
        {
            string? connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__CompanyApi");
            Guard.Against.NullOrEmpty(connectionString!);

            services.AddDbContext<CompanyDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    x => x.UseHierarchyId()
                )
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
            );

            services.AddSingleton<DapperContext>(_ => new DapperContext(connectionString!));
            services.AddScoped<ICompanyService, CompanyService>();

            // services.AddScoped<ICompanyService>(sp =>
            //     sp.GetRequiredService<CompanyService>());

            // services.AddMemoryCache();
            // services.AddSingleton<ICacheService, CacheService>();
        }
    }
}