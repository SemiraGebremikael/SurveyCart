using SurveyCart.Api.Contracts.Votes;

namespace SurveyCart.Api.Services
{
    public interface IVoteServices
    {
        Task<Result> AddAsync(int pollId,  string userId, VoteRequest request,  CancellationToken cancellationToken = default);
    }
}
