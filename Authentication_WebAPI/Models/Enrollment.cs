using TMS_WebAPI.Models;

namespace Authentication_WebAPI.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int EnrollmentStatus { get; set; }
        public DateTime RequestedDate { get; set; }
        public int UserId {  get; set; }
        public User? User { get; set; }

        public int BatchId { get; set; }
        public Batch? Batch { get; set; }   

        public List<Feedback>? Feedbacks { get; set; }
        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? Updated { get; set; }

        public bool IsActive { get; set; } // is to perform soft delete



    }
}
