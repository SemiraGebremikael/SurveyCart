using SurveyCart.Api.Contracts.Answers;

namespace SurveyCart.Api.Contracts.Questions
{
    public record QuestionResponse(
        int Id,
        string Content,
        IEnumerable<AnswerResponse> Answers

        );
 
}
