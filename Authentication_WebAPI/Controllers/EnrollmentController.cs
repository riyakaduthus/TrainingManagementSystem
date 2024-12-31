using Authentication_WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using TMS_WebAPI.Repo;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TMS_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        EnrollmentRepository _enrollmentRepo;
        public EnrollmentController(EnrollmentRepository enrollmentRepo)
        {
            _enrollmentRepo = enrollmentRepo;
        }

        // GET: api/<EnrollmentController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet("GetManagerName/{id}")]
        public IActionResult GetManagerName(int id)
        {
            return Ok(_enrollmentRepo.GetManagerId(id));
        }

        // GET api/<EnrollmentController>/5
        [HttpGet("{id}")]
        public IActionResult GetEnrollmentById(int id) 
        {
            return Ok(_enrollmentRepo.GetEnrollmentByEnrollmentId(id));
        }

        // POST api/<EnrollmentController>
        [HttpPost]
        public IActionResult Post(Enrollment enrollment)
        {
            _enrollmentRepo.AddEnrollment(enrollment);
            return Created("Enrollment Requested",enrollment);
        }

        // PUT api/<EnrollmentController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Enrollment enrollment)
        {
            _enrollmentRepo.UpdateEnrollmentStatus(id, enrollment);
            return Ok();
        }
       
    }
}
