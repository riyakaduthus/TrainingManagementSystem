using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TMS_Application.Enums;
using TMS_Application.Models;
using TMS_Application.ViewModel;
namespace TMS_Application.Controllers
{
    public class EnrollmentController : Controller
    {
        HttpClient client = new HttpClient();
        public EnrollmentController()
        {
            client.BaseAddress = new Uri("https://localhost:7206/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
               
        public async Task<ActionResult> Index()
        {
            return View();
        }

        // GET: EnrollmentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

               // POST: EnrollmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Enrollment1(int batchId)
        {
            try
            {
                int managerId = 0;

                // Ensure token and userId are available in the session
                var token = HttpContext.Session.GetString("token");
                var userIdString = HttpContext.Session.GetString("userId");

                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userIdString))
                {
                    ViewBag.msg = "Session expired. Please log in again.";
                    return View();
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                int UserId = Convert.ToInt32(userIdString);

                // Get ManagerId using UserId
                HttpResponseMessage response = await client.GetAsync($"api/Enrollment/GetManagerId/{UserId}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    managerId = JsonConvert.DeserializeObject<int>(jsonString);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ViewBag.msg = $"Error fetching ManagerId: {response.StatusCode} - {errorMessage}";
                    return View();
                }

                // Create the Enrollment object
                var enrollment = new Enrollment
                {
                    EnrollmentStatus = (int)EnrollmentStatus.Requested,
                    RequestedDate = DateTime.Now,
                    BatchId = batchId,
                    UserId = UserId,
                    CreatedBy = UserId,
                    CreatedOn = DateTime.Now,
                    IsActive = true,
                    ManagerId = managerId
                };

                // Send the Enrollment object to the API
                StringContent content = new StringContent(JsonConvert.SerializeObject(enrollment), Encoding.UTF8, "application/json");
                HttpResponseMessage res = await client.PostAsync("api/Enrollment", content);

                if (res.IsSuccessStatusCode)
                {
                    var jsonResponse = await res.Content.ReadAsStringAsync();
                    var enrolledBatch = JsonConvert.DeserializeObject<Enrollment>(jsonResponse);

                    return RedirectToAction(nameof(Index)); // Redirect after success
                }
                else
                {
                    var errorMessage = await res.Content.ReadAsStringAsync();
                    ViewBag.msg = $"Error enrolling in batch: {res.StatusCode} - {errorMessage}";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = "An unexpected error occurred. Please try again later.";
                return View();
            }
        }


        // GET: EnrollmentController/Edit/5
       
    }
}
