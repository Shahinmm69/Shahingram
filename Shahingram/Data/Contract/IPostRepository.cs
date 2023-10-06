using Entities.Models;

namespace Data.Contract
{
    public interface IPostRepository
    {
        Task CraetionDateAsync(Post entity, CancellationToken cancellationToken);
        Task DeletionDateAsync(Post entity, CancellationToken cancellationToken);
        Task Forward(Post post, int userreceiverid, CancellationToken cancellationToken);
        string GetDescribtion(int id, CancellationToken cancellationToken);
        IEnumerable<Like>? GetLike(int id, CancellationToken cancellationToken);
        Task<Photo> GetPhoto(int id, CancellationToken cancellationToken);
        Task<Video> GetVideo(int id, CancellationToken cancellationToken);
        Task HashtagsHandler(int id, CancellationToken cancellationToken);
        Task<object?> LoadContentAsync(CancellationToken cancellationToken, params int[] ids);
        Task NewPhotoHandler(string address, int id, int userid, CancellationToken cancellationToken);
        Task NewVideoHandler(string address, int id, int userid, CancellationToken cancellationToken);
        Task UpdatModificationDateAsync(Post entity, CancellationToken cancellationToken);
    }
}