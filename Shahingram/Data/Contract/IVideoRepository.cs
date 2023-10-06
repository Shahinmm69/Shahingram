using Entities.Models;

namespace Data.Contract
{
    public interface IVideoRepository
    {
        Task CraetionDateAsync(Video entity, CancellationToken cancellationToken);
        Task DeletionDateAsync(Video entity, CancellationToken cancellationToken);
    }
}