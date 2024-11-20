using Awc.ApiGateways.Web.Api.Gateway.Middleware;
using Serilog;
using Serilog.Sinks.OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

try
{       
    const string appName = "Web Api Gateway";
    const string serviceName = "yarpProxy";
 
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
 
    builder.Services.AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

    builder.Services.AddOpenTelemetry().WithTracing(builder => 
        builder.AddAspNetCoreInstrumentation()
               .AddHttpClientInstrumentation()
               .AddConsoleExporter()
               .AddSource("Yarp.ReverseProxy")
               .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
               .AddOtlpExporter(opts => opts.Endpoint = new Uri("http://localhost:4317"))                                   
    );

    var app = builder.Build();

    app.UseHttpsRedirection();

    app.Logger.LogInformation("Starting web host ({ApplicationName})...", appName);
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.MapReverseProxy();

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Web.Api.Gateway terminated unexpectedly with message: {ex.Message}");
}
finally
{
    Serilog.Log.CloseAndFlush();
}

namespace Awc.ApiGateways.Web.Api.Gateway
{
    public partial class Program;
}

