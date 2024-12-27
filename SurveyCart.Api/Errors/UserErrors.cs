namespace SurveyCart.Api.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentails = 
        new ("User.InvalidaCedentials", "Invalid email or password");
}
