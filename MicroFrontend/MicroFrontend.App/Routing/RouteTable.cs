namespace MicroFrontend.App.Routing;

internal sealed class RouteTable
{
    public RouteTable(RouteEntry[] routes)
    {
        Routes = routes;
    }

    public RouteEntry[] Routes { get; }

    public void Route(RouteContext routeContext)
    {
        for (var i = 0; i < Routes.Length; i++)
        {
            Routes[i].Match(routeContext);
            if (routeContext.Handler != null)
            {
                return;
            }
        }
    }
}
