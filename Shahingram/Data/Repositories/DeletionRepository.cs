using Common;
using Common.Utilities;
using Data.Contract;
using Entities.Common;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class DeletionRepository<TEntity> : Repository<TEntity>, IScopedDependency, IDeletionRepository<TEntity> where TEntity : class, IDeletion, IEntity
    {
        protected readonly ApplicationDbContext DbContext;
        private readonly SignInManager<User> signInManager;
        public DeletionRepository(ApplicationDbContext dbContext, SignInManager<User> signInManager)
            : base(dbContext)
        {
            DbContext = dbContext;
            this.signInManager = signInManager;
        }
        public virtual Task DeletionDateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            entity.DeletionDate = DateTime.Now;
            entity.IsDeleted = true;
            entity.UserDeletionId = Convert.ToInt32(signInManager.Context.Request.HttpContext.User.Identity.GetUserId());
            return DeleteAsync(entity, cancellationToken);
        }
    }
}
