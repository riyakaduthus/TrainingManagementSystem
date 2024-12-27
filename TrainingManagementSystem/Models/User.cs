using System.ComponentModel.DataAnnotations.Schema;

namespace TMS_Application.Models
{
    public class User
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }

        // self join
        [ForeignKey("ManagerId")]
        public int? ManagerId { get; set; }
        public User? Manager { get; set; }
        public List<Enrollment>? Enrollments { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public bool IsActive { get; set; }
    }
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<User>? Users { get; set; }
    }
}
