
using Azure;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
namespace SurveyCart.Api.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly  int _refreshTokenExpiryDays = 30;
    private readonly ILogger<AuthService> _logger;

    public AuthService(UserManager<User> userManager, IJwtProvider jwtProvider, ILogger<AuthService> logger)
    {
        _userManager = userManager; 
        _jwtProvider = jwtProvider;
        _logger = logger;
    }

    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentails);
            }
            var isvalidPassWord = await _userManager.CheckPasswordAsync(user, password);
            if (!isvalidPassWord)
            {
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentails);

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

            var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);
            return Result.Success(response);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, $"Failed process to get token {email} {password}", ex.Message);
            throw;
        }


    }


    public async  Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        if (userId == null)
        {
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentails);
        }

        var user  = await  _userManager.FindByIdAsync(userId); 
        if (user == null)
        {
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentails);
        };

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
        if (userRefreshToken == null)
        {
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentails);
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
        var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);
        return Result.Success(response);

    }

    public async  Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        if (userId == null)
        {
           return Result.Failure<AuthResponse>(UserErrors.InvalidCredentails);
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentails);
        };

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
        if (userRefreshToken == null)
        {
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentails);
        }
        userRefreshToken.RevokedOn = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return Result.Success(user);

    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));


        //try
        //{
        //    return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        //}
        //catch (Exception ex) when (ex is not OperationCanceledException)
        //{
        //    _logger.LogError(ex, $"Failed process to generate refresh", ex.Message);
        //    throw;
        //}
    }


}
