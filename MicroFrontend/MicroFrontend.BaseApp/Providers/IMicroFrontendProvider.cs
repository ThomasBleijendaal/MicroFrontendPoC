namespace MicroFrontend.BaseApp.Providers;

public interface IMicroFrontendProvider
{
    Task<MicroFrontentRegistration?> GetMicroFrontendAsync(string slug);
    Task<IEnumerable<MicroFrontentRegistration>> GetMicroFrontendsAsync();
}
