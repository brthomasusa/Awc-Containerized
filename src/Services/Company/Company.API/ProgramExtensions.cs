using Awc.Services.Company.API.Application.Behaviors;
using Awc.Services.Company.API.Services;
using AWC.Shared.Kernel.Guards;

namespace Awc.Services.Company.API
{
    public static class ProgramExtensions
    {
        private const string AppName = "Company API Service";
        private static readonly string[] tagsArray = ["Feedback", "Company API Db"];
    
        public static void ConfigureHealthChecks(this IServiceCollection services)
        {
            string? connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__CompanyApi");
            Guard.Against.NullOrEmpty(connectionString!);

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddSqlServer(
                    connectionString!, 
                    healthQuery: "select 1", 
                    name: "Company API Db-check", 
                    failureStatus: HealthStatus.Unhealthy, 
                    tags: tagsArray
                );

            services.AddHealthChecksUI(opt =>
            {
                opt.SetEvaluationTimeInSeconds(10); //time in seconds between check    
                opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks    
                opt.SetApiMaxActiveRequests(1); //api requests concurrency    
                opt.AddHealthCheckEndpoint("feedback api", "/hc"); //map health check api    

            })
            .AddInMemoryStorage();                
        }


        public static void AddCustomSwagger(this WebApplicationBuilder builder) =>
            builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = $"Adventure Works Cycles - {AppName}", Version = "v1" }));

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
            string? connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__CompanyApi");
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