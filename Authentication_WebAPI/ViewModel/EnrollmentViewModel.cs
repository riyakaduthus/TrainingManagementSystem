using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TMS_WebAPI.ViewModel
{
    public class EnrollmentViewModel
    {
        public int EnrollmentId { get; set; }
        public int? UserId { get; set; }
        public int? ManagerId { get; set; }
        public int BatchId { get; set; }
        public int EnrollmentStatus { get; set; }
        public DateTime RequestedDate { get; set; }
        public string EnrollmentStatusName { get; set; }
        public string UserName { get; set; }
        public string ManagerName { get; set; }
        public string BatchName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public int? UpdatedOn
        {
            get; set;
        }
        public bool IsActive { get; set; }
    }
}
