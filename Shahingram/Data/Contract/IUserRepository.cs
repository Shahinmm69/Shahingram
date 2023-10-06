using Entities.Models;

namespace Data.Contract
{
    public interface IUserRepository
    {
        Task AddAsync(User user, string password, CancellationToken cancellationToken);
        Task<User> GetByUserAndPass(string username, string password, CancellationToken cancellationToken);
        Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
        Task UpdateSecuirtyStampAsync(User user, CancellationToken cancellationToken);
    }
}