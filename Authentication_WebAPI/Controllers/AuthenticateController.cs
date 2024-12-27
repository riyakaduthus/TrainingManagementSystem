using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Authentication_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TMS_WebAPI.IRepo;
using TMS_WebAPI.ViewModel;

namespace TMS_WebAPI.Controllers
{
    public class AuthenticateController : ControllerBase
    {
        IAuthenticate _repo;
        IConfiguration _config;
        public AuthenticateController(IAuthenticate repo, IConfiguration configuration)
        {
            _repo = repo;
            _config = configuration;
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            IActionResult response = Unauthorized();

            var user = _repo.AuthenticateUser(loginViewModel);
            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
