using System.ComponentModel.DataAnnotations;

namespace TMS_Application.Models
{
    public class Batch
    {
        public int? BatchId { get; set; }
        [Display(Name ="Batch Name")]
        public string BatchName { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        /// <summary>
        /// number persons can be accomedeated to the batch
        /// </summary>
        /// 
        [Display(Name = "Batch Count")]
        public int BatchCount { get; set; }

        // this is to add a fk , CourseId becomes FK
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public List<Enrollment>? Enrollments { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? Updated { get; set; }

        public bool IsActive { get; set; } // is to perform soft delete
    }
}
