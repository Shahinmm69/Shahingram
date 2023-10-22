using Common;
using Common.Exceptions;
using Data.Contract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class LikeServices : ILikeServices, IScopedDependency
    {
        protected readonly ICreationRepository<Like> creationlikerepository;
        protected readonly IRepository<Like> likerepository;
        public LikeServices(ICreationRepository<Like> creationlikerepository, IRepository<Like> likerepository)
        {
            this.creationlikerepository = creationlikerepository;
            this.likerepository = likerepository;
        }

        public async Task CraetionConfigAsync(Like entity, CancellationToken cancellationToken)
        {
            var like = await likerepository.TableNoTracking.Where(x => x.UserId == entity.UserId && x.PostId == entity.PostId).LastAsync();

            if (like == null || like.IsDeleted == true)
            {
                await creationlikerepository.CraetionDateAsync(entity, cancellationToken);
            }
            else
            {
                throw new BadRequestException("شما قبلا این پست را لایک کردید");
            }
        }
    }
}
