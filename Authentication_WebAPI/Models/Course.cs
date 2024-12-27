namespace Authentication_WebAPI.Models
{
    public class Course
    {
        /// <summary>
        /// ID of the course
        /// </summary>
        public int CourseId { get; set; }
        /// <summary>
        /// Name of the course
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// Description for the course
        /// </summary>
        public string CourseDescription { get; set; }
        /// <summary>
        /// Duration of the course in days
        /// </summary>
        public int DurationInDays {  get; set; }
        /// <summary>
        /// is the course is available or not
        /// </summary>
        public bool Availablity { get; set; }

        /// <summary>
        /// this is to link Batch Class
        /// </summary>
        public List<Batch>? Batches { get; set; }
        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? Updated { get; set; }

        public bool IsActive { get; set; } // is to perform soft delete

    }
}
