using MicroFrontend.BaseApp.Providers;
using MicroFrontend.BaseApp.Routing;
using MicroFrontend.Core;
using Microsoft.AspNetCore.Components;

namespace MicroFrontend.App.Pages;

public partial class FrontendPage
{
    private static readonly IDictionary<string, object> EmptyParametersDictionary
        = new Dictionary<string, object>();

    [Inject]
    private IMicroFrontendProvider MicroFrontendProvider { get; set; } = null!;

    [Inject]
    private Interop Interop { get; set; } = null!;

    private Type? Component { get; set; }

    [Parameter]
    public string Slug { get; set; } = null!;

    [Parameter]
    public string? Page { get; set; }

    public IDictionary<string, object> ComponentParameters { get; set; } = EmptyParametersDictionary;

    protected override async Task OnParametersSetAsync()
    {
        var frontend = await MicroFrontendProvider.GetMicroFrontendAsync(Slug);

        var page = $"/{Page}";

        Component = null;

        if (frontend != null)
        {
            var context = new RouteContext(page);

            frontend.RouteTable.Route(context);

            if (context.Handler != null)
            {
                Component = context.Handler;
                ComponentParameters = context.Parameters ?? EmptyParametersDictionary;

                await Interop.IncludeLink(frontend.Namespace, $"{Constants.FrontendFolder}{frontend.NameTag}/{frontend.Namespace}.styles.css");
            }
        }
    }
}
