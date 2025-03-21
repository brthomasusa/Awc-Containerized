using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebUI.Models.CompanyApi;
using WebUI.Services.Repositories.Company;
using WebUI.Services.Repositories.Product;
using WebUI.Utilities;
using WebUI;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<IProductService, ProductService>();
// builder.Services.AddHttpClient<ICompanyService, CompanyService>(client =>
//     client.BaseAddress = new Uri(builder.Configuration["WebGatewayUrl"]!)
// );

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddRadzenComponents();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

await builder.Build().RunAsync();
