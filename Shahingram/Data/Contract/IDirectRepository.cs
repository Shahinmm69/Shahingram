using Entities.Models;

namespace Data.Contract
{
    public interface IDirectRepository
    {
        Task CraetionDateAsync(Direct entity, CancellationToken cancellationToken);
        Task DeletionDateAsync(Direct entity, CancellationToken cancellationToken);
        Task Forward(int id, int userreceiverid, CancellationToken cancellationToken);
        string GetDescribtion(int id, CancellationToken cancellationToken);
        Task<Photo> GetPhoto(int id, CancellationToken cancellationToken);
        Task<Post> GetPost(int id, CancellationToken cancellationToken);
        Task<Video> GetVideo(int id, CancellationToken cancellationToken);
        Task<object?> LoadContentAsync(CancellationToken cancellationToken, params int[] ids);
        Task NewPhotoHandler(string address, int id, int userid, CancellationToken cancellationToken);
        Task NewVideoHandler(string address, int id, int userid, CancellationToken cancellationToken);
        Task UpdatModificationDateAsync(Direct entity, CancellationToken cancellationToken);
    }
}