using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using MicroFrontend.Core;

namespace MicroFrontend.App.Providers;

internal class MicroFrontendProvider : IMicroFrontendProvider
{
    private readonly HttpClient _httpClient;
    private bool _isLoaded = false;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly List<MicroFrontentRegistration> _registrations = new();

    public MicroFrontendProvider(
        HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public MicroFrontentRegistration? GetMicroFrontend(string slug)
        => _isLoaded ? _registrations.FirstOrDefault(x => x.Slug == slug) : null;

    public async Task<IEnumerable<MicroFrontentRegistration>> GetMicroFrontendsAsync()
    {
        if (_isLoaded)
        {
            return _registrations;
        }

        await _semaphore.WaitAsync();

        if (_isLoaded)
        {
            return _registrations;
        }

        var config = await _httpClient.GetFromJsonAsync<FrontendConfig>("/frontends/frontends.json");

        if (config?.Frontends != null)
        {
            foreach (var frontend in config.Frontends)
            {
                var assembly = await LoadAssembliesAsync(frontend.Key, frontend.Value);

                if (assembly != null)
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        Console.WriteLine(type.FullName);

                        if (!type.IsAbstract && !type.IsInterface && type.GetInterfaces().Any(type => type == typeof(IMicroFrontendPlugin)))
                        {
                            if (Activator.CreateInstance(type) is IMicroFrontendPlugin plugin)
                            {
                                var componentAssemblyName = assembly?.GetName().Name;
                                var rootComponent = assembly?.GetType($"{componentAssemblyName}.Root");

                                if (componentAssemblyName != null && rootComponent != null)
                                {
                                    _registrations.Add(new MicroFrontentRegistration(
                                        CreateSlug(plugin.Name),
                                        plugin.Name,
                                        frontend.Key,
                                        componentAssemblyName,
                                        rootComponent));
                                }
                            }
                        }
                    }
                }
            }
        }

        _isLoaded = true;

        _semaphore.Release();

        return _registrations;
    }

    private async Task<Assembly?> LoadAssembliesAsync(string folder, string[] assemblies)
    {
        Assembly? assembly = null;

        foreach (var file in assemblies)
        {
            var stream = await _httpClient.GetStreamAsync($"/frontends/{folder}/{file}.dll");
            assembly = AssemblyLoadContext.Default.LoadFromStream(stream);
        }

        return assembly;
    }

    private string CreateSlug(string name) => Regex.Replace(name.ToLower(), "[^a-z0-9]", "-").Replace("--", "-");
}


