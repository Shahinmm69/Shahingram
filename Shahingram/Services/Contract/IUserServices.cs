using Entities.Models;

namespace Services.Contract
{
    public interface IUserServices
    {
        Task<User> CraetionConfigAsync(User entity, CancellationToken cancellationToken);
        Task DeletePhotoHandlerAsync(int id, CancellationToken cancellationToken);
        Task<string> GetUserNameByIdAsync(int? id, CancellationToken cancellationToken);
        Task<string> GetCategoryTitleByIdAsync(int id, CancellationToken cancellationToken);
        Task<string> GetCountryTitleByIdAsync(int id, CancellationToken cancellationToken);
        Task<string> GetDescribtionAsync(int id, CancellationToken cancellationToken);
        Task<List<int>?> GetDirectsWithAnotherAsync(int id, int anotherid, CancellationToken cancellationToken);
        Task<List<int>?> GetFollowersAsync(int id, CancellationToken cancellationToken);
        Task<int?> GetFollowersCountAsync(int id, CancellationToken cancellationToken);
        Task<List<int>?> GetFollowingsAsync(int id, CancellationToken cancellationToken);
        Task<int?> GetFollowingsCountAsync(int id, CancellationToken cancellationToken);
        Task<string?> GetPhotoAsync(int id, CancellationToken cancellationToken);
        Task<int> GetPostsCountAsync(int id, CancellationToken cancellationToken);
        Task<List<int>?> GetPostsIdAsync(int id, CancellationToken cancellationToken);
        Task<List<User>?> GetUsersHaveDirectsAsync(int id, CancellationToken cancellationToken);
        Task NewPhotoHandlerAsync(string address, int id, CancellationToken cancellationToken);
    }
}