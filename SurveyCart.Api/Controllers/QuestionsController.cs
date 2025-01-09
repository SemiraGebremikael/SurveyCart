

using Azure.Core;
using SurveyCart.Api.Contracts.Questions;

namespace SurveyCart.Api.Controllers;

[Route("api/polls/{pollId}[controller]")]
[ApiController]
//[Authorize]
public class QuestionsController : Controller
{
    private readonly IQuestionService _questionService;
    public QuestionsController(IQuestionService questionService)
    {
        _questionService = questionService;
    }


    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var result = await _questionService.GetAll(pollId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Problem(statusCode: StatusCodes.Status404NotFound);
    }


    [HttpGet("id")]
    public async Task<IActionResult> Get([FromRoute] int pollId, int id, [FromRoute] CancellationToken cancellationToken)
    {
        var result = await _questionService.GetAsync(pollId, id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Problem(statusCode: StatusCodes.Status404NotFound);

    }


    [HttpPost("")]
    public async Task<IActionResult> Add([FromRoute] int pollId, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var result = await _questionService.AddAsync(pollId,request,cancellationToken);

        if (result.IsSuccess)
        {
           return CreatedAtAction(nameof(Get), new {pollId, result.Value.Id}, result.Value);
        }
        return result.Error.Equals(QuestionErrors.DublicatedQuestionContent)
            ? Problem(statusCode: StatusCodes.Status409Conflict)
            : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.cod, detail: result.Error.Dscription);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int pollId, [FromRoute] int id, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var result = await _questionService.UpdatedAsync(pollId, id,request, cancellationToken);

        if (result.IsSuccess)
        {
            return NoContent();
        }
        return result.Error.Equals(QuestionErrors.DublicatedQuestionContent)
            ? Problem(statusCode: StatusCodes.Status409Conflict)
            : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.cod, detail: result.Error.Dscription);
    }


    [HttpPut(template: "{id}/ToggleStatus")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int pollId,[FromRoute] int id,  CancellationToken cancellationToken)
    {
        var result = await _questionService.ToggleSatusAsync(pollId ,id, cancellationToken);
        if (result.IsSuccess)
        {
            return NoContent();
        }
        return Problem(statusCode: StatusCodes.Status400BadRequest, title: result.Error.cod, detail: result.Error.Dscription);

    }
}
