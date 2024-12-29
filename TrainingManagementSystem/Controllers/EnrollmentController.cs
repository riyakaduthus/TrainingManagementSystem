using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TMS_Application.ViewModel;

namespace TMS_Application.Controllers
{
    public class EnrollmentController : Controller
    {
        HttpClient client = new HttpClient();
        public EnrollmentController()
        {
            client.BaseAddress = new Uri("http://localhost:5244/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
               
        public async Task<ActionResult> Index()
        {
            List<EnrollmentViewModel> enroll = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/Enrollment");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    enroll = JsonConvert.DeserializeObject<List<EnrollmentViewModel>>(jsonString.Result);
                    if (enroll != null)
                    {
                        return View(enroll);
                    }
                    else
                    {
                        ViewBag.msg = "No Enrollments till now!!!";
                        return View();
                    }
                }
                else
                {
                    ViewBag.msg = response.ReasonPhrase;
                    return View();
                }
            }
            catch 
            {
                return View();
            }
        }

        // GET: EnrollmentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EnrollmentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EnrollmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Enrollment(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EnrollmentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EnrollmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EnrollmentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EnrollmentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
