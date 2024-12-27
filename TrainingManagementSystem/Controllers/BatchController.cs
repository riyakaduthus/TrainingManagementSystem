﻿using System.Net.Http.Headers;
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

        public BatchController()
        {
            client.BaseAddress = new Uri("http://localhost:5244/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: BatchController
        public async Task<ActionResult> Index()
        {
            List<BatchViewModel> batches = null;
            HttpResponseMessage response = await client.GetAsync("api/Batch");
            if (response.IsSuccessStatusCode)
            {
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

        // GET: BatchController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            HttpResponseMessage response = await client.GetAsync("api/Batch/" + id);
            if (response.IsSuccessStatusCode)
            {
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

        // GET: BatchController/Create
        public async Task<ActionResult> Create()
        {
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

        // POST: BatchController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Batch batch)
        {
            batch.CreatedBy = 1;
            batch.CreatedOn = DateTime.Now;
            batch.BatchId ??= 0;

            try
            {
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
                    if(temp != null){
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

        // GET : BatchController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
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

        // POST: BatchController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Batch batch)
        {
            batch.Updated = DateTime.Now;
            batch.UpdatedBy = 1;           
            batch.BatchId ??= 0;

            try
            {
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
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: BatchController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
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

        // POST: BatchController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Deleted(int id)
        {
            try
            {
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