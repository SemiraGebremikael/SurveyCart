
namespace SurveyCart.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService; 
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("")]
        public async Task<IActionResult> LogginAsync( [FromBody]LoginRequest request, CancellationToken cancellationToken)
        {
            var authResut = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);
            return authResut.IsSuccess ? Ok(authResut) : BadRequest(authResut.Error);
        }


        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var authResut = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
            if (authResut == null)
            {
                return BadRequest("Invalid token");
            }
            return Ok("Invalid email or password");
        }


        [HttpPost("revokeRefreshToken")]
        public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var isRoevoked = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

            if (isRoevoked == false)
            {
                return BadRequest("Operation failed");
            }

            return Ok("Token revoked successfully");
        }
    }
}
