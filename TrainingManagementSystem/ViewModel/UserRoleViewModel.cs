namespace TMS_Application.ViewModel
{
    public class UserRoleViewModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }
        public int? ManagerId { get; set; }
        public string ManagerName { get; set; }

    }
}
