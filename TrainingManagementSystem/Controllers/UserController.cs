using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using TMS_Application.Models;
using TMS_Application.ViewModel;

namespace TMS_Application.Controllers
{
    public class UserController : Controller
    {
        HttpClient client = new HttpClient();
        static List<User> user = null;

        public UserController()
        {
            client.BaseAddress = new Uri("http://localhost:5244/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // GET: UserController
        public async Task<ActionResult> Index()
        {
            List<UserRoleViewModel> user = null;
            try {
                HttpResponseMessage response = await client.GetAsync("api/User");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    user = JsonConvert.DeserializeObject<List<UserRoleViewModel>>(jsonString.Result);
                    if (user !=null)
                    {
                        return View(user);
                    }
                    else
                    {
                        ViewBag.msg = "No User exists";
                        return View();
                        
                    }
                }
                else
                {
                    ViewBag.msg = response.ReasonPhrase;
                    return View();
                }
            }
            catch { return View(); }
        }

        // GET: UserController/Details/5
        public async Task<ActionResult> Details(int id)
        {            
            try 
            {
                HttpResponseMessage response = await client.GetAsync("api/User/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    var user = JsonConvert.DeserializeObject<UserRoleViewModel>(jsonString.Result);

                    if (user == null)
                    {
                        ViewBag.msg = "No Such data exists";
                        return View();
                    }
                    else
                    {
                        return View(user);
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

        // GET: UserController/Create
        public async Task<ActionResult> Create()
        {
            List<Role> roles = null;
            List<UserRoleViewModel> manager = null;
            HttpResponseMessage response = await client.GetAsync("api/User/GetRoles");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                roles = JsonConvert.DeserializeObject<List<Role>>(jsonString.Result);
            }
            if (roles == null || !roles.Any())
            {
                ViewBag.Msg = "No Role is available";
            }
            ViewBag.roles = new SelectList(roles, "RoleId", "RoleName");

             
            HttpResponseMessage msg = await client.GetAsync("api/User/GetManagerName");
            if (msg.IsSuccessStatusCode)
            {
                var jsonString = msg.Content.ReadAsStringAsync();
                jsonString.Wait();
                manager = JsonConvert.DeserializeObject<List<UserRoleViewModel>>(jsonString.Result);
            }
            if (manager == null || !manager.Any())
            {
                ViewBag.Msg = "No Manager is available";
            }
            ViewBag.manager = new SelectList(manager, "ManagerId", "ManagerName");


            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(User usr)
        {
            usr.CreatedBy = 1;
            usr.CreatedOn=DateTime.Now;
            usr.IsActive= true;
            usr.UserId ??= 0;
            try
            {
                StringContent content = new StringContent
                    (JsonConvert.SerializeObject(usr), Encoding.UTF8, "application/json");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = await client.PostAsync("api/User", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    var temp = JsonConvert.DeserializeObject<User>(jsonString.Result);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ViewBag.msg = "Error: " + response.StatusCode + " - " + errorMessage;
                    return View(usr);
                    //ViewBag.msg = response.ReasonPhrase;
                    //return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            List<Role> roles = null;
            List<UserRoleViewModel> manager = null; 
           
            #region Roles
            HttpResponseMessage response = await client.GetAsync("api/User/GetRoles");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                roles = JsonConvert.DeserializeObject<List<Role>>(jsonString.Result);
            }
            if (roles == null || !roles.Any())
            {
                ViewBag.Msg = "No Role is available";
            }
            ViewBag.roles = new SelectList(roles, "RoleId", "RoleName");

            #endregion

            #region Manager
            HttpResponseMessage msg = await client.GetAsync("api/User/managers/" + id);
            if (msg.IsSuccessStatusCode)
            {
                var jsonString = msg.Content.ReadAsStringAsync();
                jsonString.Wait();
                manager = JsonConvert.DeserializeObject<List<UserRoleViewModel>>(jsonString.Result);
            }
            if (manager == null || !manager.Any())
            {
                ViewBag.Msg = "No Manager is available";
            }
            ViewBag.manager = new SelectList(manager, "ManagerId", "ManagerName");

            #endregion
            #region Userdata
            HttpResponseMessage response1 = await client.GetAsync("api/User/" + id);
            if (response1.IsSuccessStatusCode)
            {
                var jsonString = response1.Content.ReadAsStringAsync();
                jsonString.Wait();
                var user = JsonConvert.DeserializeObject<User>(jsonString.Result);

                return View(user);
            }
            else
            {
                ViewBag.msg = response1.ReasonPhrase;
                return View();
            }
            #endregion
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, User user)
        {
            user.Updated = DateTime.Now;
            user.UpdatedBy = 1;
            user.UserId ??= 0;
            user.IsActive = true;

            try
            {
                StringContent content = new StringContent
                   (JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = await client.PutAsync("api/User/" + id, content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    var temp = JsonConvert.DeserializeObject<User>(jsonString.Result);
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

        // GET: UserController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await client.GetAsync("api/User/" + id);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                var user = JsonConvert.DeserializeObject<User>(jsonString.Result);

                return View(user);
            }
            else
            {
                ViewBag.msg = response.ReasonPhrase;
                return View();
            }
        }

        // POST: UserController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Deleted(int id)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync("api/User/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    var temp = JsonConvert.DeserializeObject<User>(jsonString.Result);

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
