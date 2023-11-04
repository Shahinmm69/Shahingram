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
            var user = await userrepository.TableNoTracking.Where(x => x.UserName == entity.UserName).LastAsync();

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
            var userphoto = await userphotorepository.TableNoTracking.Where(x => x.UserId == id).Include(x => x.Photo).Where(x => x.Photo.IsDeleted == false).ToListAsync();

            if (userphoto is null)
            {
                var newphoto = new Photo() { Address = address, UserCraetionId = id, Describtion = await GetDescribtionAsync(id, cancellationToken) };
                await creationphotorepository.CraetionDateAsync(newphoto, cancellationToken);
                var newuserphoto = new UserPhoto() { UserId = id, PhotoId = newphoto.Id };
                await userphotorepository.AddAsync(newuserphoto, cancellationToken);
            }
            else
                throw new BadRequestException("شما یک تصویر فعال دارید");
        }

        public async Task DeletePhotoHandlerAsync(int id, CancellationToken cancellationToken)
        {
            var userphoto = await userphotorepository.TableNoTracking.Where(x => x.UserId == id).Include(x => x.Photo).Where(x => x.Photo.IsDeleted == false).SingleAsync();
            if (userphoto is null)
                throw new BadRequestException("شما تصویر فعالی ندارید");
            else
                await deletephotorepository.DeletionDateAsync(userphoto.Photo, cancellationToken);
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
            var userphoto = await userphotorepository.TableNoTracking.Where(x => x.UserId == id).Include(x => x.Photo).Where(x => x.Photo.IsDeleted == false).SingleAsync();
            var photo = userphoto.Photo;
            var user = await userrepository.TableNoTracking.Where(x => x.Id == id).Include(x => userphoto).ToListAsync();

            if (photo is not null)
                if (user[0].IsDeleted == true && photo.IsDeleted != true)
                {
                    photo.Describtion = photo.Describtion + ", Is Deleted";
                    await deletephotorepository.DeletionDateAsync(photo, cancellationToken);
                }

            return photo.Address;
        }

        public async Task<List<int>?> GetPostsIdAsync(int id, CancellationToken cancellationToken)
        {
            List<int> ids = new List<int>();
            var userposts = await postrepository.TableNoTracking.Where(x => x.UserId == id).ToListAsync(cancellationToken);
            if (userposts is not null)
                foreach (var userpost in userposts)
                {
                    if (userpost.IsDeleted != true)
                        ids.Add(userpost.Id);
                }
            return ids;
        }

        public async Task<int> GetPostsCountAsync(int id, CancellationToken cancellationToken)
        {
            var userposts = await postrepository.TableNoTracking.Where(x => x.UserId == id).ToListAsync(cancellationToken);
            if (userposts is not null)
                foreach (var userpost in userposts)
                {
                    if (userpost.IsDeleted == true)
                        userposts.Remove(userpost);
                }
            return userposts.Count();
        }

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
            var user = await userrepository.GetByIdAsync(cancellationToken, id);
            var directlist1 = user.Directs.Where(x => x.UserSenderId == id && x.SenderIsDeleted == false).ToList();
            var directlist2 = user.Directs.Where(x => x.UserReceiverId == id && x.ReceiverIsDeleted == false).ToList();

            if (directlist1 is not null)
                foreach (var direct in directlist1)
                {
                    userIds.Add(direct.UserReceiverId);
                }
            if (directlist2 is not null)
                foreach (var direct in directlist2)
                {
                    userIds.Add(direct.UserSenderId);
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
            var sendDirects = user.Directs.Where(x => x.UserSenderId == id && x.SenderIsDeleted == false && x.UserReceiverId == anotherid).ToList();
            var receiveDirects = user.Directs.Where(x => x.UserReceiverId == id && x.ReceiverIsDeleted == false && x.UserSenderId == anotherid).ToList();

            if (sendDirects != null || receiveDirects != null)
            {
                sendDirects.AddRange(receiveDirects);
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
            var followers = user.Follows.Where(x => x.UserId == id && x.IsDeleted == false).ToList();

            if (followers is not null)
                foreach (var follower in followers)
                {
                    if (follower.IsDeleted != true)
                        ids.Add(follower.Id);
                }
            return ids;
        }

        public async Task<List<int>?> GetFollowingsAsync(int id, CancellationToken cancellationToken)
        {
            List<int> ids = new List<int>();
            var user = await userrepository.GetByIdAsync(cancellationToken, id);
            var followings = user.Follows.Where(x => x.FollowId == id && x.IsDeleted == false).ToList();

            if (followings is not null)
                foreach (var following in followings)
                {
                    if (following.IsDeleted == true)
                        ids.Add(following.Id);
                }
            return ids;
        }

        public async Task<int> GetFollowersCountAsync(int id, CancellationToken cancellationToken)
        {
            var user = await userrepository.GetByIdAsync(cancellationToken, id);
            var followers = user.Follows.Where(x => x.UserId == id && x.IsDeleted == false).ToList();

            foreach (var follower in followers)
            {
                if (followers is not null)
                {
                    if (follower.IsDeleted == true)
                        followers.Remove(follower);
                }
            }
            return followers.Count();
        }

        public async Task<int> GetFollowingsCountAsync(int id, CancellationToken cancellationToken)
        {
            var user = await userrepository.GetByIdAsync(cancellationToken, id);
            var followings = user.Follows.Where(x => x.FollowId == id && x.IsDeleted == false).ToList();

            foreach (var following in followings)
            {
                if (followings is not null)
                {
                    if (following.IsDeleted == true)
                        followings.Remove(following);
                }
            }
            return followings.Count();
        }

        public async Task<string> GetDescribtionAsync(int id, CancellationToken cancellationToken)
        {
            var user = await userrepository.GetByIdAsync(cancellationToken, id);
            return $"Porofile image for userId: {user.Id}";
        }
    }
}
