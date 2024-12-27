using Authentication_WebAPI.Context;
using Authentication_WebAPI.IRepo;
using Authentication_WebAPI.Models;
using TMS_WebAPI.ViewModel;

namespace TMS_WebAPI.Repo
{
    public class BatchRepository : IBatchRepo
    {
        AppDBContext _dbContext;
        public BatchRepository(AppDBContext dbContext)
        {
            _dbContext  = dbContext;
        }
        public Batch AddBatch(Batch batch)
        {
            
            batch.IsActive = true;
            batch.CreatedOn = DateTime.Now;
            batch.CreatedBy = 1;
            
            
            _dbContext.Batches.Add(batch);
            _dbContext.SaveChanges();

            return batch;
        }
        public bool DeleteBatch(int batchId)
        {
            Batch batch = GetBatchDetailsById(batchId);
            if (batch != null)
            {
                //_dbContext.Remove(batch);

                batch.IsActive = false;

                _dbContext.SaveChanges();
                return true;
            }
            else
                return false;
        }
        public bool UpdateBatch(int batchId, Batch batch)
        {
            Batch obj = GetBatchDetailsById(batchId);
            if (obj != null)
            {
                obj.Updated = DateTime.Now;
                obj.UpdatedBy = 1;

                obj.BatchName = batch.BatchName;
                obj.StartDate = batch.StartDate;
                obj.EndDate = batch.EndDate;
                obj.BatchCount = batch.BatchCount;
                obj.CourseId = batch.CourseId;

                _dbContext.SaveChanges();
                return true;
            }
            else
                return false;
        }
        public BatchViewModel GetBatchById(int id)
        {
            var batch = (from x in _dbContext.Batches
                         join y in _dbContext.Courses
                         on x.CourseId equals y.CourseId
                         where x.BatchId == id && x.IsActive == true
                         select new BatchViewModel
                         {
                             BatchName = x.BatchName,
                             BatchCount = x.BatchCount,
                             StartDate = x.StartDate,
                             EndDate = x.EndDate,
                             CourseName = y.CourseName,
                             BatchId=x.BatchId,
                             CourseId=y.CourseId
                         }).FirstOrDefault();
            return batch;
        }
        public Batch GetBatchDetailsById(int batchId)
        {
            return _dbContext.Batches.FirstOrDefault(x => x.BatchId == batchId);
        }
        public List<BatchViewModel> GetBatchDetails() {

            List<BatchViewModel> batchView = (from x in _dbContext.Batches
                                              join y in _dbContext.Courses
                                              on x.CourseId equals y.CourseId
                                              where x.IsActive == true
                                              select new BatchViewModel
                                              {
                                                  BatchName = x.BatchName,
                                                  CourseName = y.CourseName,
                                                  StartDate = x.StartDate,
                                                  EndDate = x.EndDate,
                                                  BatchCount = x.BatchCount,
                                                  BatchId = x.BatchId
                                              }).ToList();         

            return batchView;
        }

        /// <summary>
        /// showing course names in the dropdown list
        /// </summary>
        /// <returns></returns>
        public List<CourseViewModel> GetCourseList()
        {
            List<CourseViewModel> courseModel = (from x in _dbContext.Courses
                                                 where x.IsActive == true
                                                 select new CourseViewModel
                                                 {
                                                     CourseId = x.CourseId,
                                                     CourseName = x.CourseName
                                                 }).ToList();
            return courseModel;
        }
    }
}
