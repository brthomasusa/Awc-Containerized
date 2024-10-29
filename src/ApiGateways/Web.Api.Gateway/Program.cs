using Awc.ApiGateways.Web.Api.Gateway.Middleware;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration)
);

try
{
    const string appName = "Web Api Gateway";

    builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

    builder.Services.AddOcelot()
        .AddCacheManager(x => x.WithDictionaryHandle());

    var app = builder.Build();

    await app.UseOcelot();
    app.UseHttpsRedirection();

    app.Logger.LogInformation("Starting web host ({ApplicationName})...", appName);
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.Run();
}
catch (Exception ex)
{
    Serilog.Log.Fatal(ex, "Web.Api.Gateway terminated unexpectedly with message {ex.Message}.", ex.Message);
}
finally
{
    Serilog.Log.CloseAndFlush();
}

namespace Awc.ApiGateways.Web.Api.Gateway
{
    public partial class Program;
}

