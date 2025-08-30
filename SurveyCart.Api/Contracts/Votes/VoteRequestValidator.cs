using SurveyCart.Api.Abstractions.Consts;
using SurveyCart.Api.Contracts.Users;

namespace SurveyCart.Api.Contracts.Votes
{
    public class VoteRequestValidator : AbstractValidator<VoteRequest>
    {
        public VoteRequestValidator()
        {
            RuleFor(x => x.Answers)
                .NotEmpty();

            RuleForEach(x => x.Answers)
                .SetInheritanceValidator(v => v.Add (new VoteAnswerRequestValidator()));
        }
    }

}
