namespace SurveyCart.Api.Services
{
    public interface INotificationService
    {
        Task SendNewPollNotification(int? pollId = null);
    }
}
