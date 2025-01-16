using System.ComponentModel.DataAnnotations;

namespace TMS_Application.Enums
{
    public enum EnrollmentStatus
    {
        [Display(Name = "Requested for Enrollment")]
        Requested = 1,
        [Display(Name = "Rejected by manager")]
        RequestedButNotApproved = 2,
        [Display(Name = "Requested but pending with manager")]
        RequestedButPending = 3
    }
}
