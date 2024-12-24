using SurveyCart.Api.Contracts.Authentication;

namespace SurveyCart.Api.Services
{
    public interface IAuthService
    {
        Task<AuthResponse?> getTokenAsync(string email,string password, CancellationToken cancellationToken = default );

    }
}
