using FluentValidation;
using SurveyCart.Api.Abstractions.Consts;

namespace SurveyCart.Api.Contracts.Authentication
{
    public class ResendConfirmationEmailValidator : AbstractValidator<ResendConfirmationEmailRequest>
    {
        public ResendConfirmationEmailValidator() {
            RuleFor(x => x.Email)
           .NotEmpty()
           .EmailAddress();


        }
    }
}

