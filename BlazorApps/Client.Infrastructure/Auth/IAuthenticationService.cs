using Client.Infrastructure.ApiClient;

namespace Client.Infrastructure.Auth
{
    public interface IAuthenticationService
    {
        AuthProvider ProviderType { get; }

        void NavigateToExternalLogin(string returnUrl);

        Task<bool> LoginAsync(string tenantId, TokenRequest request);

        Task LogoutAsync();

        Task ReLoginAsync(string returnUrl);
    }
}