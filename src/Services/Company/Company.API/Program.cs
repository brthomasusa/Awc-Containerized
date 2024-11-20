using System.Text;
using System.Text.Json;
using Awc.Services.Company.API.Middleware;
using Serilog.Sinks.OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

const string appName = "Company API Service";
const string serviceName = "companyApi";

var builder = WebApplication.CreateBuilder(args);

try
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

    builder.Services.ConfigureHealthChecks();
    builder.AddCustomSwagger();
    builder.Services.AddMappings();
    builder.Services.AddMediatr();
    builder.AddCustomDatabase();

    builder.Services.AddControllers();

    builder.Services.AddOpenTelemetry()
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
                .AddConsoleExporter()
                .AddOtlpExporter(opts => opts.Endpoint = new Uri("http://localhost:4317"))
        );

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseCustomSwagger();
    }

    app.UseSerilogRequestLogging();
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.MapGet("/", () => Results.LocalRedirect("~/swagger"));
    app.MapControllers();

    Serilog.Log.Information("Starting web host {ApplicationName}...", appName);

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
