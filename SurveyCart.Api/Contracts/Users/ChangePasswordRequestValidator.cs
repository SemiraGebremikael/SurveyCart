using SurveyCart.Api.Abstractions.Consts;

namespace SurveyCart.Api.Contracts.Users
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
              .NotEmpty()
              .Matches(RegexPatterns.Password)
              .WithMessage("Password shoude be at least  8 digit  contain uppercase, one lowercase letter, special character.")
              .NotEqual(x => x.CurrentPassword)
              .WithMessage("New password can not be same as the current password");
        }

    }

}
