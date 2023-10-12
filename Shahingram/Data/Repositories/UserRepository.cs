using Common;
using Common.Exceptions;
using Common.Utilities;
using Data.Common;
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
    public class UserRepository : Repository<User>, IDeletionRepository<User>
    {
        protected readonly ApplicationDbContext DbContext;
        protected readonly IRepository<UserPhoto> userphotorepository;
        protected readonly IRepository<Photo> photorepository;
        protected readonly IRepository<Video> videorepository;
        protected readonly IRepository<Direct> directrepository;
        protected readonly IRepository<Post> postrepository;
        protected readonly IRepository<Like> likerepository;
        protected readonly IRepository<Follow> followrepository;
        protected readonly IRepository<Comment> commentrepository;
        public UserRepository(ApplicationDbContext dbContext, IRepository<UserPhoto> userphotorepository, IRepository<Post> postrepository
            , IRepository<Photo> photorepository, IRepository<Video> videorepository, IRepository<Direct> directrepository)
            : base(dbContext)
        {
            DbContext = dbContext;
            this.userphotorepository = userphotorepository;
            this.postrepository = postrepository;
            this.videorepository = videorepository;
            this.photorepository = photorepository;
            this.directrepository = directrepository;
        }

        public Task DeletionDateAsync(User entity, CancellationToken cancellationToken)
        {
            entity.DeletionDate = DateTime.Now;
            entity.IsDeleted = true;
            var p = DbContext.Set<UserPhoto>()
                .AsNoTracking()
                .Where(p => p.UserId == entity.Id)
                .Include(p => p.Photo.IsDeleted != false).Single();
            var photo = photorepository.GetById(p.PhotoId);
            var posts = DbContext.Set<Post>()
                .AsNoTracking()
                .Where(p => p.UserId == entity.Id && p.IsDeleted != true);
            if (photo != null)
            {
                photo.DeletionDate = DateTime.Now;
                photo.Describtion = photo.Describtion + ", Is Deleted";
                photo.IsDeleted = true;
                photorepository.DeleteAsync(photo, cancellationToken);
            }
            if (posts != null)
            {
                foreach (var post in posts)
                {
                    post.DeletionDate = DateTime.Now;
                    post.IsDeleted = true;
                    postrepository.DeleteAsync(post, cancellationToken);
                }
            }
            return DeleteAsync(entity, cancellationToken);
        }

        public Task NewPhotoHandler(string address, int id, int userid, CancellationToken cancellationToken)
        {
            var newphoto = new Photo() { Address = address, CrationDate = DateTime.Now, UserCraetionId = userid, Describtion = GetDescribtion(id, cancellationToken) };
            photorepository.AddAsync(newphoto, cancellationToken);
            var newpostphoto = new PostPhoto() { PostId = id, PhotoId = newphoto.Id };
            Postphotorepository.AddAsync(newpostphoto, cancellationToken);
            return Task.CompletedTask;
        }

        public Task NewVideoHandler(string address, int id, int userid, CancellationToken cancellationToken)
        {
            var newvideo = new Video() { Address = address, CrationDate = DateTime.Now, UserCraetionId = userid, Describtion = GetDescribtion(id, cancellationToken) };
            videorepository.AddAsync(newvideo, cancellationToken);
            var newpostvideo = new PostVideo() { PostId = id, VideoId = newvideo.Id };
            Postvideorepository.AddAsync(newpostvideo, cancellationToken);
            return Task.CompletedTask;
        }

        public Task<Photo> GetPhoto(int id, CancellationToken cancellationToken)
        {
            var u = DbContext.Set<PostPhoto>()
                .AsNoTracking()
                .Where(u => u.PostId == id)
                .Include(u => u.Photo).Single();
            var p = DbContext.Set<Photo>()
                .AsNoTracking()
                .Where(p => p.Id == u.PhotoId)
                .SingleAsync();

            //one image that's show "This item is not exist!"
            if (p.Result.IsDeleted == true)
                return DbContext.Set<Photo>()
                .AsNoTracking()
                .Where(p => p.Id == 1)
                .SingleAsync();

            return p;
        }

        public Task<Video> GetVideo(int id, CancellationToken cancellationToken)
        {
            var u = DbContext.Set<PostVideo>()
                .AsNoTracking()
                .Where(u => u.PostId == id)
                .Include(u => u.Video).Single();
            var p = DbContext.Set<Video>()
                .AsNoTracking()
                .Where(p => p.Id == u.VideoId)
                .SingleAsync();

            //one image that's show "This item is not exist!"
            if (p.Result.IsDeleted == true)
                return DbContext.Set<Video>()
                .AsNoTracking()
                .Where(p => p.Id == 1)
                .SingleAsync();

            return p;
        }

        public IEnumerable<Like>? GetLike(int id, CancellationToken cancellationToken)
        {
            var likes = DbContext.Set<Like>()
                .Include(u => u.PostId == id)
                .Where(u => u.IsDeleted == false)
                .ToListAsync();
            foreach (var like in likes.Result)
                yield return likerepository.GetById(cancellationToken, like.Id);
        }

        public Task HashtagsHandler(int id, CancellationToken cancellationToken)
        {
            int index;
            string text = base.GetById(id).Text;
            var hashtags = text.Split('@');
            if (text.StartsWith('@'))
                index = 0;
            else
                index = 1;
            for (int i = index; i < hashtags.Length; i++)
            {
                var hashtag = DbContext.Set<Hashtag>()
                .Where(u => u.Address == hashtags[i]).SingleAsync();

                if (hashtag.Result.IsDeleted != true)
                {
                    if (hashtag != null)
                    {
                        var newposthashtag = new PostHashtag() { PostId = id, HashtagId = hashtag.Id };
                        Posthashtagrepository.AddAsync(newposthashtag, cancellationToken);
                    }
                    else
                    {
                        var newhashtag = new Hashtag() { Address = hashtags[i], CrationDate = DateTime.Now, UserCraetionId = base.GetById(id, cancellationToken).UserCraetionId };
                        hashtagrepository.AddAsync(newhashtag, cancellationToken);
                        var newposthashtag = new PostHashtag() { PostId = id, HashtagId = newhashtag.Id };
                        Posthashtagrepository.AddAsync(newposthashtag, cancellationToken);
                    }
                }
            }
            return Task.CompletedTask;
        }

        public async Task<object?> LoadContentAsync(CancellationToken cancellationToken, params int[] ids)
        {
            object result;
            foreach (var id in ids)
            {
                var post = DbContext.Set<Post>()
                    .AsNoTracking()
                    .Where(u => u.Id == id).SingleAsync();
                if (GetPhoto(post.Id, cancellationToken) is not null)
                {
                    result = await GetPhoto(post.Id, cancellationToken);
                    return result as Photo;
                }
                else if (GetVideo(post.Id, cancellationToken) is not null)
                {
                    result = await GetVideo(post.Id, cancellationToken);
                    return result as Video;
                }
            }
            return Task.CompletedTask;
        }

        public Task Forward(Post post, int userreceiverid, CancellationToken cancellationToken)
        {
            if (post.IsDeleted == false)
                _ = directrepository.AddAsync(new Direct() { Text = post.Text, CrationDate = DateTime.Now, PostId = post.Id, UserCraetionId = post.UserId/*, UserSenderId*/, UserReceiverId = userreceiverid, Post = post }, cancellationToken);
            return Task.CompletedTask;
        }

        public string GetDescribtion(int id, CancellationToken cancellationToken)
        {
            var post = base.GetById(id, cancellationToken);
            return $"UserId: {post.UserCraetionId} - PostId: {post.Id}";
        }
    }
}
