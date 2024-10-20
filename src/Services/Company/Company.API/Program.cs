using HealthChecks.UI.Configuration;

var appName = "Company API Service";
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

try
{
    // builder.Services.AddCustomHealthChecks();
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

    var pathBase = builder.Configuration["PATH_BASE"];
    if (!string.IsNullOrEmpty(pathBase))
    {
        app.UsePathBase(pathBase);
    }

    app.MapGet("/", () => Results.LocalRedirect("~/swagger"));
    app.MapControllers();

        // app.Logger.LogInformation("Applying database migration ({ApplicationName})...", appName);
        // app.ApplyDatabaseMigration();

        app.Logger.LogInformation("Starting web host ({ApplicationName})...", appName);

        app.MapHealthChecks("/hc", new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseHealthChecksUI((Options options) =>
        {
            options.UIPath = "/hc-ui";
            // options.AddCustomStylesheet("./healthchecksui.css");

        });




        // app.UseHealthChecks("/hc", new HealthCheckOptions()
        // {
        //     Predicate = _ => true,
        //     ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        // });
        // app.UseHealthChecksUI(options => options.UIPath = "/hc-ui");

        app.Run();
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
