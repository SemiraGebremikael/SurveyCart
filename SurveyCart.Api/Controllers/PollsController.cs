
using SurveyCart.Api.Abstractions;
using SurveyCart.Api.Entities;

namespace SurveyCart.Api.Controllers;

[Route("api/[controller]")]
[ApiController]


public class PollsController : ControllerBase
{
    private readonly IPollService _pollService;

    public PollsController(IPollService pollService)
    {
        _pollService = pollService;
    }


    [HttpGet(template: "")]
    // [Authorize]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _pollService.GettAllAsync(cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return Problem(statusCode: StatusCodes.Status400BadRequest, title: result.Error.cod, detail: result.Error.Dscription);
    }


    [HttpGet(template: "{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.GettByIdAsync(id, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return Problem(statusCode: StatusCodes.Status400BadRequest, title: result.Error.cod, detail: result.Error.Dscription);
    }

    [HttpPost(template: "")]
    public async Task<IActionResult> Add([FromBody] Poll request, CancellationToken cancellationToken)
    {

        var result = await _pollService.AddAsync(request, cancellationToken);
        //return CreatedAtAction(nameof(Get), new { id = newPoll }, newPoll);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return Problem(statusCode: StatusCodes.Status400BadRequest, title: result.Error.cod, detail: result.Error.Dscription);
    }

    [HttpPut(template: "{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request, CancellationToken cancellationToken)
    {
        var result =  await _pollService.updateAsync(id, request, cancellationToken);
        if (result.IsSuccess)
        {
            return NoContent();
        }
        return Problem(statusCode: StatusCodes.Status400BadRequest, title: result.Error.cod, detail: result.Error.Dscription);

    }

    [HttpDelete(template: "{id}")]

    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.deleteAsync(id, cancellationToken);
        if (result.IsSuccess)
        {
            return NoContent();

        }
        return Problem(statusCode: StatusCodes.Status400BadRequest, title: result.Error.cod, detail: result.Error.Dscription);

    }


    [HttpPut(template: "{id}/TogglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.TogglePublishSatusAsync(id, cancellationToken);
        if (result.IsSuccess)
        {
            return NoContent();
        }
        return Problem(statusCode: StatusCodes.Status400BadRequest, title: result.Error.cod, detail: result.Error.Dscription);

    }

}
