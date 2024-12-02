using Awc.ApiGateways.Web.Api.Gateway.Middleware;
using Awc.BuildingBlocks.Observability;

var builder = WebApplication.CreateBuilder(args);

try
{       
    const string appName = "Web Api Gateway"; 
 
     builder.Configuration
           .SetBasePath(builder.Environment.ContentRootPath)
           .AddJsonFile("appsettings.json", false, true)
           .AddEnvironmentVariables()
           .AddCommandLine(args);    

    builder.AddObservability(); 

    builder.Services.AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

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

