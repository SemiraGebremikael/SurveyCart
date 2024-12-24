
namespace SurveyCart.Api.Contracts.Validation;
public class PollRequestValidator: AbstractValidator<PollRequest>
{
    public PollRequestValidator() {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(min: 5, max: 100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .Length(min: 3, max: 1000);

        RuleFor(x => x.StartAT)
           .NotEmpty()
           .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

        RuleFor(x => x.EndAT)
          .NotEmpty();
        RuleFor(x => x)
            .Must(HasValidDates)
            .WithName(nameof(PollRequest.EndAT))
            .WithMessage("{PropertyName} muste be greater tahn or equals start date"); 
    }
    private bool HasValidDates(PollRequest pollRequest)
    {
      return pollRequest.EndAT>=pollRequest.StartAT;
    }
}
