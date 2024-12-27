using Authentication_WebAPI.Models;
using TMS_WebAPI.ViewModel;

namespace Authentication_WebAPI.IRepo
{
    public interface IBatchRepo
    {
        BatchViewModel GetBatchById(int id);
        Batch AddBatch(Batch batch);
        bool UpdateBatch(int batchId, Batch batch);
        bool DeleteBatch(int batchId);

        List<BatchViewModel> GetBatchDetails();
        List<CourseViewModel> GetCourseList();

    }
}
