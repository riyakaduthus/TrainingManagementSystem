using Authentication_WebAPI.Context;
using TMS_WebAPI.IRepo;
using TMS_WebAPI.Models;

namespace TMS_WebAPI.Repo
{
    public class FeedbackRepository : IFeedbackRepo
    {
        AppDBContext _dbContext;
        public FeedbackRepository(AppDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public Feedback AddFeedback(Feedback feedback)
        {
            feedback.IsActive = true;
            _dbContext.Add(feedback);
            _dbContext.SaveChanges();

            return feedback;
        }

        public List<Feedback> GetFeedbacks()
        {
            return _dbContext.Feedbacks.Where(x=>x.IsActive == true).ToList();
        }
    }
}
