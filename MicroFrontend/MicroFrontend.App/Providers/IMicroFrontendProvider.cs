namespace MicroFrontend.App.Providers;

internal interface IMicroFrontendProvider
{
    MicroFrontentRegistration? GetMicroFrontend(string slug);
    Task<IEnumerable<MicroFrontentRegistration>> GetMicroFrontendsAsync();
}


