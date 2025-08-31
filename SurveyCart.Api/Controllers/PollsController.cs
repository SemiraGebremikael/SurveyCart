namespace SurveyCart.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController : ControllerBase
{
    private readonly IPollService _pollService;
    private readonly ILogger<PollsController> _logger;

    public PollsController(IPollService pollService,ILogger<PollsController> logger )
    {
        _pollService = pollService;
        _logger = logger;
    }


    [HttpGet(template: "")]
    // [Authorize]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _pollService.GettAllAsync(cancellationToken);

        try
        {
            return result.IsSuccess ? Ok(result.Value) : Problem();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while get  all polls ");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    [HttpGet(template: "currentPolls")]
    public async Task<IActionResult> GetCurrent(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _pollService.GetCurrentAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : Problem();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while get current poll items");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }

    }


    [HttpGet(template: "{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _pollService.GettByIdAsync(id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) :  Problem();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while get poll with id {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }

    }

    [HttpPost(template: "")]
    public async Task<IActionResult> Add([FromBody] PollRequest request, CancellationToken cancellationToken)
    {

        try
        {
            var result = await _pollService.AddAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value.Id): Problem();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while add poll");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
       
    }

    [HttpPut(template: "{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _pollService.updateAsync(id, request, cancellationToken);
            return result.IsSuccess ? NoContent() : Problem();
       
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while removing poll with id {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }


    }

    [HttpDelete(template: "{id}")]

    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {

        try
        {
            var result = await _pollService.deleteAsync(id, cancellationToken);
            return result.IsSuccess ? NoContent() : Problem();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while removing poll with id {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }

    }


    [HttpPut(template: "{id}/TogglePublish")]
    public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _pollService.TogglePublishSatusAsync(id, cancellationToken);
            return result.IsSuccess ? NoContent() : Problem();
        }
        catch (Exception ex) 
        { _logger.LogError($"An unexpected error occurred while toggle publish poll with id {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");

        }


    }

}
