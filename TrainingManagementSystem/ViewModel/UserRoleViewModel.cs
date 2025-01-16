using System.ComponentModel;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace TMS_Application.ViewModel
{
    public class UserRoleViewModel
    {
        public int RoleId { get; set; }
        [DisplayName("Role Name")]
        public string RoleName { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }
        public int? ManagerId { get; set; }
        [DisplayName("Manager Name")]
        public string ManagerName { get; set; }
        public string EmailId { get; set; }
    }
}
