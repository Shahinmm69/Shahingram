using Entities.Models;

namespace Services.Contract
{
    public interface IPostServices
    {
        Task ForwardAsync(int id, int userreceiverid, CancellationToken cancellationToken);
        Task<List<Comment>?> GetCommentsAsync(int id, int pageNumer, int pageSize, CancellationToken cancellationToken);
        Task<string> GetDescribtionAsync(int id, CancellationToken cancellationToken);
        Task<List<Like>?> GetLikesAsync(int id, int pageNumer, int pageSize, CancellationToken cancellationToken);
        Task<Post>? GetPhotoAsync(int id, CancellationToken cancellationToken);
        Task<Post>? GetVideoAsync(int id, CancellationToken cancellationToken);
        Task HashtagsHandlerAsync(int id, CancellationToken cancellationToken);
        Task<List<Post>?> LoadContentAsync(CancellationToken cancellationToken, params int[] ids);
        Task NewPhotoHandlerAsync(string address, int id, CancellationToken cancellationToken);
        Task NewVideoHandlerAsync(string address, int id, CancellationToken cancellationToken);
    }
}