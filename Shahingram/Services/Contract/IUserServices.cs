using Entities.Models;

namespace Services.Contract
{
    public interface IUserServices
    {
        Task CraetionConfigAsync(User entity, CancellationToken cancellationToken);
        Task<int> GetCategoryIdAsync(string title, CancellationToken cancellationToken);
        Task<int> GetCountryIdAsync(string title, CancellationToken cancellationToken);
        Task<string> GetDescribtionAsync(int id, CancellationToken cancellationToken);
        IAsyncEnumerable<Follow> GetFollowersAsync(int userid, CancellationToken cancellationToken);
        IAsyncEnumerable<Follow> GetFollowingsAsync(int userid, CancellationToken cancellationToken);
        Task<Photo> GetPhotoAsync(int id, CancellationToken cancellationToken);
        IAsyncEnumerable<Post> GetPostsAsync(int userid, CancellationToken cancellationToken);
        IAsyncEnumerable<Direct> GetRecieveDirectsAsync(int userid, CancellationToken cancellationToken);
        IAsyncEnumerable<Direct> GetSendDirectsAsync(int userid, CancellationToken cancellationToken);
        Task NewPhotoHandlerAsync(string address, int id, CancellationToken cancellationToken);
        Task NewPostHandlerAsync(object? file, string? text, int id, int userid, CancellationToken cancellationToken);
    }
}