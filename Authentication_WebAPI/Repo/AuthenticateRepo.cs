using Authentication_WebAPI.Context;
using Authentication_WebAPI.Models;
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
            var user = _dbContext.Users.FirstOrDefault(x => x.UserName == loginViewModel.UserName
            && x.Password == loginViewModel.Password && x.IsActive == true);

            return user;
        }
    }
}
