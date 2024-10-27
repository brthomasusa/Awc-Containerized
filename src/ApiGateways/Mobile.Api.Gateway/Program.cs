using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot()
    .AddCacheManager(x => x.WithDictionaryHandle());

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {}

await app.UseOcelot();
app.UseHttpsRedirection();

app.Run();
