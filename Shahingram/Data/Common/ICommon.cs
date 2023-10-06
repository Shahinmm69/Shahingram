using Entities.Common;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Common
{
    public interface ICommon<TEntity> where TEntity : class, IEntity
    {
        Task UpdatModificationDateAsync(TEntity entity, CancellationToken cancellationToken);
        Task CraetionDateAsync(TEntity entity, CancellationToken cancellationToken);
        Task DeletionDateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
