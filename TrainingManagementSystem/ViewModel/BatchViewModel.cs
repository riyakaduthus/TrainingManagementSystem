namespace TMS_Application.ViewModel
{
    public class BatchViewModel
    {
        public int CourseId {  get; set; }
        public string CourseName { get; set; }
        public string BatchName { get; set; }
        public int BatchId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //number persons can be accomedeated to the batch
        public int BatchCount { get; set; }
        public bool Availablity { get; set; }

    }
}
