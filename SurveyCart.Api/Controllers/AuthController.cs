
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
        public async Task<IActionResult> LogginAsync( [FromBody]LoginRequest request, CancellationToken cancellationToken)
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
            


        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
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
        public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
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
    }
}
