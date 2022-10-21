using MicroFrontend.App.Routing;

namespace MicroFrontend.App.Providers;

internal record MicroFrontentRegistration(
    string Slug,
    string Name,
    string NameTag,
    string Namespace,
    RouteTable RouteTable);
