namespace SurveyCart.Api.Entities;
public class Poll
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPublished { get; set; } = true;
    public DateOnly StartAT { get; set; }
    public DateOnly EndAT { get; set; }
    public ICollection<Question> questions { get; set; } = [];
    public ICollection<Vote> votes { get; set; } = [];
  


}
