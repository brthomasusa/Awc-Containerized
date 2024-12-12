using System.Text;
using System.Text.Json;
using Awc.Services.Company.API.Middleware;
using Awc.Services.Company.API.Extentions;
using Awc.BuildingBlocks.Observability;
using Awc.BuildingBlocks.Observability.Options;

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

    string? connectionString = builder.Configuration["ConnectionStrings:AdventureWorksCycles"] 
        ?? ArgumentNullException("Connection string from environment is null.");

    observabilityOptions.DbConnectionString = connectionString!;    

    builder.AddObservability();        
    builder.Services.AddHealthChecks(observabilityOptions);
    builder.Services.AddCustomSwagger();
    builder.Services.AddMappings();
    builder.Services.AddMediatr();
    builder.AddCustomDatabase();

    builder.Services.AddControllers();

    WebApplication app = builder.Build();

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
    app.MapControllers();
    
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

string? ArgumentNullException(string v)
{
    throw new NotImplementedException();
}

namespace Awc.Services.Company.API
{
    public partial class Program;
}
