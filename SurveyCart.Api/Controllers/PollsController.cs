
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
    public async Task  <IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var polls = await _pollService.GettAllAsync(cancellationToken);
        var response = polls.Adapt<IEnumerable<Poll>>();
        return Ok(response);
    }


    [HttpGet(template: "{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var poll = await _pollService.GettByIdAsync(id, cancellationToken); 
        if (poll == null)
            return NotFound();

        var response = poll.Adapt<PollResponse>();
        return Ok(response);
    }

    [HttpPost(template: "")]
    public async Task<IActionResult> Add([FromBody] PollRequest request, CancellationToken cancellationToken)
    {

        var newPoll = await _pollService.AddAsync(request.Adapt<Poll>(), cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);
    }

    [HttpPut(template: "{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request, CancellationToken cancellationToken)
    {
        var isUpdated =  await _pollService.updateAsync(id, request.Adapt<Poll>(), cancellationToken);
        if (!isUpdated)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete(template: "{id}")]

    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var isDeleted = await _pollService.deleteAsync(id, cancellationToken);
        if (!isDeleted)
        {
            return NotFound();
        }
        return NoContent();

    }


    [HttpPut(template: "{id}/TogglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        var isUpdated = await _pollService.TogglePublishSatusAsync(id, cancellationToken);
        if (!isUpdated)
        {
            return NotFound();
        }
        return NoContent();
    }

}
