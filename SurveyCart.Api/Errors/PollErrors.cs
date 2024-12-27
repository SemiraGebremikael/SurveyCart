namespace SurveyCart.Api.Errors
{
    public class PollErrors
    {

        public static readonly Error PollNoFound =
            new("Poll.PollNoFound", "Poll was not found with the given ID");

    }
}
