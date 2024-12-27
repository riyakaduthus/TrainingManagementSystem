using Authentication_WebAPI.Models;
using TMS_WebAPI.ViewModel;

namespace TMS_WebAPI.IRepo
{
    public interface IUserRepo
    {       
        User GetUserById(int id);
        User AddUser(User user);
        bool UpdateUser(int UserId, User user);
        bool DeleteUser(int UserId);
        List<UserRoleViewModel> GetUsers();
        UserRoleViewModel GetUserDetailsById(int UserId);
        List<Role> GetRoles();

        List<UserRoleViewModel> GetManagerNames();
        List<UserRoleViewModel> GetManagers(int id);

        //bool AssignRole(int id);
    }
}
