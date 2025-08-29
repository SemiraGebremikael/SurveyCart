
using Azure;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services; 
using Microsoft.AspNetCore.WebUtilities;
using SurveyCart.Api.Helpers;
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
    private readonly IEmailSender _emailSender;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public AuthService(
        UserManager<User> userManager, 
        SignInManager<User> signInManager,  
        IJwtProvider jwtProvider, 
        ILogger<AuthService> logger, 
        IEmailSender  emailSender,
        IHttpContextAccessor httpContextAccessor
        )
    {
        _userManager = userManager; 
        _signInManager = signInManager;
        _jwtProvider = jwtProvider;
        _logger = logger;
        _emailSender = emailSender;
        _httpContextAccessor = httpContextAccessor;
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
            //BackgroundJob.Enqueue(() => SendConfirmationEmail(user, code));
            await SendConfirmationEmail(user, code);
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
        await SendConfirmationEmail(user, code);

        return Result.Success();
    }



    public async Task<Result> SendResetPasswordAsync (string email)
    {
        if(await _userManager.FindByEmailAsync(email) is not { }  user)
         return Result.Success();

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        _logger.LogInformation("Rest code: {Code}", code);
        await SendResetPassword(user, code);

        return Result.Success();

    }


    public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.Eamil);
        if (user is null || !user.EmailConfirmed)
            return Result.Failure(UserErrors.InvalidCode);
       IdentityResult result;
        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            result = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);
        }
        catch (FormatException)
        {
            result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }
        if (result.Succeeded)
            return Result.Success();
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    private async Task SendConfirmationEmail(User user, string code)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
            templateModel: new Dictionary<string, string>
                {
                        {"{{nam}}", user.FirstName},
                        {"{{action_url}}" , $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}"}
                }
         );
        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "survey Cart: Email Confirmation", emailBody));

        await Task.CompletedTask;
    }

    private async Task SendResetPassword(User user, string code)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("ForegetPassword",
            templateModel: new Dictionary<string, string>
                {
                        {"{{nam}}", user.FirstName},
                        {"{{action_url}}" , $"{origin}/auth/ForegetPassword?email={user.Email}&code={code}"}
                }
         );
        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "survey Cart: change password ", emailBody));

        await Task.CompletedTask;
    }
}
