namespace SurveyCart.Api.Contracts.Users
{
    public record UserProfileResponse(
        string  Email,
        string UserNamw,
        string FirstName,
        string LastName
     );

}
