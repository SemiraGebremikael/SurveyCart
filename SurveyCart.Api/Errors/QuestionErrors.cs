namespace SurveyCart.Api.Errors
{
    public class QuestionErrors
    {

        public static readonly Error questionNoFound =
            new("Question.NoFound", "No question was found with the given ID");
        public static readonly Error DublicatedQuestionContent =
           new("Question.DublicatedQuestionContent", "Another Question with the same content is already exists");

    }
}
