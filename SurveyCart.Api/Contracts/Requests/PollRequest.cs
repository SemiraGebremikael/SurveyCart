namespace SurveyCart.Api.Contracts.Requests;
public record PollRequest(
    string Title,
    string Description,
    bool IsPublished,
    DateOnly StartAT,
    DateOnly EndAT
);
    
   