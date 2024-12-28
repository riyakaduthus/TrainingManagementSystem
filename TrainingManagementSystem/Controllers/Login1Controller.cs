using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TMS_Application.ViewModel;

namespace TMS_Application.Controllers
{
    public class JWT1
    {
        public string Token { get; set; }
    }
    public class Login1Controller : Controller
    {
        ILogger<Login1Controller> _logger;
        HttpClient client = new HttpClient();

        public Login1Controller(ILogger<Login1Controller> logger)
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
            return View(loginViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Login1(LoginViewModel user)
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
                    JWT1 jwt = JsonConvert.DeserializeObject<JWT1>(stringJWT);

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
