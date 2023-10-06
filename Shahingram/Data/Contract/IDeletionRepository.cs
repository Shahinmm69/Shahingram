using Entities.Common;

namespace Data.Contract
{
    public interface IDeletionRepository<TEntity> where TEntity : class, IDeletion, IEntity
    {
        Task DeletionDateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}