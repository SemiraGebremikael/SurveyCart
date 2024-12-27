
namespace SurveyCart.Api.Services;

public class PollService : IPollService
{
    public  readonly ApplicationDbContext _context;
    public PollService( ApplicationDbContext applicationDb)
    {
        _context = applicationDb;
    }

    public async Task<Result<IEnumerable<PollResponse>>> GettAllAsync(CancellationToken cancellationToken = default)
    {
        var polls = await _context.Polls.AsNoTracking().ToListAsync(cancellationToken);
        return Result.Success(polls.Adapt<IEnumerable<PollResponse>>());
    }

    public async Task<Result<PollResponse>> GettByIdAsync(int id, CancellationToken cancellationToken = default)
    {
       var poll = await _context.Polls.FindAsync(id, cancellationToken);
        if (poll == null)
        {
            return Result.Failure<PollResponse>(PollErrors.PollNoFound);

        }
        return Result.Success(poll.Adapt<PollResponse>());
    }

    public async Task<Result<PollResponse>> AddAsync(Poll poll, CancellationToken cancellationToken = default)
    {
       await _context.AddAsync(poll, cancellationToken);
       await _context.SaveChangesAsync();
        if (poll == null)
        {
            return Result.Failure<PollResponse>(PollErrors.PollNoFound);
        }
        return Result.Success(poll.Adapt<PollResponse>());

    }

    public async Task<Result> updateAsync(int id, PollRequest poll, CancellationToken cancellationToken = default)
    {
        var currentPoll = await _context.Polls.FindAsync(id, cancellationToken);
        if (currentPoll == null)
        {
            return Result.Failure(PollErrors.PollNoFound);
        }
        currentPoll.Title = poll.Title;
        currentPoll.Description = poll.Description;
        currentPoll.StartAT = poll.StartAT;
        currentPoll.EndAT = poll.EndAT;
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> deleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var pollToRemove = await _context.Polls.FindAsync(id, cancellationToken);
        if (pollToRemove == null)
        {
            return Result.Failure(PollErrors.PollNoFound);
        }
        _context.Remove(pollToRemove);
        await _context.SaveChangesAsync( cancellationToken);
        return Result.Success();

    }
    public async Task<Result> TogglePublishSatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await GettByIdAsync(id, cancellationToken);
        if (poll == null)
        {
            return Result.Failure(PollErrors.PollNoFound);
        }
        //poll.IsPublished = !poll.IsPublished;
        await _context.SaveChangesAsync();
        return Result.Success();
    }


}
