namespace SurveyCart.Api.Services;

public interface IPollService
{
   Task <IEnumerable<Poll>> GettAllAsync( CancellationToken cancellationToken = default);
    Task<Poll> GettByIdAsync(int id, CancellationToken cancellationToken = default);
   Task<Poll>AddAsync (Poll poll, CancellationToken cancellationToken = default);
   Task <bool> updateAsync ( int id , Poll poll, CancellationToken cancellationToken = default);
   Task <bool> deleteAsync(int id , CancellationToken cancellationToken = default);
   Task<bool> TogglePublishSatusAsync(int id , CancellationToken cancellationToken = default);
}
