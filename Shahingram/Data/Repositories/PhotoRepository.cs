using Data.Contract;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        protected readonly ApplicationDbContext DbContext;
        public PhotoRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            DbContext = dbContext;
        }

        public Task CraetionDateAsync(Photo entity, CancellationToken cancellationToken)
        {
            entity.CrationDate = DateTime.Now;
            //Wait for accept user
            entity.IsDeleted = true;
            return AddAsync(entity, cancellationToken);
        }

        public Task DeletionDateAsync(Photo entity, CancellationToken cancellationToken)
        {
            entity.DeletionDate = DateTime.Now;
            entity.IsDeleted = true;
            return DeleteAsync(entity, cancellationToken);
        }
    }
}
