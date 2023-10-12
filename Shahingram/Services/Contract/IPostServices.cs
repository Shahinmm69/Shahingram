using Entities.Models;

namespace Services.Contract
{
    public interface IPostServices
    {
        Task ForwardAsync(int id, int userreceiverid, CancellationToken cancellationToken);
        Task<string> GetDescribtionAsync(int id, CancellationToken cancellationToken);
        IAsyncEnumerable<Like>? GetLike(int id, CancellationToken cancellationToken);
        Task<Photo> GetPhotoAsync(int id, CancellationToken cancellationToken);
        Task<Video> GetVideoAsync(int id, CancellationToken cancellationToken);
        Task HashtagsHandler(int id, CancellationToken cancellationToken);
        Task<object?> LoadContentAsync(CancellationToken cancellationToken, params int[] ids);
        Task NewPhotoHandlerAsync(string address, int id, int userid, CancellationToken cancellationToken);
        Task NewVideoHandlerAsync(string address, int id, int userid, CancellationToken cancellationToken);
    }
}