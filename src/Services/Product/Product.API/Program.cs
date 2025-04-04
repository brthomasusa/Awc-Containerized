using Asp.Versioning.Builder;
using Asp.Versioning;
using System.Text;
using System.Text.Json;
using Awc.Services.Product.Product.API;
using Awc.Services.Product.Product.API.Infrastructure;
using Awc.Services.Product.Product.API.Middleware;
using Awc.Services.Product.Product.API.Extentions;
using Awc.Services.Product.Product.API.Services;
using Awc.BuildingBlocks.Observability;
using Awc.BuildingBlocks.Observability.Options;
using StackExchange.Redis;

const string appName = "Product API Service";

var builder = WebApplication.CreateBuilder(args);

try
{
    builder.Configuration.Sources.Clear();
    builder.Configuration
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile("appsettings.Development.json", false, true)
        .AddEnvironmentVariables();

    // Configure Redis cache
    string? redisConnectionString = builder.Configuration["ConnectionStrings:Redis"] ?? throw new ArgumentNullException("Redis connection string is null.");
    var redis = ConnectionMultiplexer.Connect(redisConnectionString);
    builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
    builder.Services.AddSingleton<ICacheService, CacheService>();

    // Configure db connection retry policy, efcore, and dapper
    string? dbConnectionString = builder.Configuration["ConnectionStrings:ProductDb"] ?? throw new ArgumentNullException("Db connection string is null.");
    builder.Services.Configure<DatabaseReconnectSettings>(builder.Configuration.GetSection("DatabaseReconnectSettings"));
    builder.Services.AddSingleton<IDatabaseRetryService, DatabaseRetryService>();
    builder.AddCustomDatabase(dbConnectionString);

    // Configure OpenTelemetry
    ObservabilityOptions observabilityOptions = new();

    builder.Configuration
        .GetRequiredSection(nameof(ObservabilityOptions))
        .Bind(observabilityOptions);

    observabilityOptions.DbConnectionString = dbConnectionString;

    builder.AddObservability();

    builder.Services.AddHealthChecks(observabilityOptions);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddCustomSwagger();

    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    }).AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

    builder.Services.AddEndpoints(typeof(Program).Assembly);
    builder.Services.AddMappings();
    builder.Services.AddMediatr();


    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseCustomSwagger();
    }

    app.UseSerilogRequestLogging(opts =>
        {
            opts.EnrichDiagnosticContext = LogHelper.EnrichFromRequest;
            opts.GetLevel = LogHelper.ExcludeHealthChecks;
        }
    );

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    ApiVersionSet apiVersionSet = app.NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .ReportApiVersions()
        .Build();

    RouteGroupBuilder versionedGroup = app
        .MapGroup("api/v{version:apiVersion}")
        .WithApiVersionSet(apiVersionSet);

    app.MapEndpoints(versionedGroup);

    app.MapGet("/", () => Results.LocalRedirect("~/swagger"));

    app.MapHealthChecks(
        "/hc",
        new HealthCheckOptions
        {
            AllowCachingResponses = false,
            ResultStatusCodes =
            {
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
            },
            ResponseWriter = JsonResponse
        }
    )
    .WithName("HealthCheck")
    .WithOpenApi();

    Serilog.Log.Information("Starting web host {ApplicationName}...", appName);

    app.Run();

    static async Task JsonResponse(HttpContext context, HealthReport healthReport)
    {
        context.Response.ContentType = "application/json; charset=utf-8";
        var options = new JsonWriterOptions { Indented = true };

        await using var memoryStream = new MemoryStream();

        await using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
        {
            jsonWriter.WriteStartObject();
            jsonWriter.WriteString("status", healthReport.Status.ToString());
            jsonWriter.WriteStartObject("results");

            foreach (var healthReportEntry in healthReport.Entries)
            {
                jsonWriter.WriteStartObject(healthReportEntry.Key);
                jsonWriter.WriteString(
                    "status",
                    healthReportEntry.Value.Status.ToString()
                );
                jsonWriter.WriteString(
                    "description",
                    healthReportEntry.Value.Description
                );
                jsonWriter.WriteStartObject("data");

                foreach (var item in healthReportEntry.Value.Data)
                {
                    jsonWriter.WritePropertyName(item.Key);

                    JsonSerializer.Serialize(
                        jsonWriter,
                        item.Value,
                        item.Value?.GetType() ?? typeof(object)
                    );
                }

                jsonWriter.WriteEndObject();
                jsonWriter.WriteEndObject();
            }

            jsonWriter.WriteEndObject();
            jsonWriter.WriteEndObject();
        }

        await context.Response.WriteAsync(
            Encoding.UTF8.GetString(memoryStream.ToArray()));
    }

}
catch (Exception ex)
{
    Console.WriteLine($"Product API microservice terminated unexpectedly with message: {ex.Message}");
}
finally
{
    Serilog.Log.CloseAndFlush();
}

namespace Awc.Services.Product.Product.API
{
    public partial class Program;
}
