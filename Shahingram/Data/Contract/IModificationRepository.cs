using Entities.Common;

namespace Data.Contract
{
    public interface IModificationRepository<TEntity> where TEntity : Modification
    {
        Task UpdatModificationDateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}