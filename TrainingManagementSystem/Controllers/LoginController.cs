using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TMS_Application.ViewModel;

namespace TMS_Application.Controllers
{
    public class JWT
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
    public class LoginController : Controller
    {
        ILogger<LoginController> _logger;
        HttpClient client = new HttpClient();

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
            client.BaseAddress = new Uri("https://localhost:7206/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            ViewBag.HideHeader = true;
            return View(loginViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            try
            {
                StringContent content = new StringContent
                    (JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                var contentType = new MediaTypeWithQualityHeaderValue
("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = await client.PostAsync("api/Authentication", content);
                if (response.IsSuccessStatusCode)
                {
                    var stringJWT = response.Content.ReadAsStringAsync().Result;
                    JWT jwt = JsonConvert.DeserializeObject<JWT>(stringJWT);

                    HttpContext.Session.SetString("token", jwt.Token);
                    HttpContext.Session.SetString("userId", jwt.UserId.ToString());
                    HttpContext.Session.SetString("userName", jwt.UserName);

                    return RedirectToAction("Index", "Course");
                }
                else
                {
                    _logger.LogCritical("Someone not authenicated is trying to access application");
                    ViewBag.msg = "Unauthorized attempt to login!!!!! \n User do not exist";
                    ViewBag.HideHeader = true;
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");
            HttpContext.Session.Remove("userId");
            HttpContext.Session.Remove("userName");
            return RedirectToAction("Login");
        }
    }
}
