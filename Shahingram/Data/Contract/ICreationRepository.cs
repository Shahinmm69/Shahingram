using Entities.Common;

namespace Data.Contract
{
    public interface ICreationRepository<TEntity> where TEntity : Craetion
    {
        Task CraetionDateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}