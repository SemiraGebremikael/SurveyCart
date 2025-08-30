//using Microsoft.Extensions.Caching.Hybrid;

using SurveyCart.Api.Contracts.Questions;

namespace SurveyCart.Api.Services
{
    public class QuestionService(
        ApplicationDbContext applicationDb, 
        ILogger<QuestionService> logger
       //, _hybridCache = hybridCache;
     ) : IQuestionService
    {
        public readonly ApplicationDbContext _context = applicationDb;
        private readonly ILogger<QuestionService> _logger = logger;
        //private readonly HybridCache _hybridCache;
        private const string _cachePrefix = "availableQuestions";


        public async Task<Result<IEnumerable<QuestionResponse>>> GetAll(int pollId, string userId,  CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
   

        //{
        //    try
        //    {
        //        var cacheKey = $"{_cachePrefix}-{pollId}";
        //        var pollExistsCacheKey = $"pollExists-{pollId}";
        //        var pollIsExists = await _hybridCache.GetOrCreateAsync<bool>(
        //            pollExistsCacheKey,
        //            async cacheEntry =>
        //            {
        //                //cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10)); // Cache for 10 minutes
        //                return await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);
        //            }
        //        );

        //        if (!pollIsExists)
        //        {
        //            return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNoFound);
        //        }

        //        var questions = await _hybridCache.GetOrCreateAsync<IEnumerable<QuestionResponse>>(
        //            cacheKey,
        //            async cacheEntry =>
        //            {
        //                _logger.LogInformation($"Cache miss: Fetching from DB and storing in cache for key: {cacheKey}");

        //                var data = await _context.Questions
        //                    .Where(x => x.PollId == pollId && x.isActive)
        //                    .Include(x => x.Answers)
        //                    .ProjectToType<QuestionResponse>()
        //                    .AsNoTracking()
        //                    .ToListAsync(cancellationToken);
        //                _logger.LogInformation($"Data cached successfully for key: {cacheKey}");
        //                return data;
        //            }
        //        );

        //        _logger.LogInformation($"Returning data for key: {cacheKey}");
        //        return Result.Success(questions);
        //    }
        //    catch (Exception ex) when (ex is not OperationCanceledException)
        //    {
        //        _logger.LogError(ex, $"Failed process to get all questions {pollId}", ex.Message);
        //        throw;
        //    }
        //}


        public async Task<Result<QuestionResponse>> GetAsync(int pollId, int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var question = await _context.Questions
             .Where(x => x.PollId == pollId && x.Id == id)
             .Include(x => x.Answers)
             .ProjectToType<QuestionResponse>()
             .AsNoTracking()
             .SingleOrDefaultAsync(cancellationToken);

                if (question == null)
                {
                    return Result.Failure<QuestionResponse>(QuestionErrors.questionNoFound);
                }
                return Result.Success<QuestionResponse>(question);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, $"Failed process to get a questions {pollId},  {id}", ex.Message);
                throw;
            }

        }

        public async  Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var pollIsExists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken: cancellationToken);
                if (!pollIsExists)
                {
                    return Result.Failure<QuestionResponse>(PollErrors.PollNoFound);
                }

                var questionIsExists = await _context.Questions.AnyAsync(x => x.Content == request.Content && x.PollId == pollId, cancellationToken: cancellationToken);
                if (questionIsExists)
                {
                    return Result.Failure<QuestionResponse>(QuestionErrors.questionNoFound);
                }

                var question = request.Adapt<Question>();
                question.PollId = pollId;
                await _context.AddAsync(question, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                //await _hybridCache.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);
                return Result.Success(question.Adapt<QuestionResponse>());
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, $"Failed process for add a questions {pollId},  {request}", ex.Message);
                throw;
            }

        }

        public  async Task<Result> UpdatedAsync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var questionIsExists = await _context.Questions.AnyAsync(x => x.PollId == pollId
                                                                     &&    x.Id !=id && x.Content ==request.Content, cancellationToken: cancellationToken);
                if (questionIsExists)
                {
                    return Result.Failure<QuestionResponse>(QuestionErrors.questionNoFound);
                }
                var question = await _context.Questions.Include(x => x.Answers)
                                                       .SingleOrDefaultAsync(x => x.PollId == pollId && x.Id == id, cancellationToken: cancellationToken);
                if (question == null)
                {
                    return Result.Failure<QuestionResponse>(QuestionErrors.questionNoFound);
                }
                question.Content = request.Content;

                var currentAnswers = question.Answers.Select(x => x.Content).ToList();
                var newAnswers = request.Answers.Except(currentAnswers).ToList();
                newAnswers.ForEach(answer =>
                {
                    question.Answers.Add(new Answer
                    {
                        Content = answer
                    });
                    question.Answers.ToList().ForEach(answer =>
                    {
                        answer.isActive = request.Answers.Contains(answer.Content);
                    });
                });

                await _context.SaveChangesAsync(cancellationToken);
                //await _hybridCache.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);

                return Result.Success();
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, $"Failed process for update a questions {pollId}, {id} , {request}", ex.Message);
                throw;
            }

        }

        public async  Task<Result> ToggleSatusAsync(int pollId, int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var question = await _context.Questions.SingleOrDefaultAsync(x => x.PollId == pollId && x.Id == id, cancellationToken);
                if (question == null)
                {
                    return Result.Failure(QuestionErrors.questionNoFound);
                }
                question.IsActive= !question.IsActive;
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, $"Failed process for toggle status a questions {pollId}, {id}", ex.Message);
                throw;
            }

        }
    }
}
