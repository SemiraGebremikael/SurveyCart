using SurveyCart.Api.Contracts.Questions;

namespace SurveyCart.Api.Services
{
    public interface IQuestionService
    {
        Task<Result<IEnumerable<QuestionResponse>>> GetAll(int pollId, CancellationToken cancellationToken = default);
        Task<Result<QuestionResponse>> GetAsync(int pollId,int id,  CancellationToken cancellationToken = default);

        Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default);
         Task<Result> UpdatedAsync(int pollId, int id, QuestionRequest request,CancellationToken cancellationToken = default);
        Task<Result> ToggleSatusAsync(int pollId,int id, CancellationToken cancellationToken = default);

    }
}
