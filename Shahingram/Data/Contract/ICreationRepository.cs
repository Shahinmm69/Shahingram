using Entities.Common;

namespace Data.Contract
{
    public interface ICreationRepository<TEntity> where TEntity : Creation
    {
        Task CraetionDateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}