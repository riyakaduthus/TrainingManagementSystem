namespace TMS_WebAPI.ViewModel
{
    public class BatchViewModel
    {
        public int CourseId {  get; set; }
        public string CourseName { get; set; }
        public string BatchName { get; set; }
        public int BatchId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Availablity { get; set; }
        /// <summary>
        /// number persons can be accomedeated to the batch
        /// </summary>
        public int BatchCount { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }

        public DateTime? Updated { get; set; }
        public bool IsActive { get; set; }


    }
}
