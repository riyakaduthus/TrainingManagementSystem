using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TMS_Application.ViewModel
{
    public class EnrollmentViewModel
    {
        public int EnrollmentId { get; set; }
        public int? UserId { get; set; }
        public int? ManagerId { get; set; }
        public int BatchId { get; set; }
        public int EnrollmentStatus { get; set; }
        [DisplayName("Requested Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RequestedDate { get; set; }
        /// <summary>
        /// for displaying the status in the view
        /// </summary>
        /// 
        [DisplayName("Enrollment Status")]
        public string EnrollmentStatusName { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [DisplayName("Manager Name")]
        public string ManagerName { get; set; }
        [DisplayName("Batch Name")]
        public string BatchName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? Updated
        {
            get; set;
        }
        public bool IsActive { get; set; }

    }
}
