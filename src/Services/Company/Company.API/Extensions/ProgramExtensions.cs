#pragma warning disable CA1861

using Awc.Services.Company.API.Application.Behaviors;
using Awc.Services.Company.API.Endpoints;
using Awc.Services.Company.API.Services;
using AWC.Shared.Kernel.Guards;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Awc.Services.Company.API.Extentions
{
    public static class ProgramExtensions
    {
        private const string AppName = "Company API Service";

        public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
        {
            ServiceDescriptor[] serviceDescriptors = assembly
                .DefinedTypes
                .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                            type.IsAssignableTo(typeof(IEndpoint)))
                .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
                .ToArray();

            services.TryAddEnumerable(serviceDescriptors);

            return services;
        }

        public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
        {
            IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

            IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

            foreach (IEndpoint endpoint in endpoints)
            {
                endpoint.MapEndpoint(builder);
            }

            return app;
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
            string? connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__AdventureWorksCycles");
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
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            // builder.Services.AddMemoryCache();
            // builder.Services.AddSingleton<ICacheService, CacheService>();

        }
    }
}