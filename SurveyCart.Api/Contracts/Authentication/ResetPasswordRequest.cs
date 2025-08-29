namespace SurveyCart.Api.Contracts.Authentication
{
    public record ResetPasswordRequest (
        string Eamil,
        string Code,
        string NewPassword
        );

}
