using MicroFrontend.BaseApp.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace MicroFrontend.BaseApp;

public static class DependencyConfiguration
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IMicroFrontendProvider, MicroFrontendProvider>();

        return services;
    }
}
