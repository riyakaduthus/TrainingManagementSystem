using Authentication_WebAPI.IRepo;
using Authentication_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update;
using TMS_WebAPI.Repo;

namespace TMS_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchController : ControllerBase
    {
        IBatchRepo _batchRepo;
        public BatchController(IBatchRepo batchRepo)
        {
            _batchRepo = batchRepo;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetBatch()
        {
            return Ok(_batchRepo.GetBatchDetails());
        }
        
        [HttpGet]
        [Route("GetAvailableBatches")]
        public IActionResult GetAvailableBatch()
        {
            return Ok(_batchRepo.GetAvailableBatchDetails());
        }

        // GET api/<BatchController>/5
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Get(int id)
        {
            return Ok(_batchRepo.GetBatchById(id));
        }


        // POST api/<BatchController>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Post(Batch batch)
        {
            _batchRepo.AddBatch(batch);
            return Created("batch added", batch);

        }

        // PUT api/<BatchController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Put(int id, Batch batch)
        {
            _batchRepo.UpdateBatch(id, batch);
            return Ok();
        }

        // DELETE api/<BatchController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _batchRepo.DeleteBatch(id);
            return Ok();
        }

        [HttpGet]
        [Route("GetCourseNames")]
        public IActionResult GetCourses()
        {
            return Ok(_batchRepo.GetCourseList());
        }
    }
}
