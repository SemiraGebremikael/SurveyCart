using SurveyCart.Api.Abstractions;

namespace SurveyCart.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService; 
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger=logger;
        }

        [HttpPost("")]
        public async Task<IActionResult> Loggin( [FromBody]LoginRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);
                return result.IsSuccess ? Ok(result) : Problem();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while logging  {request}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
                return result.IsSuccess ? Ok(result) : Problem();   
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while refresh token {request}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }
        }


        [HttpPost("revokeRefreshToken")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
                return result.IsSuccess ? Ok(result) : Problem();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while revoke refresh token {request}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _authService.RegisterAsync(request,  cancellationToken);
                return result.IsSuccess ? Ok(result) : Problem();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while refresh token {request}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> Register([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _authService.ConfirmEmailAsync(request);
                return result.IsSuccess? Ok(result) : Problem();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while refresh token {request}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }
        }


        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _authService.ResendConfirmationEmailAsync(request);
                return result.IsSuccess ? Ok(result) : Problem();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while refresh token {request}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            try
            {
                var result = await _authService.SendResetPasswordAsync(request.Email);
                return result.IsSuccess ? Ok(result) : Problem();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while refresh token {request}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                var result = await _authService.ResetPasswordAsync(request);
                return result.IsSuccess ? Ok(result) : Problem();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while refresh token {request}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }
        }
    }
}
