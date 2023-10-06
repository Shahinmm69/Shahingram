using Common;
using Common.Exceptions;
using Common.Utilities;
using Data.Contract;
using Entities.Common;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRepository : Repository<User>
    {
        protected readonly ApplicationDbContext DbContext;
        public UserRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            DbContext = dbContext;
        }

        public Task<Photo> GetPhoto(int id, CancellationToken cancellationToken)
        {
            var u = DbContext.Set<UserPhoto>()
                .AsNoTracking()
                .Where(u => u.UserId == id)
                .Include(u => u.Photo.IsDeleted != false).Single();
            var p = DbContext.Set<Photo>()
                .AsNoTracking()
                .Where(p => p.Id == u.PhotoId)
                .SingleAsync();
            return p;
        }

        public Task<User> GetByUserAndPass(string username, string password, CancellationToken cancellationToken)
        {
            var passwordHash = SecurityHelper.GetSha256Hash(password);
            return Table.Where(p => p.UserName == username && p.PasswordHash == passwordHash).SingleOrDefaultAsync(cancellationToken);
        }

        public Task UpdateSecuirtyStampAsync(User user, CancellationToken cancellationToken)
        {
            //user.SecurityStamp = Guid.NewGuid();
            return UpdateAsync(user, cancellationToken);
        }

        //public override void Update(User entity, bool saveNow = true)
        //{
        //    entity.SecurityStamp = Guid.NewGuid();
        //    base.Update(entity, saveNow);
        //}


        public async Task AddAsync(User user, string password, CancellationToken cancellationToken)
        {
            var exists = await TableNoTracking.AnyAsync(p => p.UserName == user.UserName);
            if (exists)
                throw new BadRequestException("نام کاربری تکراری است");

            var passwordHash = SecurityHelper.GetSha256Hash(password);
            user.PasswordHash = passwordHash;
            await base.AddAsync(user, cancellationToken);
        }
    }
}
