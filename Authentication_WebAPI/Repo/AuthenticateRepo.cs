using Authentication_WebAPI.Context;
using Authentication_WebAPI.Models;
using Microsoft.AspNetCore.SignalR;
using TMS_WebAPI.IRepo;
using TMS_WebAPI.ViewModel;

namespace TMS_WebAPI.Repo
{
    public class AuthenticateRepo : IAuthenticate
    {
        AppDBContext _dbContext;
        public AuthenticateRepo(AppDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public User AuthenticateUser(LoginViewModel loginViewModel)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.EmailId == loginViewModel.EmailId
            && x.Password == loginViewModel.Password && x.IsActive == true);

            return user;
        }

        public List<Role> GetAllRoles()
        {
            return _dbContext.Roles.ToList();
        }

        public string GetRoleName(int roleId)
        {
            string temp = (from x in _dbContext.Roles
                           where x.RoleId == roleId
                           select x.RoleName).FirstOrDefault();

            return temp.ToString();
        }
    }
}
