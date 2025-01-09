namespace SurveyCart.Api.Contracts.Questions
{
    public record AnswerRequest(
        string Content,
        List<string> Answers
        );
   
}
