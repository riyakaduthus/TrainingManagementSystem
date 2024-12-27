using Authentication_WebAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TMS_WebAPI.Models
{
    public class Feedback
    {
        public int FeedbackId {  get; set; }
        public string FeedbackText { get; set; }

        public DateTime CreatedDate { get; set; }

        public int EnrollmentId { get; set; }

        public Enrollment? Enrollment { get; set; }
        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? Updated { get; set; }

        public bool IsActive { get; set; } // is to perform soft delete
    }
}
