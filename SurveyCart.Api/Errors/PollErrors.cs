namespace SurveyCart.Api.Errors
{
    public class PollErrors
    {

        public static readonly Error PollNoFound =
            new("Poll.PollNoFound", "Poll was not found with the given ID", StatusCodes.Status400BadRequest);
        public static readonly Error DublicatedPollTitle =
           new("Poll.PollNoFound", "Another poll with the same title is already exists", StatusCodes.Status409Conflict);

    }
}
