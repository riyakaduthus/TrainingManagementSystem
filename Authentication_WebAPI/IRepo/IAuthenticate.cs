using Authentication_WebAPI.Models;
using TMS_WebAPI.ViewModel;

namespace TMS_WebAPI.IRepo
{
    public interface IAuthenticate
    {
        User AuthenticateUser(LoginViewModel loginViewModel);
    }
}
