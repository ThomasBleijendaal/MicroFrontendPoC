using Microsoft.Extensions.DependencyInjection;

namespace MicroFrontend.Core;

public interface IMicroFrontendPlugin
{
    string Name { get; }

    void ConfigureServices(IServiceCollection services);
}
