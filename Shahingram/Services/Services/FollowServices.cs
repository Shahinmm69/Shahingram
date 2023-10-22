using Common;
using Common.Exceptions;
using Data.Contract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Contract;

namespace Services.Services
{
    public class FollowServices : IFollowServices, IScopedDependency
    {
        protected readonly ICreationRepository<Follow> creationfollowrepository;
        protected readonly IRepository<Follow> followrepository;
        public FollowServices(ICreationRepository<Follow> creationfollowrepository, IRepository<Follow> followrepository)
        {
            this.creationfollowrepository = creationfollowrepository;
            this.followrepository = followrepository;
        }

        public async Task CraetionConfigAsync(Follow entity, CancellationToken cancellationToken)
        {
            var follow = await followrepository.TableNoTracking.Where(x => x.UserId == entity.UserId && x.FollowId == entity.FollowId).ToListAsync();

            if (follow == null)
            {
                //Wait for accept user
                entity.IsDeleted = true;
                await creationfollowrepository.CraetionDateAsync(entity, cancellationToken);
            }
            if (follow[follow.Count - 1].IsDeleted != true)
            {
                throw new BadRequestException("شما در حال حاضر با فرد مورد نظر دوست هستید");
            }
            else
            {
                if (follow[follow.Count - 2].IsDeleted == true)
                {
                    throw new BadRequestException("درخواست دوستی به فرد مورد نظر قبلا ارسال شده است");
                }
                else
                {
                    //Wait for accept user
                    entity.IsDeleted = true;
                    await creationfollowrepository.CraetionDateAsync(entity, cancellationToken);
                }
            }
        }
    }
}
