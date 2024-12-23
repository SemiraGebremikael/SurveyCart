namespace SurveyCart.Api.Contracts.Responses;

    public record PollResponse(
        int Id,
        string Title,
        string Description
    );
