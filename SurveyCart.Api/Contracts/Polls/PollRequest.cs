namespace SurveyCart.Api.Contracts.Polls;
public record PollRequest(
    string Title,
    string Description,
    DateOnly StartAT,
    DateOnly EndAT
);

