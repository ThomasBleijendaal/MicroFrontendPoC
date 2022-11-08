using MicroFrontend.Core;
using Microsoft.Extensions.DependencyInjection;
using ServiceOne.MicroFrontend.Services;

namespace ServiceOne.MicroFrontend;

public class ServiceOneMicroFrontendPlugin : IMicroFrontendPlugin
{
    public string Name => "Service One";

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ServiceOneService>();
    }
}
