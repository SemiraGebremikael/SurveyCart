using SurveyCart.Api.Contracts.Votes;
using SurveyCart.Api.Extensions;

namespace SurveyCart.Api.Controllers;

[Route("api/polls/{pollId}/vote")]
[ApiController]
//[Authorize]
public class VotesController(IQuestionService questionService, IVoteServices voteServices) : ControllerBase
{
    private readonly IQuestionService _questionService = questionService;
    private readonly IVoteServices _voteServices = voteServices;

    [HttpGet("")]
    public async Task<IActionResult> Start([FromBody] int pollId, CancellationToken cancellationToken )
    {
        try 
        {
            var userId = User.GetUserId();
            var result = await _questionService.GetAll(pollId, userId!, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : Problem();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred while get user info ");
            
        }

       
    }

    [HttpPost("")]
    public async Task<IActionResult> Vote([FromRoute] int pollId, [FromBody] VoteRequest request,CancellationToken cancellationToken)
    {
        try
        {
            var result = await _voteServices.AddAsync(pollId, User.GetUserId()!, request, cancellationToken);
            return result.IsSuccess ? Created() : Problem();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred while validating the request {request} ");
        }

    }


}
