using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using WebUI;
using WebUI.Services.Repositories.Company;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<ICompanyService, CompanyService>(client =>
    client.BaseAddress = new Uri(builder.Configuration["WebGatewayUrl"]!)
);

builder.Services.AddFluentUIComponents();

await builder.Build().RunAsync();
