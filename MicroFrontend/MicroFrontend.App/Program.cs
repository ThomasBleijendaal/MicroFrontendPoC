using MicroFrontend.App;
using MicroFrontend.App.Providers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<Interop>();
builder.Services.AddScoped<IMicroFrontendProvider, MicroFrontendProvider>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddOidcAuthentication(config =>
{
    Console.WriteLine("OIDC");

    builder.Configuration.Bind("DevOIDC", config);

});

await builder.Build().RunAsync();
