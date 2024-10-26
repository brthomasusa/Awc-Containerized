using System.Text.Json;
using System.Text;

var appName = "Company API Service";
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

try
{
    string? appKey = builder.Configuration.GetValue<string>("ApplicationInsights:ConnectionString");

    builder.Services.AddApplicationInsightsTelemetry();
    builder.Services.ConfigureHealthChecks();
    builder.AddCustomSwagger();
    builder.Services.AddMappings();
    builder.Services.AddMediatr();
    builder.AddCustomDatabase();

    builder.Services.AddControllers();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseCustomSwagger();
    }

    // var pathBase = builder.Configuration["PATH_BASE"];
    // if (!string.IsNullOrEmpty(pathBase))
    // {
    //     app.UsePathBase(pathBase);
    // }

    app.MapGet("/", () => Results.LocalRedirect("~/swagger"));
    app.MapControllers();

    // app.Logger.LogInformation("Applying database migration ({ApplicationName})...", appName);
    // app.ApplyDatabaseMigration();

    app.Logger.LogInformation("Starting web host ({ApplicationName})...", appName);

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

    // app.UseHealthChecksUI((Options options) =>
    // {
    //     options.UIPath = "/hc-ui";
    //     // options.AddCustomStylesheet("./healthchecksui.css");

    // });

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
    Serilog.Log.Fatal(ex, "Company API microservice terminated unexpectedly with message {ex.Message}.", ex.Message);
}
finally
{
    Serilog.Log.CloseAndFlush();
}

namespace Awc.Services.Company.API
{
    public partial class Program;
}
