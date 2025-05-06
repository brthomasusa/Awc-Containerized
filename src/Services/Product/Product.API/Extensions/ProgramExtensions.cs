#pragma warning disable CA1861

using System.Reflection;
using Awc.Services.Product.Product.API.Application.Behaviors;
using Awc.Services.Product.Product.API.Endpoints;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Awc.Services.Product.Product.API.Extentions
{
    public static class ProgramExtensions
    {
        private const string AppName = "Product API Service";

        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                );
            });

        public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
        {
            ServiceDescriptor[] serviceDescriptors = [.. assembly
                .DefinedTypes
                .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                            type.IsAssignableTo(typeof(IEndpoint)))
                .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))];

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

        public static void AddCustomDatabase(this WebApplicationBuilder builder, string? connectionString)
        {
            builder.Services.AddSingleton<DapperContext>(_ => new DapperContext(connectionString!));

            builder.Services.AddScoped<IProductService, ProductService>();
        }
    }
}