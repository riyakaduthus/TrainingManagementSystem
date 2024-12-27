using System.Reflection.Metadata.Ecma335;
using Authentication_WebAPI.Context;
using Authentication_WebAPI.Models;
using TMS_WebAPI.IRepo;
using TMS_WebAPI.ViewModel;

namespace TMS_WebAPI.Repo
{
    public class EnrollmentRepository : IEnrollmentRepo
    {
        AppDBContext _dbContext;
        public EnrollmentRepository(AppDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public Enrollment AddEnrollment(Enrollment enrollment)
        {
            enrollment.IsActive = true;

            _dbContext.Enrollments.Add(enrollment);
            _dbContext.SaveChanges();
            return enrollment;
        }

        public Enrollment GetEnrollmentByEnrollmentId(int id)
        {
            var enrollment = _dbContext.Enrollments.FirstOrDefault(x=>x.EnrollmentId == id && x.IsActive == true);
            return enrollment;
        }

        public List<Enrollment> GetEnrollments()
        {
            return _dbContext.Enrollments.Where(x => x.IsActive == true).ToList();
        }

        public bool UpdateEnrollmentStatus(int id, Enrollment enrollment)
        {
            Enrollment enroll = GetEnrollmentByEnrollmentId(id);
            if (enroll != null) 
            {
                enroll.EnrollmentStatus = enrollment.EnrollmentStatus;
                
                _dbContext.SaveChanges();
                return true;
            }
            else return false;
        }
        public List<EnrollmentViewModel> GetEnrollmentViews()
        {
            List<EnrollmentViewModel> enroll = (from x in _dbContext.Enrollments
                                                join y in _dbContext.Users on x.UserId equals y.UserId
                                                join z in _dbContext.Batches on x.BatchId equals z.BatchId
                                                select new EnrollmentViewModel
                                                {
                                                    EnrollmentId = x.EnrollmentId,
                                                    UserId = x.UserId,
                                                    UserName = y.UserName,
                                                    BatchId = z.BatchId,
                                                    BatchName = z.BatchName,
                                                    EnrollmentStatus = x.EnrollmentStatus,
                                                    RequestedDate = x.RequestedDate
                                                }).ToList();
            return enroll;
        }
    }
}
