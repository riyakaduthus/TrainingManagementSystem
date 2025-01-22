using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authentication_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TMS_WebAPI.IRepo;
using TMS_WebAPI.ViewModel;

namespace TMS_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        IAuthenticate _repo;
        IConfiguration _config;
        string roleName = string.Empty;
        public AuthenticationController(IAuthenticate repo, IConfiguration configuration)
        {
            _repo = repo;
            _config = configuration;
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            IActionResult response = Unauthorized();
            if (ModelState.IsValid)
            {
                var user = _repo.AuthenticateUser(loginViewModel);
                if (user != null)
                {
                    roleName = _repo.GetRoleName(user.RoleId);

                    var tokenString = GenerateJSONWebToken(user);
                    response = Ok(new { token = tokenString, user.UserId, user.UserName, roleName });
                }
                else
                {
                    response = BadRequest("Invalid username or password.");
                }
            }
            else
            {
                response = BadRequest("Invalid login request.");
            }

            return response;
        }

        private string GenerateJSONWebToken(User user)
        {
            
            List<Role> roles = new List<Role>();
            roles = _repo.GetAllRoles();

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.EmailId),
                new Claim(ClaimTypes.Role, roleName),
                new Claim(type:"Date", DateTime.Now.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()),
                  new Claim(JwtRegisteredClaimNames.Sid,user.UserId.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
