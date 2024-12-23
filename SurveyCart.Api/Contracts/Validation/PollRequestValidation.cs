
namespace SurveyCart.Api.Contracts.Validation
{
    public class PollRequestValidation: AbstractValidator<PollRequest>
    {
        public PollRequestValidation() {
            RuleFor(x => x.Title)
                .NotEmpty()
                .Length(min: 3, max: 100);

            RuleFor(x => x.Description)
                .NotEmpty()
                .Length(min: 3, max: 1000);
        }
    }
}
