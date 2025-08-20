using SurveyCart.Api.Contracts.Users;

namespace SurveyCart.Api.Controllers;


[Route("api/AccountInfo")]
[ApiController]
[Authorize]

public class AccountController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    [HttpGet("")]
    public async Task<IActionResult> Info()
    {
        //var tesult = await _userService.GetProfileAsync(User.GetUserId()!);
        //return Ok(tesult.value);

        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        var result = await _userService.GetProfileAsync(userIdClaim.Value);
        return Ok(result.Value);

    }

    [HttpGet("update-info")]

    public async Task<IActionResult> UpdateInfo([FromBody] UpdateProfileRequest request)
    {
        //var userIdClaim = await _userService.UpdateProfileAsync(User.GetUserId()!, request);
        //return NoContent();


        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        await _userService.UpdateProfileAsync(userId, request);
        return NoContent();

    }
}
