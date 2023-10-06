using Entities.Models;

namespace Data.Contract
{
    public interface IPhotoRepository
    {
        Task CraetionDateAsync(Photo entity, CancellationToken cancellationToken);
        Task DeletionDateAsync(Photo entity, CancellationToken cancellationToken);
    }
}