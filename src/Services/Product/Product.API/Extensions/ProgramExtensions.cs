#pragma warning disable CA1861

using AWC.Shared.Kernel.Guards;
using Serilog.Sinks.OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

namespace Awc.Services.Product.Product.API.Extentions
{
    public static class ProgramExtensions
    {
        private const string AppName = "Product API Service";

        public static void AddCustomSwagger(this IServiceCollection services) =>
            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo { Title = $"Adventure Works Cycles - {AppName}", Version = "v1" })
            );

        public static void UseCustomSwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{AppName} V1"));
        }

        // public static void AddMediatr(this IServiceCollection services)
        // {
        //     services.AddMediatR(config =>
        //     {
        //         config.RegisterServicesFromAssembly(typeof(Program).Assembly);
        //         config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
        //         config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        //     });
        // }

        // public static void AddMappings(this IServiceCollection services)
        // {
        //     var config = TypeAdapterConfig.GlobalSettings;
        //     config.Scan(ServerAssembly.Instance);
        //     config.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);

        //     services.AddSingleton(config);
        //     services.AddScoped<IMapper, ServiceMapper>();
        // }

        // public static void AddCustomDatabase(this WebApplicationBuilder builder)
        // {
        //     string? connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__AdventureWorksCycles");
        //     Guard.Against.NullOrEmpty(connectionString!);

        //     builder.Services.AddDbContext<CompanyDbContext>(options =>
        //         options.UseSqlServer(
        //             connectionString,
        //             x => x.UseHierarchyId()
        //         )
        //         .EnableSensitiveDataLogging()
        //         .EnableDetailedErrors()
        //     );

        //     builder.Services.AddSingleton<DapperContext>(_ => new DapperContext(connectionString!));

        //     builder.Services.AddScoped<ICompanyService, CompanyService>();

        // }
    }
}