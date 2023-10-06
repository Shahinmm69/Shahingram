using Data.Contract;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        protected readonly ApplicationDbContext DbContext;
        public LikeRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            DbContext = dbContext;
        }

        public Task CraetionDateAsync(Like entity, CancellationToken cancellationToken)
        {
            entity.CrationDate = DateTime.Now;
            return AddAsync(entity, cancellationToken);
        }

        public Task DeletionDateAsync(Like entity, CancellationToken cancellationToken)
        {
            entity.DeletionDate = DateTime.Now;
            entity.IsDeleted = true;
            return DeleteAsync(entity, cancellationToken);
        }
    }
}
