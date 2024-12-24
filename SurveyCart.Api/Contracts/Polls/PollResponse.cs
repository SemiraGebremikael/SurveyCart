namespace SurveyCart.Api.Contracts.Polls;
public record PollResponse(
    int Id,
    string Title,
    string Description,
    bool IsPublished,
    DateOnly StartAT,
    DateOnly EndAT
);
