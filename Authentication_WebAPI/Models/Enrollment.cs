using System.ComponentModel.DataAnnotations.Schema;
using TMS_WebAPI.Models;

namespace Authentication_WebAPI.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int EnrollmentStatus { get; set; }
        public DateTime RequestedDate { get; set; }
        [ForeignKey(nameof(User)), Column(Order = 0)]
        public int? UserId {  get; set; }

        public virtual User? User { get; set; }

        public int BatchId { get; set; }
        public Batch? Batch { get; set; }        
        public List<Feedback>? Feedbacks { get; set; }
        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? Updated { get; set; }

        [ForeignKey(nameof(Manager)), Column(Order = 1)]
        public int? ManagerId { get; set; }
        public bool IsActive { get; set; } // is to perform soft delete

       // [ForeignKey("ManagerId")]
       
        public virtual User? Manager { get; set; }
        
    }
}
