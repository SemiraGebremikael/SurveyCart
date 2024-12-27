
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
        var polls = await _pollService.GettAllAsync(cancellationToken);

        if (polls.IsFailure)
        {
            return NotFound(polls.Error);
        }
        return Ok(polls.Value);
    }


    [HttpGet(template: "{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.GettByIdAsync(id, cancellationToken);
        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }
        return Ok(result.Value);
    }

    [HttpPost(template: "")]
    public async Task<IActionResult> Add([FromBody] Poll request, CancellationToken cancellationToken)
    {

        var newPoll = await _pollService.AddAsync(request, cancellationToken);
        //return CreatedAtAction(nameof(Get), new { id = newPoll }, newPoll);
        if (newPoll.IsFailure)
        {
            return NotFound(newPoll.Error);
        }
        return Ok(newPoll.Value);
    }

    [HttpPut(template: "{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request, CancellationToken cancellationToken)
    {
        var result =  await _pollService.updateAsync(id, request, cancellationToken);
        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }
        return  NoContent();
    }

    [HttpDelete(template: "{id}")]

    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.deleteAsync(id, cancellationToken);
        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }
        return NoContent();

    }


    [HttpPut(template: "{id}/TogglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _pollService.TogglePublishSatusAsync(id, cancellationToken);
        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }
        return NoContent();
    }

}
