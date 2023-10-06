using Data.Contract;
using Entities.Common;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class DeletionRepository<TEntity> : Repository<TEntity>, IDeletionRepository<TEntity> where TEntity : class, IDeletion, IEntity
    {
        protected readonly ApplicationDbContext DbContext;
        public DeletionRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            DbContext = dbContext;
        }
        public Task DeletionDateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            entity.DeletionDate = DateTime.Now;
            entity.IsDeleted = true;
            return DeleteAsync(entity, cancellationToken);
        }
    }
}
