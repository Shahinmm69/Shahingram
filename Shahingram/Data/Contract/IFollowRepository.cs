using Entities.Models;

namespace Data.Contract
{
    public interface IFollowRepository
    {
        Task CraetionDateAsync(Follow entity, CancellationToken cancellationToken);
        Task DeletionDateAsync(Follow entity, CancellationToken cancellationToken);
        Task FollowBack(Follow entity, CancellationToken cancellationToken);
    }
}