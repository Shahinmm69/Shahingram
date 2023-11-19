using Common.Exceptions;
using Data.Contract;
using Data.Repositories;
using Entities.Common;
using Entities.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Contract;
using Shahingram.Models;
using WebFramework.Api;

namespace Shahingram.Controllers
{
    public class PostController : CrudControllerType2<PostDto, PostSelectDto, Post>
    {
        private readonly IRepository<Post> postRepository;
        private readonly IPostServices postServices;
        private readonly IUserServices userServices;
        private readonly IDeletionRepository<Post> deletionPostRepository;
        private readonly IModificationRepository<Post> modificationPostRepository;
        private readonly SignInManager<User> signInManager;

        public PostController(IRepository<Post> postRepository, IPostServices postServices, ICreationRepository<Post> creationPostRepository
            , IDeletionRepository<Post> deletionPostRepository, IModificationRepository<Post> modificationPostRepository, IUserServices userServices
            , SignInManager<User> signInManager)
                : base(postRepository, deletionPostRepository, modificationPostRepository, signInManager)
        {
            this.postRepository = postRepository;
            this.postServices = postServices;
            this.deletionPostRepository = deletionPostRepository;
            this.modificationPostRepository = modificationPostRepository;
            this.userServices = userServices;
            this.signInManager = signInManager;
        }

        //public override async Task<ApiResult<PostSelectDto>> Get(int id, CancellationToken cancellationToken)
        //{
        //    var post = await postRepository.GetByIdAsync(cancellationToken, id);
        //    if (post == null)
        //        return NotFound();
        //    var postSelectDto = new PostSelectDto
        //    {
        //        Text = post.Text,
        //        PhotoAddress = await postServices.GetPhotoAsync(id, cancellationToken),
        //        VideoAddress = await postServices.GetVideoAsync(id, cancellationToken),
        //        CreationDate = post.CreationDate.ToString(),
        //        ModificationDate = post.ModificationDate.ToString(),
        //        DeletionDate = post.DeletionDate.ToString(),
        //        IsDeleted = post.IsDeleted.ToString(),
        //        UserCraetionName = await userServices.GetUserNameByIdAsync(post.UserCreationId, cancellationToken),
        //        UserModificationName = await userServices.GetUserNameByIdAsync(post.UserModificationId, cancellationToken),
        //        UserDeletionName = await userServices.GetUserNameByIdAsync(post.UserDeletionId, cancellationToken),
        //        UserId = post.UserId
        //    };
        //    return postSelectDto;
        //}

        [HttpPost("[action]")]
        public virtual async Task<ApiResult> Photo(IFormFile file,int id, CancellationToken cancellationToken)
        {
            int userId = Convert.ToInt32(signInManager.Context.Request.HttpContext.User.Identity.GetUserId());
            var post = await postRepository.GetByIdAsync(cancellationToken, id);

            if (post.UserDeletionId == userId)
            {
                if (file.ContentType.ToString() is "image/jpg" or "image/png" or "image/jpeg" or "image/bnp")
                {
                    var filePath = Path.GetTempFileName();

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }
                    await postServices.NewPhotoHandlerAsync(filePath, id, cancellationToken);
                }
                else
                {
                    throw new BadRequestException("فرمت فایل ارسالی اشتباه است");
                }

                return Ok();
            }

            throw new BadRequestException("عدم دسترسی");
        }

        [HttpPost("[action]")]
        public virtual async Task<ApiResult> Video(IFormFile file, int id, CancellationToken cancellationToken)
        {
            int userId = Convert.ToInt32(signInManager.Context.Request.HttpContext.User.Identity.GetUserId());
            var post = await postRepository.GetByIdAsync(cancellationToken, id);

            if (post.UserDeletionId == userId)
            {
                if (file.ContentType.ToString() is "image/MP4" or "image/MOV" or "image/WMV" or "image/MKV")
                {
                    var filePath = Path.GetTempFileName();

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }
                    await postServices.NewVideoHandlerAsync(filePath, id, cancellationToken);
                }
                else
                {
                    throw new BadRequestException("فرمت فایل ارسالی اشتباه است");
                }

                return Ok();
            }

            throw new BadRequestException("عدم دسترسی");
        }
    }
}
