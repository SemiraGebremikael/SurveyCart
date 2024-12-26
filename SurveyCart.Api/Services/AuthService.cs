
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurveyCart.Api.Entities;
using System.Security.Cryptography;
namespace SurveyCart.Api.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly  int _refreshTokenExpiryDays = 30;

    public AuthService(UserManager<User> userManager, IJwtProvider jwtProvider)
    {
        _userManager = userManager; 
        _jwtProvider = jwtProvider;
    }

    public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
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
        };
        var (token, expiresIn)= _jwtProvider.GenerateToken(user);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration
        });
        await _userManager.UpdateAsync(user);
        return new AuthResponse(user.Id, user.Email,  user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);

    }


    public async  Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        if (userId == null)
        {
            return null;
        }

        var user  = await  _userManager.FindByIdAsync(userId); 
        if (user == null)
        {
            return null;
        };

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
        if (userRefreshToken == null)
        {
            return null;
        }
        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var (newToken, expiresIn)= _jwtProvider.GenerateToken(user);
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration
        });
        await _userManager.UpdateAsync(user);
        return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);

    }

    public async  Task<bool?> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        if (userId == null)
        {
            return false;
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        };

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
        if (userRefreshToken == null)
        {
            return false;
        }
        userRefreshToken.RevokedOn = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return true;

    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    }


}
