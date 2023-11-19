using Common;
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
    public class CreationRepository<TEntity> : Repository<TEntity>, IScopedDependency, ICreationRepository<TEntity>
        where TEntity : Creation
    {
        protected readonly ApplicationDbContext DbContext;
        private readonly SignInManager<User> signInManager;
        public CreationRepository(ApplicationDbContext dbContext, SignInManager<User> signInManager)
            : base(dbContext)
        {
            DbContext = dbContext;
            this.signInManager = signInManager;
        }
        public virtual Task CraetionDateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            entity.CreationDate = DateTime.Now;
            entity.UserCreationId = Convert.ToInt32(signInManager.Context.Request.HttpContext.User.Identity.GetUserId());
            return AddAsync(entity, cancellationToken);

        }
    }
}
