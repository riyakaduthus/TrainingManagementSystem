using System.Reflection.Metadata.Ecma335;
using Authentication_WebAPI.Context;
using Authentication_WebAPI.Models;
using Microsoft.EntityFrameworkCore;
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

        #region Get Enrollment Details
        /// <summary>
        /// Get enrollment details by enrollment id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EnrollmentViewModel GetEnrollmentByEnrollmentId(int id)
        {
            var enrollment = (from x in _dbContext.Enrollments
                              join y in _dbContext.Users on x.UserId equals y.UserId
                              join z in _dbContext.Batches on x.BatchId equals z.BatchId
                              join a in _dbContext.Users on x.ManagerId equals a.UserId
                              where x.EnrollmentId == id && x.IsActive == true
                              select new EnrollmentViewModel
                              {
                                  EnrollmentId = x.EnrollmentId,
                                  EnrollmentStatus = x.EnrollmentStatus,
                                  RequestedDate = x.RequestedDate,
                                  UserId = x.UserId,
                                  UserName = y.UserName,
                                  BatchId = x.BatchId,
                                  BatchName = z.BatchName,
                                  ManagerId = x.ManagerId,
                                  ManagerName = a.UserName,
                                  IsActive = x.IsActive
                              }).FirstOrDefault();

            return enrollment;
        }
        #endregion

        #region Get all enrollment details (index view of enrollment)
        /// <summary>
        /// Get all enrollment details (index view of enrollment)
        /// </summary>
        /// <returns></returns>
        public List<EnrollmentViewModel> GetEnrollments()
        {
            List<EnrollmentViewModel> enrollments = (from x in _dbContext.Enrollments
                                                     join y in _dbContext.Users on x.UserId equals y.UserId
                                                     join z in _dbContext.Batches on x.BatchId equals z.BatchId
                                                     join a in _dbContext.Users on x.ManagerId equals a.UserId
                                                     where x.IsActive == true
                                                     select new EnrollmentViewModel
                                                     {
                                                         EnrollmentId = x.EnrollmentId,
                                                         EnrollmentStatus = x.EnrollmentStatus,
                                                         RequestedDate = x.RequestedDate,
                                                         UserId = x.UserId,
                                                         UserName = y.UserName,
                                                         BatchId = x.BatchId,
                                                         BatchName = z.BatchName,
                                                         ManagerId = x.ManagerId,
                                                         ManagerName = a.UserName,
                                                         IsActive = x.IsActive,
                                                         CreatedBy = x.CreatedBy,
                                                         CreatedOn = x.CreatedOn,
                                                         UpdatedBy = x.UpdatedBy
                                                     }).ToList();
            return enrollments;

        }
        #endregion



        public bool UpdateEnrollmentStatus(int id, Enrollment enrollment)
        {
            Enrollment enroll = _dbContext.Enrollments.FirstOrDefault(x => x.EnrollmentId == id && x.IsActive == true);
            if (enroll != null)
            {
                enroll.EnrollmentStatus = enrollment.EnrollmentStatus;
                enroll.UpdatedBy = enrollment.UpdatedBy;
                enroll.Updated = enrollment.Updated;
                enroll.ManagerId = enrollment.ManagerId;
                enroll.IsActive = enrollment.IsActive;

                _dbContext.Entry(enroll).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return true;
            }
            else return false;
        }

    }
}
