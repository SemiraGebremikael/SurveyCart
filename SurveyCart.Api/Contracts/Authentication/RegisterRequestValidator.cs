using SurveyCart.Api.Abstractions.Consts;

namespace SurveyCart.Api.Contracts.Authentication
{
    public class RegisterRequestValidator:  AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Password)
              .NotEmpty()
              .Matches(RegexPatterns.Password)
              .WithMessage("password should be at least 8 digits and should contains Lowercase, Uppercase, NonAlphanumeric");
            RuleFor(x => x.FirstName)
               .NotEmpty()
               .Length(3, 100);
            RuleFor(x => x.Lastname)
              .NotEmpty()
              .Length(3, 100);
        }

    }
}
