using Entities.Models;

namespace Data.Contract
{
    public interface ILikeRepository
    {
        Task CraetionDateAsync(Like entity, CancellationToken cancellationToken);
        Task DeletionDateAsync(Like entity, CancellationToken cancellationToken);
    }
}