using Authentication_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS_WebAPI.IRepo;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TMS_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User1Controller : ControllerBase
    {
        IUserRepo _userRepo;
        public User1Controller(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }


        // GET: api/<UserController>
        [HttpGet]
        public IActionResult GetUser()
        {
            return Ok(_userRepo.GetUsers());
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public IActionResult GetUsersById(int id)
        {
            return Ok(_userRepo.GetUserDetailsById(id));
        }
               
    // POST api/<UserController>
        [HttpPost]
        //[Authorize]
        public IActionResult Post(User usr)
        {
            _userRepo.AddUser(usr);
            return Created("User created", usr);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        //[Authorize]
        public IActionResult Put(int id, User usr)
        {
            _userRepo.UpdateUser(id, usr);
            return Ok();
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        //[Authorize]
        public IActionResult Delete(int id)
        {
            _userRepo.DeleteUser(id);
            return Ok();
        }
        [HttpGet]
        [Route("GetRoles")]
        public IActionResult GetUserRoles()
        {
            return Ok(_userRepo.GetRoles());
        }

        [HttpGet]
        [Route("GetManagerName")]
        public IActionResult GetManagerName()
        {
            return Ok(_userRepo.GetManagerNames());
        }


        [HttpGet("managers/{id}")]
        public IActionResult GetManagersNameById(int id)
        {
            return Ok(_userRepo.GetManagers(id));
        }

    }
}
