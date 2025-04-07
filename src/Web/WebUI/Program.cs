using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebUI.Models.CompanyApi;
using WebUI.Services.Repositories.Company;
using WebUI.Services.Repositories.Product;
using WebUI.Utilities;
using WebUI;
using Fluxor;
using Radzen;
using Polly;
using Polly.Extensions.Http;
using Fluxor.Blazor.Web.ReduxDevTools;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

Uri awcBaseApiAddress = new(builder.Configuration["WebGatewayUrl"]!);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// CompanyService
builder.Services.AddHttpClient<ICompanyService, CompanyService>(client =>
    client.BaseAddress = awcBaseApiAddress!
)
.SetHandlerLifetime(TimeSpan.FromMinutes(5))
.AddPolicyHandler(GetRetryPolicy());

// ProductService
builder.Services.AddHttpClient<IProductService, ProductService>(client =>
    client.BaseAddress = awcBaseApiAddress!
)
.SetHandlerLifetime(TimeSpan.FromMinutes(5))
.AddPolicyHandler(GetRetryPolicy());

builder.Services.AddFluxor(o => o
    .ScanAssemblies(typeof(Program).Assembly)
    .UseReduxDevTools()
);
builder.Services.AddRadzenComponents();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

await builder.Build().RunAsync();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}
