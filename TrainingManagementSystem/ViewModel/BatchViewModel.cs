using System.ComponentModel;

namespace TMS_Application.ViewModel
{
    public class BatchViewModel
    {
        public int CourseId { get; set; }
        [DisplayName("Course Name")]
        public string CourseName { get; set; }
        [DisplayName("Batch Name")]
        public string BatchName { get; set; }
        public int BatchId { get; set; }
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }
        //number persons can be accomedeated to the batch
        [DisplayName("Number of Maximum Participants")]
        public int BatchCount { get; set; }
        [DisplayName("Availablity of Batch")]
        public bool Availablity { get; set; }
        [DisplayName("Number of seats taken")]
        public int EnrolledCount {  get; set; }

    }
}
