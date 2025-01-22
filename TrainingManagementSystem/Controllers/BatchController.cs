using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TMS_Application.Models;
using TMS_Application.ViewModel;

namespace TMS_Application.Controllers
{
    public class BatchController : Controller
    {
        HttpClient client = new HttpClient();
        static List<Batch> batch = null;
        static List<CourseViewModel> courseView = null;
        private void SetViewBag() => ViewBag.UserRole = HttpContext.Session.GetString("roleName");
        public BatchController()
        {
            client.BaseAddress = new Uri("https://localhost:7206/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
       
        public async Task<ActionResult> Index()
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            List<BatchViewModel> batches = null;
            HttpResponseMessage response = await client.GetAsync("api/Batch");
            if (response.IsSuccessStatusCode)
            {
                SetViewBag();
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                batches = JsonConvert.DeserializeObject<List<BatchViewModel>>(jsonString.Result);
                if (batches.Count == 0)
                {
                    ViewBag.msg = "No batch exists!!!";
                    return View();
                }
                else
                {
                    return View(batches);
                }
            }
            else
            {
                ViewBag.msg = response.ReasonPhrase;
                return View();
            }
        }

        public async Task<ActionResult> ViewAvailableBatches()
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            List<BatchViewModel> batches = null;
            HttpResponseMessage response = await client.GetAsync("api/Batch/GetAvailableBatches");
            if (response.IsSuccessStatusCode)
            {
                SetViewBag();
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                batches = JsonConvert.DeserializeObject<List<BatchViewModel>>(jsonString.Result);
                if (batches.Count == 0)
                {
                    ViewBag.msg = "No batch exists!!!";
                    return View();
                }
                else
                {
                    return View(batches);
                }
            }
            else
            {
                ViewBag.msg = response.ReasonPhrase;
                return View();
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage response = await client.GetAsync("api/Batch/" + id);
            if (response.IsSuccessStatusCode)
            {
                SetViewBag();
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                var batch = JsonConvert.DeserializeObject<BatchViewModel>(jsonString.Result);

                if (batch == null)
                {
                    ViewBag.msg = "No Such data exists";
                    return View();
                }
                else
                {
                    return View(batch);
                }
            }
            else
            {
                ViewBag.msg = response.ReasonPhrase;
                return View();
            }
        }
        
        public async Task<ActionResult> Create()
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage response = await client.GetAsync("api/Batch/GetCourseNames");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                courseView = JsonConvert.DeserializeObject<List<CourseViewModel>>(jsonString.Result);
            }
            if (courseView == null || !courseView.Any())
            {
                ViewBag.Msg = "No courses available";
            }

            ViewBag.courses = new SelectList(courseView, "CourseId", "CourseName");

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Batch batch)
        {
            batch.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("userId"));
            batch.CreatedOn = DateTime.Now;
            batch.BatchId ??= 0;

            try
            {
                client.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

                StringContent content = new StringContent
                    (JsonConvert.SerializeObject(batch), Encoding.UTF8, "application/json");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = await client.PostAsync("api/Batch", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    var temp = JsonConvert.DeserializeObject<Batch>(jsonString.Result);
                    if (temp != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.msg = "Some error occured!! Try later";
                        return View();
                    }
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ViewBag.msg = "Error: " + response.StatusCode + " - " + errorMessage;
                    return View(batch);
                    //ViewBag.msg = response.ReasonPhrase;
                    //return View();
                }
            }
            catch
            {
                return View();
            }

        }
       
        public async Task<ActionResult> Edit(int id)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage response = await client.GetAsync("api/Batch/GetCourseNames");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                courseView = JsonConvert.DeserializeObject<List<CourseViewModel>>(jsonString.Result);
            }
            if (courseView == null || !courseView.Any())
            {
                ViewBag.Msg = "No courses available";
            }

            ViewBag.courses = new SelectList(courseView, "CourseId", "CourseName");

            HttpResponseMessage response1 = await client.GetAsync("api/Batch/" + id);
            if (response1.IsSuccessStatusCode)
            {
                var jsonString = response1.Content.ReadAsStringAsync();
                jsonString.Wait();
                var batch = JsonConvert.DeserializeObject<Batch>(jsonString.Result);

                return View(batch);
            }
            else
            {
                ViewBag.msg = response1.ReasonPhrase;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Batch batch)
        {
            batch.Updated = DateTime.Now;
            batch.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("userId"));
            batch.BatchId ??= 0;

            try
            {
                client.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

                StringContent content = new StringContent
                    (JsonConvert.SerializeObject(batch), Encoding.UTF8, "application/json");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = await client.PutAsync("api/Batch/" + id, content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    var temp = JsonConvert.DeserializeObject<Batch>(jsonString.Result);
                    return RedirectToAction(nameof(Index));
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
                
        public async Task<ActionResult> Delete(int id)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

            HttpResponseMessage response = await client.GetAsync("api/Batch/" + id);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                var batch = JsonConvert.DeserializeObject<Batch>(jsonString.Result);

                return View(batch);
            }
            else
            {
                ViewBag.msg = response.ReasonPhrase;
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Deleted(int id)
        {
            try
            {
                client.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

                HttpResponseMessage response = await client.DeleteAsync("api/Batch/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    var temp = JsonConvert.DeserializeObject<Batch>(jsonString.Result);

                    return RedirectToAction(nameof(Index));
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

    }
}
