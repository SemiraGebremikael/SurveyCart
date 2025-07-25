namespace SurveyCart.Api.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentails =
       new("User.InvalidaCedentials", "Invalid email or password", StatusCodes.Status401Unauthorized);

    public static readonly Error DuplicatedEmail =
       new("User.DuplicatedEmail", "Another user with the same email is already exists", StatusCodes.Status409Conflict);

    public static readonly Error EmailNotComfirmed =
      new("User.EmailNotComfirmed", "Email is not Comfirmed ", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidCode =
     new("User.InvalidCode", "Invalid code", StatusCodes.Status401Unauthorized);

    public static readonly Error DublicatedConfirmation =
   new("User.DublicatedConfirmation", "Email already confirmed", StatusCodes.Status400BadRequest);

}
