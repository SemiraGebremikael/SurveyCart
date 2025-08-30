using SurveyCart.Api.Contracts.Votes;

namespace SurveyCart.Api.Services
{
    public class VoteServices(
        ApplicationDbContext context
        ) : IVoteServices
    {
        public readonly ApplicationDbContext _context = context;

        public async Task<Result> AddAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken = default)
        {
            var hasVoted = await _context.Votes.AnyAsync(v => v.PollId == pollId && v.UserId == userId, cancellationToken);
            if (hasVoted)
            {
                return Result.Failure(VoteErrors.DubplicatedVote);
            }
            var pollExists = await _context.Polls.AnyAsync(p => p.Id == pollId && p.IsPublished && p.StartAT <= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);
            if (!pollExists)
            {
                return Result.Failure(PollErrors.PollNoFound);        
            }
            var availableQuestions = await _context.Questions.Where(q => q.PollId == pollId && q.IsActive)
                                                               .Select(q => q.Id).ToListAsync(cancellationToken);


            if(!request.Answers.Select(x => x.QuestionId).SequenceEqual(availableQuestions))
            {
                return Result.Failure(VoteErrors.InvalidQuestions);
            }

            var vote = new Vote
            {
                PollId = pollId,
                UserId = userId,
                Answers = request.Answers.Adapt<IEnumerable<VoteAnswer>>().ToList()
            };

            await _context.Votes.AddAsync(vote, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
