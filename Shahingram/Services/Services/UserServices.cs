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
            , IRepository<Category> categoryrepository, IRepository<Country> countryrepository, IRepository<Like> likerepository, IRepository<Follow> followrepository
            , IRepository<Comment> commentrepository)
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

        public async Task<User> CraetionConfigAsync(User entity, CancellationToken cancellationToken)
        {
            var user = await userrepository.TableNoTracking.Where(x => x.UserName == entity.UserName).OrderBy(x => x.DeletionDate).LastOrDefaultAsync();

            if (user == null || user.IsDeleted == true)
            {
                entity.CrationDate = DateTime.Now;
            }
            else
            {
                throw new BadRequestException("نام کاریری تکراری میباشد");
            }
            return entity;
        }

        public async Task NewPhotoHandlerAsync(string address, int id, CancellationToken cancellationToken)
        {
            var userphoto = await userphotorepository.TableNoTracking.Where(x => x.UserId == id).Where(x => x.Photo.IsDeleted != true)
                .SingleOrDefaultAsync();

            if (userphoto is null)
            {
                var newphoto = new Photo() { Address = address, UserCreationId = id, Describtion = await GetDescribtionAsync(id, cancellationToken) };
                await creationphotorepository.CraetionDateAsync(newphoto, cancellationToken);
                var newuserphoto = new UserPhoto() { UserId = id, PhotoId = newphoto.Id };
                await userphotorepository.AddAsync(newuserphoto, cancellationToken);
            }
            else
                throw new BadRequestException("شما یک تصویر فعال دارید");
        }

        public async Task DeletePhotoHandlerAsync(int id, CancellationToken cancellationToken)
        {
            var userphoto = await userphotorepository.TableNoTracking.Where(x => x.UserId == id).Where(x => x.Photo.IsDeleted != true).Include(x => x.Photo).SingleOrDefaultAsync();
            if (userphoto is null)
                throw new BadRequestException("شما تصویر فعالی ندارید");
            else
            {
                userphoto.Photo.Describtion = userphoto.Photo.Describtion + ", Is Deleted";
                await deletephotorepository.DeletionDateAsync(userphoto.Photo, cancellationToken);
            }
        }

        //public async Task NewPostHandlerAsync(object? file, string? text, int id, int userid, CancellationToken cancellationToken)
        //{
        //    if (file is Photo)
        //    {
        //        var result = file as Photo;
        //        var newpost = new Post() { UserId = userid, Text = text };
        //        await creationpostrepository.CraetionDateAsync(newpost, cancellationToken);
        //        await postservices.NewPhotoHandlerAsync(result.Address, newpost.Id, cancellationToken);
        //    }
        //    if (file is Video)
        //    {
        //        var result = file as Video;
        //        var newpost = new Post() { UserId = userid, Text = text };
        //        await creationpostrepository.CraetionDateAsync(newpost, cancellationToken);
        //        await postservices.NewVideoHandlerAsync(result.Address, newpost.Id, cancellationToken);
        //    }
        //}

        public async Task<string> GetUserNameByIdAsync(int? id, CancellationToken cancellationToken)
        {
            var user = await userrepository.GetByIdAsync(cancellationToken, id);
            return user?.UserName;
        }

        public async Task<string> GetCategoryTitleByIdAsync(int id, CancellationToken cancellationToken)
        {
            var category = await categoryrepository.GetByIdAsync(cancellationToken, id);
            return category.Title;
        }

        public async Task<string> GetCountryTitleByIdAsync(int id, CancellationToken cancellationToken)
        {
            var country = await countryrepository.GetByIdAsync(cancellationToken, id);
            return country.Title;
        }

        public async Task<string?> GetPhotoAsync(int id, CancellationToken cancellationToken)
        {
            var userphoto = await userphotorepository.TableNoTracking.Where(x => x.UserId == id).Include(x => x.Photo).Where(x => x.Photo.IsDeleted != true).SingleOrDefaultAsync();
            var photo = userphoto?.Photo;

            if (photo is not null)
            {
                var user = await userrepository.GetByIdAsync(cancellationToken, id);

                if (user.IsDeleted == true && photo.IsDeleted != true)
                {
                    photo.Describtion = photo.Describtion + ", Is Deleted";
                    await deletephotorepository.DeletionDateAsync(photo, cancellationToken);
                }

                return photo.Address;
            }
            return null;
        }

        public async Task<List<int>?> GetPostsIdAsync(int id, CancellationToken cancellationToken)
        {
            List<int> ids = new List<int>();
            var userposts = await postrepository.TableNoTracking.Where(x => x.UserCreationId == id && x.IsDeleted != true).ToListAsync(cancellationToken);
            if (userposts.Count != 0)
                foreach (var userpost in userposts)
                {
                    ids.Add(userpost.Id);
                }
            return ids;
        }

        public async Task<int> GetPostsCountAsync(int id, CancellationToken cancellationToken) =>
            await postrepository.TableNoTracking.Where(x => x.UserCreationId == id && x.IsDeleted != true).CountAsync(cancellationToken);

        //public async Task<List<User>?> LoadContentAsync(int id, CancellationToken cancellationToken)
        //{
        //    var userphotos = await GetPhotoAsync(id, cancellationToken);
        //    var userposts = await GetPostsAsync(id, cancellationToken);
        //    var user = (from userpost in userposts
        //                join userphoto in userphotos on userpost.Id equals userphoto.Id
        //                select new User()).ToList();
        //    return user;
        //}

        public async Task<List<User>?> GetUsersHaveDirectsAsync(int id, CancellationToken cancellationToken)
        {
            List<User> users = new List<User>();
            List<int> userIds = new List<int>();
            var sendDirects = await directrepository.TableNoTracking.Where(x=>x.UserCreationId ==id && x.SenderIsDeleted != true).ToListAsync(cancellationToken);
            var receiveDirects = await directrepository.TableNoTracking.Where(x=>x.UserReceiverId==id && x.ReceiverIsDeleted != true).ToListAsync(cancellationToken);

            if (sendDirects is not null)
                foreach (var direct in sendDirects)
                {
                    userIds.Add(direct.UserReceiverId);
                }
            if (receiveDirects is not null)
                foreach (var direct in receiveDirects)
                {
                    userIds.Add(direct.UserCreationId);
                }

            userIds = userIds.Distinct().ToList();

            foreach (var userId in userIds)
                users.Add(await userrepository.GetByIdAsync(cancellationToken, userId));

            return users;
        }

        public async Task<List<int>?> GetDirectsWithAnotherAsync(int id, int anotherid, CancellationToken cancellationToken)
        {
            List<int> ids = new List<int>();
            var user = await userrepository.GetByIdAsync(cancellationToken, id);
            var sendDirects = await directrepository.TableNoTracking.Where(x => x.UserCreationId == id && x.SenderIsDeleted != true && x.UserReceiverId == anotherid)
                .ToListAsync(cancellationToken);
            var receiveDirects = await directrepository.TableNoTracking.Where(x => x.UserReceiverId == id && x.ReceiverIsDeleted != true && x.UserCreationId == anotherid)
                .ToListAsync(cancellationToken);

            if (receiveDirects is not null)
            {
                sendDirects?.AddRange(receiveDirects);
                if (sendDirects != null)
                    foreach (var direct in sendDirects)
                    {
                        ids.Add(direct.Id);
                    }
            }
            return ids;
        }

        public async Task<List<int>?> GetFollowersAsync(int id, CancellationToken cancellationToken)
        {
            List<int> ids = new List<int>();
            var user = await userrepository.GetByIdAsync(cancellationToken, id);
            var followers = await followrepository.TableNoTracking.Where(x => x.UserCreationId == id && x.IsDeleted != true).ToListAsync(cancellationToken);

            if (followers is not null)
                foreach (var follower in followers)
                    ids.Add(follower.Id);
            return ids;
        }

        public async Task<List<int>?> GetFollowingsAsync(int id, CancellationToken cancellationToken)
        {
            List<int> ids = new List<int>();
            var user = await userrepository.GetByIdAsync(cancellationToken, id);
            var followings = await followrepository.TableNoTracking.Where(x => x.UserFollowId == id && x.IsDeleted != true).ToListAsync();

            if (followings is not null)
                foreach (var following in followings)
                    ids.Add(following.Id);
            return ids;
        }

        public async Task<int?> GetFollowersCountAsync(int id, CancellationToken cancellationToken)
        {
            var user = await userrepository.GetByIdAsync(cancellationToken, id);
            var result = await followrepository.TableNoTracking.Where(x => x.UserCreationId == id && x.IsDeleted != true).CountAsync();

            return result;
        }

        public async Task<int?> GetFollowingsCountAsync(int id, CancellationToken cancellationToken)
        {
            var user = await userrepository.GetByIdAsync(cancellationToken, id);
            var result = await followrepository.TableNoTracking.Where(x => x.UserFollowId == id && x.IsDeleted != true).CountAsync();

            return result;
        }

        public async Task<string> GetDescribtionAsync(int id, CancellationToken cancellationToken)
        {
            var user = await userrepository.GetByIdAsync(cancellationToken, id);
            return $"Porofile image for userId: {user.Id}";
        }
    }
}
