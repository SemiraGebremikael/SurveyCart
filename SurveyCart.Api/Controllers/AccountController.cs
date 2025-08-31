using SurveyCart.Api.Contracts.Users;
using SurveyCart.Api.Extensions;
namespace SurveyCart.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class AccountController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("")]
    public async Task<IActionResult> Info()
    {
        try {
            var tesult = await _userService.GetProfileAsync(User.GetUserId()!);
            return Ok(tesult.Value);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred while get user info ");
        }
        ;
    }

    [HttpPut("update-info")]
    public async Task<IActionResult> UpdateInfo([FromBody] UpdateProfileRequest request)
    {
        try
        {
            var userIdClaim = await _userService.UpdateProfileAsync(User.GetUserId()!, request);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred while update user info  {request} ");
        }
    }


    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        try
        {
            var resualt = User.GetUserId();
            await _userService.ChangePasswordAsync(resualt, request);
            return NoContent();
        }

        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred while change password user info  {request} ");
        }

    }
}
