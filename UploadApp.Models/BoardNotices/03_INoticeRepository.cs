using System;
using System.Threading.Tasks;

namespace UploadApp.Models.BNotices
{
    /// <summary>
    /// [3] Repository Interface 
    /// </summary>
    public interface INoticeRepository : ICrudRepository<BoardNotices>
    {
        Task<Tuple<int, int>> GetStatus(int parentId);
        Task<bool> DeleteAllByParentId(int parentId); 
    }
}
