using Entities.Models;

namespace Services.Contract
{
    public interface IPostServices
    {
        Task ForwardAsync(int id, int userreceiverid, CancellationToken cancellationToken);
        Task<List<Comment>?> GetCommentsAsync(int id, int pageNumer, int pageSize, CancellationToken cancellationToken);
        Task<string> GetDescribtionAsync(int id, CancellationToken cancellationToken);
        Task<List<Like>?> GetLikesAsync(int id, int pageNumer, int pageSize, CancellationToken cancellationToken);
        Task<List<string>?> GetPhotoAsync(int id, CancellationToken cancellationToken);
        Task<List<string>?> GetVideoAsync(int id, CancellationToken cancellationToken);
        Task HashtagsHandlerAsync(int id, CancellationToken cancellationToken);
        Task NewPhotoHandlerAsync(string address, int id, CancellationToken cancellationToken);
        Task NewVideoHandlerAsync(string address, int id, CancellationToken cancellationToken);
    }
}