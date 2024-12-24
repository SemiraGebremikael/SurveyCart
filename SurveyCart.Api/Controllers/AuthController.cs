
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
        public async Task<IActionResult> LogginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            var authResut = await _authService.getTokenAsync(request.Email, request.Password, cancellationToken);
            if (authResut == null)
            {
                return BadRequest("Invalid email or password");
            }
            return Ok("Invalid email or password");
        }
    }
}
