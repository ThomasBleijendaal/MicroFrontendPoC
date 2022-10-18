namespace MicroFrontend.App.Providers;

internal record MicroFrontentRegistration(
    string Slug,
    string Name,
    string Namespace,
    string RootComponentName,
    Type RootComponent);


