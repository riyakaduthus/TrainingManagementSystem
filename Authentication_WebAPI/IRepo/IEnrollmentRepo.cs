using Authentication_WebAPI.Models;
using Microsoft.AspNetCore.Components.Forms;
using TMS_WebAPI.Models;
using TMS_WebAPI.ViewModel;

namespace TMS_WebAPI.IRepo
{
    public interface IEnrollmentRepo
    {
        List<Enrollment> GetEnrollments();
        Enrollment GetEnrollmentByEnrollmentId(int id);
        Enrollment AddEnrollment(Enrollment enrollment);
        bool UpdateEnrollmentStatus(int id, Enrollment enrollment);
        //No delete as enrolllment can't be deleted

        List<EnrollmentViewModel> GetEnrollmentViews();
    }    

    public interface IFeedbackRepo
    {
        List<Feedback> GetFeedbacks();
        Feedback AddFeedback(Feedback feedback);
    }
}
