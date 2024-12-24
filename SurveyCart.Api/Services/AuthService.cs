
using Microsoft.AspNetCore.Identity;
using SurveyCart.Api.Authentication;

namespace SurveyCart.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtProvider _jwtProvider;
        public AuthService(UserManager<User> userManager, IJwtProvider jwtProvider)
        {
            _userManager = userManager; 
            _jwtProvider = jwtProvider;
        }

        public async Task<AuthResponse?> getTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
           var isvalidPassWord  = await _userManager.CheckPasswordAsync(user, password);
            if (!isvalidPassWord)
            {
                return null;
            }
            var (token, expiresIn)= _jwtProvider.GenerateToken(user);
            return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn);

        }
    }
}
