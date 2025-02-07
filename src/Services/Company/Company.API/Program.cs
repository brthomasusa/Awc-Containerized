using Asp.Versioning.Builder;
using Asp.Versioning;
using System.Text;
using System.Text.Json;
using Awc.Services.Company.API.Extentions;
using Awc.Services.Company.API.Infrastructure;
using Awc.Services.Company.API.Middleware;
using Awc.BuildingBlocks.Observability;
using Awc.BuildingBlocks.Observability.Options;
using StackExchange.Redis;

const string appName = "Company API Service";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

try
{
    builder.Configuration.Sources.Clear();
    builder.Configuration
        .AddJsonFile("appsettings.json", false, true)
        .AddEnvironmentVariables();

    ObservabilityOptions observabilityOptions = new();

    builder.Configuration
        .GetRequiredSection(nameof(ObservabilityOptions))
        .Bind(observabilityOptions);

    // Add Redis configuration
    var redisConfiguration = builder.Configuration["ConnectionStrings:Redis"];
    var redis = ConnectionMultiplexer.Connect(redisConfiguration);
    builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
    builder.Services.AddSingleton<ICacheService, CacheService>();

    string? dbConnectionString = builder.Configuration["ConnectionStrings:CompanyDbAzure"];
    if (dbConnectionString is null)
    {
        throw new ArgumentNullException("Database connection string is null!");
    }

    builder.Services.Configure<DatabaseReconnectSettings>(builder.Configuration.GetSection("DatabaseReconnectSettings"));
    builder.Services.AddSingleton<IDatabaseRetryService, DatabaseRetryService>();

    observabilityOptions.DbConnectionString = dbConnectionString!;

    builder.AddObservability();
    builder.Services.AddHealthChecks(observabilityOptions);

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

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
    builder.AddCustomDatabase();

    WebApplication app = builder.Build();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseSerilogRequestLogging(opts =>
        {
            opts.EnrichDiagnosticContext = LogHelper.EnrichFromRequest;
            opts.GetLevel = LogHelper.ExcludeHealthChecks;
        }
    );

    ApiVersionSet apiVersionSet = app.NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .ReportApiVersions()
        .Build();

    RouteGroupBuilder versionedGroup = app
        .MapGroup("api/v{version:apiVersion}")
        .WithApiVersionSet(apiVersionSet);

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapEndpoints(versionedGroup);

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
        });

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
    Console.WriteLine($"Company API microservice terminated unexpectedly with message: {ex.Message}");
}
finally
{
    Serilog.Log.CloseAndFlush();
}

namespace Awc.Services.Company.API
{
    public partial class Program;
}

