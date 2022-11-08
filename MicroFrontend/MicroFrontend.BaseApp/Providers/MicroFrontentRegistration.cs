using MicroFrontend.BaseApp.Routing;

namespace MicroFrontend.BaseApp.Providers;

public record MicroFrontentRegistration(
    string Slug,
    string Name,
    string NameTag,
    string Namespace,
    RouteTable RouteTable);
