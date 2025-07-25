
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SurveyCart.Api.Entities;
using System.Security.Cryptography;
using System.Text;
namespace SurveyCart.Api.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly  int _refreshTokenExpiryDays = 30;
    private readonly ILogger<AuthService> _logger;

    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager,  IJwtProvider jwtProvider, ILogger<AuthService> logger)
    {
        _userManager = userManager; 
        _signInManager = signInManager;
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
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (result.Succeeded) { 


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

            return Result.Failure<AuthResponse>(result.IsNotAllowed ? UserErrors.EmailNotComfirmed : UserErrors.InvalidCredentails);
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


    public async Task<Result> RegisterAsync(RegisterRequest request,  CancellationToken cancellationToken = default)
    {
        var emailIsExists = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);
        if (emailIsExists)
        {
            return Result.Failure(UserErrors.DuplicatedEmail);

        }

        var user = request.Adapt<User>();      
        var result=  await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded) 
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("Confirmation code: {Code}", code);
            return Result.Success();
        }

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }


    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken = default)

    {

        var user = await _userManager.FindByIdAsync(request.UserId);
        if(user == null)
            return Result.Failure(UserErrors.InvalidCode);

       if(user.EmailConfirmed)
            return Result.Failure(UserErrors.DublicatedConfirmation);


        var code = request.Code;
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));

        }
        catch (FormatException) {
            return Result.Failure(UserErrors.InvalidCode);
        }


        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            return Result.Success();
        }

        var error = result.Errors.First();
        return  Result.Failure(UserErrors.DublicatedConfirmation);
    }


    public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Result.Success();
        }

        if (user.EmailConfirmed)
            return Result.Failure<AuthResponse>(UserErrors.DublicatedConfirmation);

          var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
           code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        _logger.LogInformation("Confirmation code: {Code}", code);

        return Result.Success();
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
