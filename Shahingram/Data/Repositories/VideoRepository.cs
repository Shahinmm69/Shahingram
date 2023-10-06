using Data.Contract;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class VideoRepository : Repository<Video>, IVideoRepository
    {
        protected readonly ApplicationDbContext DbContext;
        public VideoRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            DbContext = dbContext;
        }

        public Task CraetionDateAsync(Video entity, CancellationToken cancellationToken)
        {
            entity.CrationDate = DateTime.Now;
            //Wait for accept user
            entity.IsDeleted = true;
            return AddAsync(entity, cancellationToken);
        }

        public Task DeletionDateAsync(Video entity, CancellationToken cancellationToken)
        {
            entity.DeletionDate = DateTime.Now;
            entity.IsDeleted = true;
            return DeleteAsync(entity, cancellationToken);
        }
    }
}
