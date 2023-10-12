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
    public class ModificationRepository<TEntity> : Repository<TEntity>, IModificationRepository<TEntity> where TEntity : Modification
    {
        protected readonly ApplicationDbContext DbContext;
        public ModificationRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            DbContext = dbContext;
        }
        public virtual Task CraetionDateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            entity.CrationDate = DateTime.Now;
            return AddAsync(entity, cancellationToken);

        }
        public virtual Task UpdatModificationDateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            entity.ModificationDate = DateTime.Now;
            return UpdateAsync(entity, cancellationToken);
        }
    }
}
