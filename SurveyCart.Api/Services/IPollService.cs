namespace SurveyCart.Api.Services
{
    public interface IPollService
    {
        IEnumerable<Poll> GettAll();
        Poll GettById(int id);

        Poll Add(Poll poll);

       bool update( int id , Poll poll);

       bool delete(int id );
    }
}
