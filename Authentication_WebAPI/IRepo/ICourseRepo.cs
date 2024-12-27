using Authentication_WebAPI.Models;
using TMS_WebAPI.ViewModel;

namespace Authentication_WebAPI.IRepo
{
    public interface ICourseRepo
    {
        List<Course> GetCourses();
        Course GetCourseById(int id);
        Course AddCourse(Course course);
        bool UpdateCourse(int courseId, Course course);
        bool DeleteCourse(int courseId);

        string GetCourseName(string courseName);        
    }

}
