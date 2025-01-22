using Authentication_WebAPI.Context;
using Authentication_WebAPI.IRepo;
using Authentication_WebAPI.Models;
using TMS_WebAPI.ViewModel;

namespace TMS_WebAPI.Repo
{
    public class CourseRepository : ICourseRepo
    {
        AppDBContext _dbContext;
        public CourseRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Course AddCourse(Course course)
        {
            course.IsActive = true;
            _dbContext.Courses.Add(course);
            _dbContext.SaveChanges();

            return course;
        }

        public bool DeleteCourse(int courseId)
        {

            Course course = GetCourseById(courseId);
            if (course != null)
            {
               // _dbContext.Remove(course);
                course.IsActive = false;
                _dbContext.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public Course GetCourseById(int id)
        {
            var course = _dbContext.Courses.FirstOrDefault(x => x.CourseId == id && x.IsActive == true);
            return course;
        }

        public string GetCourseName(string courseName)
        {
            var course = _dbContext.Courses.FirstOrDefault(x => x.CourseName == courseName && x.IsActive == true);
            return course?.CourseName ?? null;
        }

        /// <summary>
        /// To View all courses
        /// </summary>
        /// <returns></returns>
        public List<Course> GetCourses()
        {
            return _dbContext.Courses.Where(x => x.IsActive == true).ToList();
        }

        /// <summary>
        /// To View all available courses
        /// </summary>
        /// <returns></returns>
        public List<Course> GetAvailableCourses()
        {
            return _dbContext.Courses.Where(x => x.IsActive == true && x.Availablity == true).ToList();
        }

        public bool UpdateCourse(int courseId, Course course)
        {
            Course obj = GetCourseById(courseId);
            if (obj != null)
            {
                obj.CourseName = course.CourseName;
                obj.CourseDescription = course.CourseDescription;
                obj.DurationInDays = course.DurationInDays;
                obj.Availablity = course.Availablity;

                _dbContext.SaveChanges();
                return true;
            }
            else
                return false;
        }

        
    }
}
