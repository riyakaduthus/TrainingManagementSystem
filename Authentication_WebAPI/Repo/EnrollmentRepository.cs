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
            _dbContext.Enrollments.Add(enrollment);
            _dbContext.SaveChanges();
            return enrollment;
        }
        public int GetManagerId(int id)
        {
            var managerId = _dbContext.Users.FirstOrDefault(x => x.UserId == id).ManagerId;
            return managerId.Value;
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
       
    }
}
