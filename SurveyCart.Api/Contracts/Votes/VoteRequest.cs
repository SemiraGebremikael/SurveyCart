namespace SurveyCart.Api.Contracts.Votes
{
    public record VoteRequest(
        IEnumerable<VoteAnswerRequest> Answers);
}
