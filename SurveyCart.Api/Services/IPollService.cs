namespace SurveyCart.Api.Services;

public interface IPollService
{
   Task <Result<IEnumerable<PollResponse>>> GettAllAsync( CancellationToken cancellationToken = default);
   Task<Result<PollResponse>> GettByIdAsync(int id, CancellationToken cancellationToken = default);
   Task<Result<PollResponse>> AddAsync (Poll poll, CancellationToken cancellationToken = default);
   Task <Result> updateAsync ( int id , PollRequest poll, CancellationToken cancellationToken = default);
   Task <Result> deleteAsync(int id , CancellationToken cancellationToken = default);
   Task<Result> TogglePublishSatusAsync(int id , CancellationToken cancellationToken = default);
}
