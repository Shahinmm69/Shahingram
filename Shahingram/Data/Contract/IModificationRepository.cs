using Entities.Common;

namespace Data.Contract
{
    public interface IModificationRepository<TEntity> where TEntity : Modification
    {
        Task CraetionDateAsync(TEntity entity, CancellationToken cancellationToken);
        Task UpdatModificationDateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}