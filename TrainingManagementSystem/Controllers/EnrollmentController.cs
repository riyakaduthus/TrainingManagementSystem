using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using TMS_Application.Enums;
using TMS_Application.Models;
using TMS_Application.ViewModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
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
            client.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            List<EnrollmentViewModel> enrollments = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/Enrollment");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    enrollments = JsonConvert.DeserializeObject<List<EnrollmentViewModel>>(jsonString);
                    //enrollments.ForEach(x=>x.EnrollmentStatusName =                     
                    //Enum.GetName(typeof(EnrollmentStatus), x.EnrollmentStatus));    

                    enrollments.ForEach(x =>
                    {
                        var memberInfo = ((EnrollmentStatus)x.EnrollmentStatus)
                                            .GetType()
                                            .GetMember(((EnrollmentStatus)x.EnrollmentStatus).ToString())
                                            .FirstOrDefault();

                        x.EnrollmentStatusName = memberInfo?.GetCustomAttribute<DisplayAttribute>()?.Name ?? x.EnrollmentStatus.ToString();
                    });
                    return View(enrollments);
                }
                else
                {
                    ViewBag.msg = "Error fetching enrollments";
                    return View();
                }

            }
            catch (Exception ex)
            {
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RequestEnrollment(int batchId)
        {
            try
            {
                int managerId = 0;
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

                    return RedirectToAction("Index", "Batch");
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
        public async Task<ActionResult> Details(int id)
        {
            client.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            EnrollmentViewModel enrollment = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync($"api/Enrollment/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    enrollment = JsonConvert.DeserializeObject<EnrollmentViewModel>(jsonString);

                    var memberInfo = ((EnrollmentStatus)enrollment.EnrollmentStatus)
                                            .GetType()
                                            .GetMember(((EnrollmentStatus)enrollment.EnrollmentStatus).ToString())
                                            .FirstOrDefault();

                    enrollment.EnrollmentStatusName = memberInfo?.GetCustomAttribute<DisplayAttribute>()?.Name ?? enrollment.EnrollmentStatus.ToString();

                    return View(enrollment);
                }
                else
                {
                    ViewBag.msg = "Error fetching enrollment details";
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            client.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            EnrollmentViewModel enrollment = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync($"api/Enrollment/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    enrollment = JsonConvert.DeserializeObject<EnrollmentViewModel>(jsonString);

                    ViewBag.EnrollmentStatus = new SelectList(
                                    Enum.GetValues(typeof(EnrollmentStatus)).
                                    Cast<EnrollmentStatus>().
                                    Select(v => new SelectListItem
                                    {
                                        Text = v.GetType().
                                                GetMember(v.ToString()).First().
                                                GetCustomAttribute<DisplayAttribute>()?.Name ?? v.ToString(),
                                        Value = ((int)v).ToString()
                                    }), "Value", "Text", enrollment.EnrollmentStatus.ToString());
                    return View(enrollment);
                }
                else
                {
                    ViewBag.msg = "Error fetching enrollment details";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.msg = "An unexpected error occurred. Please try again later.";
                return View(Index);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Enrollment enrollment)
        {
            client.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            try
            {
                enrollment.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("userId"));
                enrollment.Updated = DateTime.Now;
                if (DateTime.TryParseExact(enrollment.RequestedDate.ToString(),
                                               "dd/MM/yyyy",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out DateTime parsedDate))
                {
                    enrollment.RequestedDate = parsedDate;
                }
                else
                {
                    ModelState.AddModelError("RequestedDate", "Invalid date format.");
                }
                StringContent content = new StringContent(JsonConvert.SerializeObject(enrollment), Encoding.UTF8, "application/json");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);

                HttpResponseMessage response = await client.PutAsync($"api/Enrollment/ {enrollment.EnrollmentId}", content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    var temp = JsonConvert.DeserializeObject<Enrollment>(jsonString.Result);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.msg = "Error updating enrollment";
                    return View();
                }

            }
            catch (Exception ex)
            {
                ViewBag.msg = "An unexpected error occurred. Please try again later.";
                return View();
            }
        }
    }
}
