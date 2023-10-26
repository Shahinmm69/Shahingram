using Entities.Models;

namespace Services.Contract
{
    public interface IUserServices
    {
        Task CraetionConfigAsync(User entity, CancellationToken cancellationToken);
        Task DeletePhotoHandlerAsync(int id, CancellationToken cancellationToken);
        Task<int> GetCategoryIdAsync(string title, CancellationToken cancellationToken);
        Task<int> GetCountryIdAsync(string title, CancellationToken cancellationToken);
        Task<string> GetDescribtionAsync(int id, CancellationToken cancellationToken);
        Task<List<Follow>?> GetFollowersAsync(int userid, CancellationToken cancellationToken);
        Task<List<Follow>?> GetFollowingsAsync(int userid, CancellationToken cancellationToken);
        Task<List<User>?> GetPhotoAsync(int id, CancellationToken cancellationToken);
        Task<List<User>?> GetPostsAsync(int id, CancellationToken cancellationToken);
        Task<List<Direct>?> GetRecieveDirectsAsync(int userid, CancellationToken cancellationToken);
        Task<List<Direct>?> GetSendDirectsAsync(int id, CancellationToken cancellationToken);
        Task<List<User>?> LoadContentAsync(int id, CancellationToken cancellationToken);
        Task NewPhotoHandlerAsync(string address, int id, CancellationToken cancellationToken);
        Task NewPostHandlerAsync(object? file, string? text, int id, int userid, CancellationToken cancellationToken);
    }
}