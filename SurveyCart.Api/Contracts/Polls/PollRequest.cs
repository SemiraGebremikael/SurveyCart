namespace SurveyCart.Api.Contracts.Polls;
public record PollRequest(
    string Title,
    string Description,
    bool IsPublished,
    DateOnly StartAT,
    DateOnly EndAT
);

