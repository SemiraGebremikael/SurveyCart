
using Azure.Core;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SurveyCart.Api.Entities;

namespace SurveyCart.Api.Services;

public class PollService : IPollService
{
    public  readonly ApplicationDbContext _context;
    public readonly ILogger<PollService> _logger;
    public PollService( ApplicationDbContext applicationDb, ILogger<PollService> logger)
    {
        _context = applicationDb;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<PollResponse>>> GettAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var polls = await _context.Polls
           .AsNoTracking()
           .ProjectToType<PollResponse>()
           .ToListAsync(cancellationToken);
            return Result.Success(polls.Adapt<IEnumerable<PollResponse>>());
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, $"Failed process to get all polls", ex.Message);
                throw; 
        }
    }
       


    

    public async Task<Result<IEnumerable<PollResponse>>> GetCurrentAsync(CancellationToken cancellationToken = default)
    {

        try
        {
            var currentPolls = await _context.Polls
          .Where(x => x.IsPublished && x.StartAT <= DateOnly.FromDateTime(DateTime.UtcNow) && x.EndAT >= DateOnly.FromDateTime(DateTime.UtcNow))
          .AsNoTracking()
          .ProjectToType<PollResponse>()
          .ToListAsync(cancellationToken);

            return Result.Success(currentPolls.Adapt<IEnumerable<PollResponse>>());

        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, $"Failed to process reponse current polls", ex.Message);
            throw;
        }
      

    }

    public async Task<Result<PollResponse>> GettByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var poll = await _context.Polls.FindAsync(id, cancellationToken);
            if (poll == null)
            {
                return Result.Failure<PollResponse>(PollErrors.PollNoFound);
            }
            return Result.Success(poll.Adapt<PollResponse>());

        }

        catch (Exception ex) when (ex is not OperationCanceledException)
        { 
           _logger.LogError(ex, $"Failed to process response for poll ID {id}", ex.Message);
          throw;

        }


    }

    public async Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellationToken = default)
    {

        try
        {
            var isExistingTitle = await _context.Polls.AnyAsync(x => x.Title == request.Title, cancellationToken: cancellationToken);
            if (isExistingTitle)
            {
                Result.Failure<PollResponse>(PollErrors.DublicatedPollTitle);
            }

            var poll = request.Adapt<Poll>();
            await _context.AddAsync(poll, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success(request.Adapt<PollResponse>());
        }
        catch (Exception ex) when(ex is not OperationCanceledException)
        {
            _logger.LogError(ex, $"Failed process response for add poll{request}", ex.Message);
            throw;
        }
      
    }

    public async Task<Result> updateAsync(int id, PollRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var isExistingTitle = await _context.Polls.AnyAsync(x => x.Title == request.Title && x.Id != id, cancellationToken: cancellationToken);
            if (isExistingTitle)
            {
                Result.Failure<PollResponse>(PollErrors.DublicatedPollTitle);
            }


            var currentPoll = await _context.Polls.FindAsync(id, cancellationToken);
            if (currentPoll == null)
            {
                return Result.Failure(PollErrors.PollNoFound);
            }
            currentPoll.Title = request.Title;
            currentPoll.Description = request.Description;
            currentPoll.StartAT = request.StartAT;
            currentPoll.EndAT = request.EndAT;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }
        catch (Exception ex) when (ex is OperationCanceledException) 
        {
            _logger.LogError(ex, $"Failed process for update poll {id}, {request}", ex.Message);
            throw;
        }
        
      
    }

    public async Task<Result> deleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var pollToRemove = await _context.Polls.FindAsync(id, cancellationToken);
            if (pollToRemove == null)
            {
                return Result.Failure(PollErrors.PollNoFound);
            }
            _context.Remove(pollToRemove);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex) when (ex is OperationCanceledException) 
        {
            _logger.LogError(ex, $"Failed process for delete poll {id}");
            throw;
        }
      

    }
    public async Task<Result> TogglePublishSatusAsync(int id, CancellationToken cancellationToken = default)
    {

        try {
            var poll = await _context.Polls.FindAsync(id, cancellationToken);
            if (poll == null)
            {
                return Result.Failure(PollErrors.PollNoFound);
            }
            poll.IsPublished = !poll.IsPublished;
            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception ex) when (ex is OperationCanceledException)
        {
            _logger.LogError(ex, $"Failed process for publish poll {id}");
            throw;
        }
    }


}
