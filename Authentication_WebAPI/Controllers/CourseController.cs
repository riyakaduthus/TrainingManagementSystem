using Authentication_WebAPI.IRepo;
using Authentication_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TMS_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        ICourseRepo _courseRepo;
        public CourseController(ICourseRepo courseRepo)
        {
            _courseRepo= courseRepo;
        }

        // GET: api/<CourseController>
        [HttpGet]
        [Authorize]
        public IActionResult GetCourses()
        {
            return Ok(_courseRepo.GetCourses());
        }

        // GET api/<CourseController>/5
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Get(int id)
        {
            return Ok(_courseRepo.GetCourseById(id));
        }

        // POST api/<CourseController>
        [HttpPost]
        [Authorize(Roles="Admin")]
        public IActionResult Post(Course course)
        {
            string courseName = _courseRepo.GetCourseName(course.CourseName);
            if (courseName != null)
            {
                return BadRequest("This Course already exist");
            }
            else
            {
                _courseRepo.AddCourse(course);
                return Created("course added", course);
            }

        }

        // PUT api/<CourseController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Put(int id, Course course)
        {
            _courseRepo.UpdateCourse(id, course);
            return Ok();
        }

        // DELETE api/<CourseController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _courseRepo.DeleteCourse(id);
            return Ok();
        }
    }
}
