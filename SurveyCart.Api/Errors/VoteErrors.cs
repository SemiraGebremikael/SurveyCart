namespace SurveyCart.Api.Errors
{
    public class VoteErrors
    {
        public static readonly Error InvalidQuestions =
          new("Vote.InvalidQuestions", "Invalid questions", StatusCodes.Status400BadRequest);
        public static readonly Error DubplicatedVote =
          new("Vote.DubplicatedVote", "This user alreday voted before for this poll", StatusCodes.Status409Conflict);
   
    }
}
