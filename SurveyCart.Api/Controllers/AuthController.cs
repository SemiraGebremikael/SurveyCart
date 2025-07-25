
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
                var authResut = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);
                if (authResut.IsSuccess)
                {
                    return Ok(authResut);
                }
                return Problem(statusCode: StatusCodes.Status400BadRequest, title: authResut.Error.cod, detail: authResut.Error.Dscription);
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
                var authResut = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
                if (authResut.IsSuccess)
                {
                    return Ok(authResut);
                }
                return Problem(statusCode: StatusCodes.Status400BadRequest, title: authResut.Error.cod, detail: authResut.Error.Dscription);
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
                var isRoevoked = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

                if (isRoevoked.IsSuccess)
                {

                    return Ok(isRoevoked);
                }

                return Problem(statusCode: StatusCodes.Status400BadRequest, title: isRoevoked.Error.cod, detail: isRoevoked.Error.Dscription);

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
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return Problem(statusCode: StatusCodes.Status400BadRequest, title: result.Error.cod, detail: result.Error.Dscription);
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
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return Problem(statusCode: StatusCodes.Status400BadRequest, title: result.Error.cod, detail: result.Error.Dscription);
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
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return Problem(statusCode: StatusCodes.Status400BadRequest, title: result.Error.cod, detail: result.Error.Dscription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while refresh token {request}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }


        }
    }
}
