namespace TMS_WebAPI.ViewModel
{
    public class EnrollmentViewModel
    {
        public int EnrollmentId { get; set; }
        public int EnrollmentStatus { get; set; }
        public DateTime RequestedDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int BatchId { get; set; }
        public string BatchName {  get; set; }

    }
}
