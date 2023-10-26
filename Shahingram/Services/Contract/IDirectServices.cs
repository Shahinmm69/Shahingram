using Entities.Models;

namespace Services.Contract
{
    public interface IDirectServices
    {
        Task ForwardAsync(int id, int userreceiverid, CancellationToken cancellationToken);
        Task<string> GetDescribtionAsync(int id, CancellationToken cancellationToken);
        Task<Direct> GetPhotoAsync(int id, CancellationToken cancellationToken);
        Task<Direct> GetPostAsync(int id, CancellationToken cancellationToken);
        Task<Direct> GetVideoAsync(int id, CancellationToken cancellationToken);
        Task<List<Direct>?> LoadContentAsync(CancellationToken cancellationToken, int pageNumer, int pageSize, params int[] ids);
        Task NewPhotoHandlerAsync(string address, int id, int userid, CancellationToken cancellationToken);
        Task NewVideoHandlerAsync(string address, int id, int userid, CancellationToken cancellationToken);
    }
}