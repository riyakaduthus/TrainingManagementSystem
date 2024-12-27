using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TMS_Application.ViewModel;

namespace TMS_Application.Controllers
{
    public class JWT
    {
        public string Token { get; set; }
    }
    public class LoginController : Controller
    {
        ILogger<LoginController> _logger;
        HttpClient client = new HttpClient();
        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
            client.BaseAddress = new Uri("https://localhost:5244/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }
        public IActionResult Login()
        {
            LoginViewModel loginViewModel = new LoginViewModel();
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
                HttpResponseMessage response = await client.PostAsync("api/Authenticate", content);
                if (response.IsSuccessStatusCode)
                {
                    var stringJWT = response.Content.ReadAsStringAsync().Result;
                    JWT jwt = JsonConvert.DeserializeObject<JWT>(stringJWT);
                    
                    HttpContext.Session.SetString("token", jwt.Token);
                 
                    return RedirectToAction("Index", "Course");
                }
                else
                {
                    _logger.LogCritical("Somone not authenicated is trying to access application");
                    ViewBag.msg = "User do not exist";
                    return View();
                }
            }
            catch
            {
                return View();
            }



        }

    }
}
