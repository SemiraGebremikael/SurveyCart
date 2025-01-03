namespace SurveyCart.Api.Errors
{
    public class PollErrors
    {

        public static readonly Error PollNoFound =
            new("Poll.PollNoFound", "Poll was not found with the given ID");
        public static readonly Error DublicatedPollTitle =
           new("Poll.PollNoFound", "Another polll with the same title is already exists");

    }
}
