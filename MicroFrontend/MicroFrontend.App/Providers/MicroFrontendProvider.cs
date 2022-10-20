using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using MicroFrontend.Core;
using Microsoft.AspNetCore.Components;

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

    public async Task<MicroFrontentRegistration?> GetMicroFrontendAsync(string slug)
    {
        if (_isLoaded)
        {
            return _registrations.FirstOrDefault(x => x.Slug == slug);
        }

        await GetMicroFrontendsAsync();

        return _registrations.FirstOrDefault(x => x.Slug == slug);
    }

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
                        if (!type.IsAbstract && !type.IsInterface && type.GetInterfaces().Any(type => type == typeof(IMicroFrontendPlugin)))
                        {
                            if (Activator.CreateInstance(type) is IMicroFrontendPlugin plugin)
                            {
                                if (assembly.GetName().Name is string componentAssemblyName)
                                {
                                    var pages = assembly.GetTypes()
                                        .Select(x => (Type: x, Route: x.GetCustomAttribute<RouteAttribute>()?.Template))
                                        .Where(x => x.Route != null)
                                        .ToDictionary(x => x.Route!, x => x.Type);

                                    _registrations.Add(new MicroFrontentRegistration(
                                        CreateSlug(plugin.Name),
                                        plugin.Name,
                                        frontend.Key,
                                        componentAssemblyName,
                                        pages));
                                }
                            }
                        }
                    }
                }
            }
        }

        _isLoaded = true;

        _semaphore.Release();

        foreach (var registrations in _registrations)
        {
            foreach (var page in registrations.Pages)
            {
                Console.WriteLine($"{page.Key} -> {page.Value.Name}");
            }
        }

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
