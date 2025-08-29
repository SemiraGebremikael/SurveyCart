using SurveyCart.Api.Abstractions.Consts;

namespace SurveyCart.Api.Contracts
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Eamil)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Code)
               .NotEmpty();

            RuleFor(x => x.NewPassword)
               .NotEmpty()
               .Matches(RegexPatterns.Password)
              .WithMessage("Password shoude be at least  8 digit  contain uppercase, one lowercase letter, special character.");
        }

    }
 
}
