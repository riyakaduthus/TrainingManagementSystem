﻿using System.Net.Http.Headers;
using System.Text;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TMS_Application.Models;

namespace TMS_Application.Controllers
{
    public class CourseController : Controller
    {
        HttpClient client = new HttpClient();
        static List<Course> course = null;
        public CourseController()
        {
            client.BaseAddress = new Uri("http://localhost:5244/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: CourseController
        public async Task<ActionResult> Index()
        {
           client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer",HttpContext.Session.GetString("token"));

            HttpResponseMessage response = await client.GetAsync("api/Course");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                course = JsonConvert.DeserializeObject<List<Course>>(jsonString.Result);
                if (course != null)
                {
                    return View(course);
                }
                else 
                { 
                    ViewBag.msg = "No course is there. Add a new Course!!!!"; 
                    return View();
                }
            }
            else
            {
                ViewBag.msg = response.ReasonPhrase;
                return View();
            }
        }

        // GET: CourseController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            HttpResponseMessage response = await client.GetAsync("api/Course/" + id);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                var course = JsonConvert.DeserializeObject<Course>(jsonString.Result);

                return View(course);
            }
            else
            {
                ViewBag.msg = response.ReasonPhrase;
                return View();
            }
        }

        // GET: CourseController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Course course)
        {
            course.CreatedBy = 1;
            course.CreatedOn = DateTime.Now;
            course.CourseId ??= 0;
            
            try
            {
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

                StringContent content = new StringContent
                    (JsonConvert.SerializeObject(course), Encoding.UTF8, "application/json");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = await client.PostAsync("api/Course", content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    var temp = JsonConvert.DeserializeObject<Course>(jsonString.Result);
                    if (temp != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.msg = "Some Error Occured. Please try adain later";
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

        // GET : CourseController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await client.GetAsync("api/Course/" + id);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                var course = JsonConvert.DeserializeObject<Course>(jsonString.Result);

                if (course != null)
                {
                    return View(course);
                }
                else {
                    ViewBag.msg = "Some Error occured!!!, Try again later";                       
                    return View(); }
            }
            else
            {
                ViewBag.msg = response.ReasonPhrase;
                return View();
            }
        }

        // POST: CourseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Course course)
        {
            course.Updated = DateTime.Now;
            course.UpdatedBy = 1;

            try
            {
                course.CourseId ??= 0;

                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

                StringContent content = new StringContent
                    (JsonConvert.SerializeObject(course), Encoding.UTF8, "application/json");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = await client.PutAsync("api/Course/" + id, content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    var temp = JsonConvert.DeserializeObject<Course>(jsonString.Result);
                    if (temp!=null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.msg = "some error occured,Try again Later";
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

        // GET: CourseController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await client.GetAsync("api/Course/" + id);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                var course = JsonConvert.DeserializeObject<Course>(jsonString.Result);

                return View(course);
            }
            else
            {
                ViewBag.msg = response.ReasonPhrase;
                return View();
            }
        }

        // POST: CourseController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Deleted(int id)
        {
            try
            {
                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));

                HttpResponseMessage response = await client.DeleteAsync("api/Course/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    var temp = JsonConvert.DeserializeObject<Course>(jsonString.Result);

                    return RedirectToAction(nameof(Index));
                }
                else
                {
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