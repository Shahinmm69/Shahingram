using Common;
using Common.Exceptions;
using Data.Contract;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Contract;

namespace Services.Services
{
    public class FollowServices : IFollowServices, IScopedDependency
    {
        protected readonly ICreationRepository<Follow> creationfollowrepository;
        protected readonly IRepository<Follow> followrepository;
        private readonly SignInManager<User> signInManager;
        public FollowServices(ICreationRepository<Follow> creationfollowrepository, IRepository<Follow> followrepository, SignInManager<User> signInManager)
        {
            this.creationfollowrepository = creationfollowrepository;
            this.followrepository = followrepository;
            this.signInManager = signInManager;
        }

        public async Task CraetionConfigAsync(Follow entity, CancellationToken cancellationToken)
        {
            var id = Convert.ToInt32(signInManager.Context.Request.HttpContext.User.Identity.GetUserId());
            var follow = await followrepository.TableNoTracking.Where(x => x.UserCreationId == id && x.UserFollowId == entity.UserFollowId).ToListAsync();

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
