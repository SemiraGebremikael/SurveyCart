
using SurveyCart.Api.Entities;

namespace SurveyCart.Api.Services;

public class PollService : IPollService
{
    public  readonly ApplicationDbContext _context;
    public PollService( ApplicationDbContext applicationDb)
    {
        _context = applicationDb;
    }

    public async Task<IEnumerable<Poll>> GettAllAsync( CancellationToken cancellationToken = default)
    { 
      return  await _context.Polls.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Poll?> GettByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id < 0)
        {
            throw new ArgumentNullException("Id not found");
        }

        return await _context.Polls.FindAsync(id, cancellationToken);
    }

    public async Task< Poll> AddAsync(Poll poll, CancellationToken cancellationToken = default)
    {
       await _context.AddAsync(poll, cancellationToken);
        await _context.SaveChangesAsync();  

        return poll;
    }

    public async Task<bool> updateAsync(int id, Poll poll, CancellationToken cancellationToken = default)
    {
        var currentPoll = await  GettByIdAsync(id, cancellationToken);
        if (currentPoll == null)
        {
            return false;
        }
        currentPoll.Title = poll.Title;
        currentPoll.Description = poll.Description;
        currentPoll.StartAT = poll.StartAT;
        currentPoll.EndAT = poll.EndAT;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> deleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var pollToRemove = await GettByIdAsync(id, cancellationToken);
        if (pollToRemove == null)
        {
            return false;
        }
        _context.Remove(pollToRemove);
        await _context.SaveChangesAsync( cancellationToken);
        return true;

    }
    public async Task<bool> TogglePublishSatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await GettByIdAsync(id, cancellationToken);
        if (poll == null)
        {
            return false;
        }
        poll.IsPublished = !poll.IsPublished;
        await _context.SaveChangesAsync();
        return true;
    }


}
