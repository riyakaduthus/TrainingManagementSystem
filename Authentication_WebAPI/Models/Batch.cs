namespace Authentication_WebAPI.Models
{
    public class Batch
    {
        public int BatchId { get; set; }
        public string BatchName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //number persons can be accomedeated to the batch
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
