namespace SurveyCart.Api.Contracts.Questions
{
    public class QuestionRequestValidator : AbstractValidator<AnswerRequest>
    {
        public QuestionRequestValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .Length(3, 1000);
            RuleFor(x => x.Content)
                .NotNull();

            RuleFor(x => x.Answers)
                .Must(x => x.Count >1)
                .WithMessage("Question should have at last 2 answers")
                .When(x => x.Answers != null);

            RuleFor(x => x.Answers)
               .Must(x => x.Distinct().Count() == x.Count)
               .WithMessage("You can not add duplicated answer for the same question")
               .When(x => x.Answers != null);
        }
    }
}
