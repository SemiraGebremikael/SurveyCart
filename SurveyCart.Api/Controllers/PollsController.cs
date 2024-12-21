
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SurveyCart.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController : ControllerBase
{
    private readonly IPollService _pollService;

    public PollsController(IPollService pollService)
    {
        _pollService=pollService;
    }

    [HttpGet(template: "")]
    public IActionResult GetAll()
    {
        return Ok(_pollService.GettAll());
    }


    [HttpGet(template: "{id}")]
    public IActionResult Get(int id)
    {
        return Ok(_pollService.GettById(id));
    }

    [HttpPost(template: "")]
    public IActionResult Add(Poll request)
    {
        var newPoll = _pollService.Add(request);
        return CreatedAtAction(nameof(Add),newPoll);
    }

    [HttpPut(template: "{id}")]
    public IActionResult Update(int id , Poll request)
    {
       var isUpdated = _pollService.update(id, request);
        if (!isUpdated)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete(template: "{id}")]

    public IActionResult Delete(int id) 
    {
        var isDeleted = _pollService.delete(id);
        if (!isDeleted)
        {
            return NotFound();
        }
        return NoContent();
    
    }

}
