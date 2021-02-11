using System;
using System.Threading.Tasks;

namespace UploadApp.Models.BUploads
{
    /// <summary>
    /// [3] Repository Interface 
    /// </summary>
    public interface IUploadRepository : ICrudRepository<BoardUploads>
    {
        Task<Tuple<int, int>> GetStatus(int parentId);
        Task<bool> DeleteAllByParentId(int parentId); 
    }
}
