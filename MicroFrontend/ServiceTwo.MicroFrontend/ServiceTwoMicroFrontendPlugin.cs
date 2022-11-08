using MicroFrontend.Core;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceOne.MicroFrontend;

public class ServiceTwoMicroFrontendPlugin : IMicroFrontendPlugin
{
    public string Name => "Service Two";

    public void ConfigureServices(IServiceCollection services)
    {
        
    }
}
