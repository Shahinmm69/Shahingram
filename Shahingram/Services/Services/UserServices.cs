using Common;
using Common.Exceptions;
using Common.Utilities;
using Data.Contract;
using Entities.Common;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Services
{
    public class UserServices : IScopedDependency, IUserServices
    {
        protected readonly IRepository<Category> categoryrepository;
        protected readonly IRepository<Country> countryrepository;
        protected readonly IRepository<UserPhoto> userphotorepository;
        protected readonly IRepository<Photo> photorepository;
        protected readonly IRepository<Video> videorepository;
        protected readonly IRepository<Direct> directrepository;
        protected readonly IRepository<Post> postrepository;
        protected readonly IRepository<Like> likerepository;
        protected readonly IRepository<Follow> followrepository;
        protected readonly IRepository<Comment> commentrepository;
        protected readonly IDeletionRepository<Photo> deletephotorepository;
        protected readonly IDeletionRepository<Video> deletevideorepository;
        protected readonly ICreationRepository<Photo> creationphotorepository;
        protected readonly ICreationRepository<Video> creationvideorepository;
        protected readonly ICreationRepository<Direct> creationdirectorepository;
        protected readonly ICreationRepository<Hashtag> creationhashtagrepository;
        protected readonly ICreationRepository<Post> creationpostrepository;
        protected readonly IUserRepository userrepository;
        protected readonly IPostServices postservices;
        public UserServices(IRepository<UserPhoto> userphotorepository, IRepository<Post> postrepository, IRepository<Photo> photorepository, IRepository<Video> videorepository
            , IRepository<Direct> directrepository, IDeletionRepository<Photo> deletephotorepository, IDeletionRepository<Video> deletevideorepository
            , ICreationRepository<Photo> creationphotorepository, ICreationRepository<Video> creationvideorepository, ICreationRepository<Direct> creationdirectorepository
            , ICreationRepository<Hashtag> creationhashtagrepository, IPostServices postservices, ICreationRepository<Post> creationpostrepository, IUserRepository userrepository
            , IRepository<Category> categoryrepository, IRepository<Country> countryrepository, IRepository<Like> likerepository, IRepository<Follow> followrepository, IRepository<Comment> commentrepository)
        {
            this.userphotorepository = userphotorepository;
            this.postrepository = postrepository;
            this.videorepository = videorepository;
            this.photorepository = photorepository;
            this.directrepository = directrepository;
            this.deletephotorepository = deletephotorepository;
            this.deletevideorepository = deletevideorepository;
            this.creationphotorepository = creationphotorepository;
            this.creationvideorepository = creationvideorepository;
            this.creationdirectorepository = creationdirectorepository;
            this.creationhashtagrepository = creationhashtagrepository;
            this.postservices = postservices;
            this.creationpostrepository = creationpostrepository;
            this.userrepository = userrepository;
            this.categoryrepository = categoryrepository;
            this.countryrepository = countryrepository;
            this.likerepository = likerepository;
            this.followrepository = followrepository;
            this.commentrepository = commentrepository;
        }

        public async Task CraetionConfigAsync(User entity, CancellationToken cancellationToken)
        {
            var user = await userrepository.TableNoTracking.Where(x => x.UserName == entity.UserName).LastAsync();

            if (user == null || user.IsDeleted == true)
            {
                await userrepository.CraetionDateAsync(entity, cancellationToken);
            }
            else
            {
                throw new BadRequestException("نام کاریری تکراری میباشد");
            }
        }

        public async Task NewPhotoHandlerAsync(string address, int id, CancellationToken cancellationToken)
        {
            var newphoto = new Photo() { Address = address, UserCraetionId = id, Describtion = await GetDescribtionAsync(id, cancellationToken) };
            await creationphotorepository.CraetionDateAsync(newphoto, cancellationToken);
            var newuserphoto = new UserPhoto() { UserId = id, PhotoId = newphoto.Id };
            await userphotorepository.AddAsync(newuserphoto, cancellationToken);
        }

        public async Task NewPostHandlerAsync(object? file, string? text, int id, int userid, CancellationToken cancellationToken)
        {
            if (file is Photo)
            {
                var result = file as Photo;
                var newpost = new Post() { UserId = userid, Text = text };
                await creationpostrepository.CraetionDateAsync(newpost, cancellationToken);
                await postservices.NewPhotoHandlerAsync(result.Address, newpost.Id, cancellationToken);
            }
            if (file is Video)
            {
                var result = file as Video;
                var newpost = new Post() { UserId = userid, Text = text };
                await creationpostrepository.CraetionDateAsync(newpost, cancellationToken);
                await postservices.NewVideoHandlerAsync(result.Address, newpost.Id, cancellationToken);
            }
        }

        public async Task<int> GetCategoryIdAsync(string title, CancellationToken cancellationToken)
        {
            var category = await categoryrepository.TableNoTracking.Where(x => x.Title == title).SingleAsync();

            if (category is not null)
                return category.Id;
            else
                throw new NotFoundException("Category isn't exist");
        }

        public async Task<int> GetCountryIdAsync(string title, CancellationToken cancellationToken)
        {
            var country = await countryrepository.TableNoTracking.Where(x => x.Title == title).SingleAsync();

            if (country is not null)
                return country.Id;
            else
                throw new NotFoundException("Country isn't exist");
        }

        public async Task<Photo> GetPhotoAsync(int id, CancellationToken cancellationToken)
        {
            var userphoto = await userphotorepository.TableNoTracking.Where(x => x.UserId == id).SingleAsync();
            var photo = userphoto.Photo;
            var user = await userrepository.GetByIdAsync(cancellationToken, id);

            if (photo is not null)
            {
                if (user.IsDeleted == true && photo.IsDeleted != true)
                {
                    photo.Describtion = photo.Describtion + ", Is Deleted";
                    await deletephotorepository.DeletionDateAsync(photo, cancellationToken);
                }

                //one image that's show "This item is not exist!"
                if (photo.IsDeleted == true)
                    return await photorepository.TableNoTracking.Where(x => x.Id == 1).SingleAsync();
            }

            return photo;
        }

        public async IAsyncEnumerable<Post> GetPostsAsync(int userid, CancellationToken cancellationToken)
        {
            var user = await userrepository.GetByIdAsync(cancellationToken, userid);
            var posts = user.Posts.Where(x => x.UserId == userid && x.IsDeleted == false);

            foreach (var post in posts)
            {
                if (posts is not null)
                {
                    if (post.IsDeleted == true)
                        yield return null;
                }
                else
                    yield return post;
            }
        }

        public async IAsyncEnumerable<Direct> GetSendDirectsAsync(int userid, CancellationToken cancellationToken)
        {
            var user = await userrepository.GetByIdAsync(cancellationToken, userid);
            var directs = user.Directs.Where((x => x.UserSenderId == userid && x.SenderIsDeleted == false));

            foreach (var direct in directs)
            {
                if (directs is not null)
                {
                    if (direct.IsDeleted == true)
                        yield return null;
                }
                else
                    yield return direct;
            }
        }

        public async IAsyncEnumerable<Direct> GetRecieveDirectsAsync(int userid, CancellationToken cancellationToken)
        {
            var user = await userrepository.GetByIdAsync(cancellationToken, userid);
            var directs = user.Directs.Where((x => x.UserReceiverId == userid && x.ReceiverIsDeleted == false));

            foreach (var direct in directs)
            {
                if (directs is not null)
                {
                    if (direct.IsDeleted == true)
                        yield return null;
                }
                else
                    yield return direct;
            }
        }

        public async IAsyncEnumerable<Follow> GetFollowersAsync(int userid, CancellationToken cancellationToken)
        {
            var user = await userrepository.GetByIdAsync(cancellationToken, userid);
            var followers = user.Follows.Where((x => x.UserId == userid && x.IsDeleted == false));

            foreach (var follower in followers)
            {
                if (followers is not null)
                {
                    // one image that's show "This item is not exist!"
                    if (follower.IsDeleted == true)
                        yield return null;
                }
                else
                    yield return follower;
            }
        }

        public async IAsyncEnumerable<Follow> GetFollowingsAsync(int userid, CancellationToken cancellationToken)
        {
            var user = await userrepository.GetByIdAsync(cancellationToken, userid);
            var followings = user.Follows.Where((x => x.FollowId == userid && x.IsDeleted == false));

            foreach (var following in followings)
            {
                if (followings is not null)
                {
                    // one image that's show "This item is not exist!"
                    if (following.IsDeleted == true)
                        yield return null;
                }
                else
                    yield return following;
            }
        }

        public async Task<string> GetDescribtionAsync(int id, CancellationToken cancellationToken)
        {
            var user = await userrepository.GetByIdAsync(cancellationToken, id);
            return $"Porofile image for userId: {user.Id}";
        }
    }
}
