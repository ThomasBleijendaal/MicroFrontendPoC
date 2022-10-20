namespace MicroFrontend.App.Providers;

internal interface IMicroFrontendProvider
{
    Task<MicroFrontentRegistration?> GetMicroFrontendAsync(string slug);
    Task<IEnumerable<MicroFrontentRegistration>> GetMicroFrontendsAsync();
}
