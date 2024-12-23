
using MapsterMapper;

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
    public IActionResult GetAll()
    {
        var polls = _pollService.GettAll();
        var response = polls.Adapt<IEnumerable<Poll>>();
        return Ok(response);
    }


    [HttpGet(template: "{id}")]
    public IActionResult Get([FromRoute] int id)
    {
        var poll = _pollService.GettById(id);
        if (poll == null)
            return NotFound();

        var response = poll.Adapt<PollResponse>();
        return Ok(response);
    }

    [HttpPost(template: "")]
    public IActionResult Add([FromBody] PollRequest request)
    {
        var newPoll = _pollService.Add(request.Adapt<Poll>());
        return CreatedAtAction(nameof(Get), new {id = newPoll.Id}, newPoll);
    }

    [HttpPut(template: "{id}")]
    public IActionResult Update([FromRoute] int id , [FromBody] PollRequest request)
    {
        var isUpdated = _pollService.update(id, request.Adapt<Poll>());
        if (!isUpdated)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete(template: "{id}")]

    public IActionResult Delete([FromRoute] int id) 
    {
        var isDeleted = _pollService.delete(id);
        if (!isDeleted)
        {
            return NotFound();
        }
        return NoContent();
    
    }

}
