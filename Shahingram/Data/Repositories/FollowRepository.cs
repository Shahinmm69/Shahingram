using Data.Common;
using Data.Contract;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class FollowRepository : Repository<Follow>, IFollowRepository
    {
        protected readonly ApplicationDbContext DbContext;
        public FollowRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            DbContext = dbContext;
        }

        public Task CraetionDateAsync(Follow entity, CancellationToken cancellationToken)
        {
            entity.CrationDate = DateTime.Now;
            //Wait for accept user
            entity.IsDeleted = true;
            return AddAsync(entity, cancellationToken);
        }

        public Task DeletionDateAsync(Follow entity, CancellationToken cancellationToken)
        {
            entity.DeletionDate = DateTime.Now;
            entity.IsDeleted = true;
            return DeleteAsync(entity, cancellationToken);
        }

        public Task FollowBack(Follow entity, CancellationToken cancellationToken)
        {
            _ = base.AddAsync(new Follow() { FollowId = entity.UserId, UserId = entity.FollowId, CrationDate = DateTime.Now }, cancellationToken);
            return Task.CompletedTask;
        }
    }
}
