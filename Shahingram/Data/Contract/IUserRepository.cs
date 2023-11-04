using Entities.Models;

namespace Data.Contract
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUserAndPass(string username, string password, CancellationToken cancellationToken);
        Task UpdateSecuirtyStampAsync(User user, CancellationToken cancellationToken);
        Task UpdatModificationDateAsync(User entity, CancellationToken cancellationToken);
    }
}