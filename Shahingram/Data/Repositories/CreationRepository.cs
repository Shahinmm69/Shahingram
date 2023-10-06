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
    public class CreationRepository<TEntity> : Repository<TEntity>, ICreationRepository<TEntity> where TEntity : Craetion
    {
        protected readonly ApplicationDbContext DbContext;
        public CreationRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            DbContext = dbContext;
        }
        public Task CraetionDateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            entity.CrationDate = DateTime.Now;
            return AddAsync(entity, cancellationToken);

        }
    }
}
