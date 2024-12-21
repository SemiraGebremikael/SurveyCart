
namespace SurveyCart.Api.Services
{
    public class PollService : IPollService
    {
        private static readonly List<Poll> _polls = [ new Poll
        {
        Id = 1,
        Title = "poll",
        Description ="first poll"
        }
      ];

  
        public IEnumerable<Poll> GettAll()
        {
           return _polls;
        }

        public Poll? GettById(int id)
        {
            if(id < 0 )
            {

                throw new ArgumentNullException("Id not found");
            }

            var poll = _polls.FirstOrDefault(pol => pol.Id == id);
            return poll;
        }

        public Poll Add(Poll poll)
        {
            poll.Id = _polls.Count + 1;  
            _polls.Add(poll);
            return poll;
        }

        public bool update( int id, Poll poll)
        {
            var currentPoll = _polls.FirstOrDefault(p => p.Id == id);
            if (currentPoll == null)
            {
                return false;
            }
            currentPoll.Title = poll.Title;
            currentPoll.Description = poll.Description;

            return true;
        }

        public bool delete(int id)
        {
            var pollToRemove = _polls.FirstOrDefault(p => p.Id == id);
            if (pollToRemove == null)
            {
                return false;
            }
            _polls.Remove(pollToRemove);
            return true;
               
        }
    }
}
