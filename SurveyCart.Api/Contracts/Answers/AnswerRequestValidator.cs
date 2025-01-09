namespace SurveyCart.Api.Contracts.Questions
{
    public class AnswerRequestValidator : AbstractValidator<AnswerRequest>
    {
        public AnswerRequestValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .Length(3, 1000);

            //RuleFor(x => x.Answers)
            //    .Must(x => x.Count >1)
            //    .WithMessage("Question should have at last 2 answers");

            RuleFor(x => x.Answers)
               .Must(x => x.Distinct().Count() == x.Count)
               .WithMessage("You can not add duplicated answer for the same question");
        }
    }
}
