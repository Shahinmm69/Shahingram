using Common;
using Data.Contract;
using Data.Repositories;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Contract;
using System.Drawing.Printing;
using System.Linq;

namespace Services.Services
{
    public class PostServices : IScopedDependency, IPostServices
    {
        protected readonly IRepository<PostPhoto> postphotorepository;
        protected readonly IRepository<PostVideo> postvideorepository;
        protected readonly IRepository<PostHashtag> posthashtagrepository;
        protected readonly IRepository<Photo> photorepository;
        protected readonly IRepository<Video> videorepository;
        protected readonly IRepository<Hashtag> hashtagrepository;
        protected readonly IRepository<Like> likerepository;
        protected readonly IRepository<Comment> commentrepository;
        protected readonly IRepository<Post> postrepository;
        protected readonly IDeletionRepository<Photo> deletephotorepository;
        protected readonly IDeletionRepository<Video> deletevideorepository;
        protected readonly ICreationRepository<Photo> creationphotorepository;
        protected readonly ICreationRepository<Video> creationvideorepository;
        protected readonly ICreationRepository<Direct> creationdirectorepository;
        protected readonly ICreationRepository<Hashtag> creationhashtagrepository;
        private readonly SignInManager<User> signInManager;
        public PostServices(IRepository<PostPhoto> postphotorepository, IRepository<PostVideo> postvideorepository
            , IRepository<PostHashtag> posthashtagrepository, IRepository<Photo> photorepository, IRepository<Video> videorepository, IRepository<Hashtag> hashtagrepository
            , IRepository<Like> likerepository, IRepository<Post> postrepository, IDeletionRepository<Photo> deletephotorepository, IDeletionRepository<Video> deletevideorepository
            , ICreationRepository<Photo> creationphotorepository, ICreationRepository<Video> creationvideorepository, ICreationRepository<Direct> creationdirectorepository
            , ICreationRepository<Hashtag> creationhashtagrepository, IRepository<Comment> commentrepository, SignInManager<User> signInManager)
        {
            this.postphotorepository = postphotorepository;
            this.postvideorepository = postvideorepository;
            this.posthashtagrepository = posthashtagrepository;
            this.videorepository = videorepository;
            this.photorepository = photorepository;
            this.hashtagrepository = hashtagrepository;
            this.likerepository = likerepository;
            this.postrepository = postrepository;
            this.deletephotorepository = deletephotorepository;
            this.deletevideorepository = deletevideorepository;
            this.creationphotorepository = creationphotorepository;
            this.creationvideorepository = creationvideorepository;
            this.creationdirectorepository = creationdirectorepository;
            this.creationhashtagrepository = creationhashtagrepository;
            this.commentrepository = commentrepository;
            this.signInManager = signInManager;
        }

        public async Task NewPhotoHandlerAsync(string address, int id, CancellationToken cancellationToken)
        {
            var post = await postrepository.TableNoTracking.Where(x => x.Id == id).SingleOrDefaultAsync();

            if (post != null)
            {
                var newphoto = new Photo() { Address = address, UserCreationId = post.UserCreationId, Describtion = await GetDescribtionAsync(id, cancellationToken) };
                await creationphotorepository.CraetionDateAsync(newphoto, cancellationToken);
                var newpostphoto = new PostPhoto() { PostId = id, PhotoId = newphoto.Id };
                await postphotorepository.AddAsync(newpostphoto, cancellationToken);
            }
        }

        public async Task NewVideoHandlerAsync(string address, int id, CancellationToken cancellationToken)
        {
            var post = await postrepository.TableNoTracking.Where(x => x.Id == id).SingleOrDefaultAsync();

            if (post != null)
            {
                var newvideo = new Video() { Address = address, UserCreationId = post.UserCreationId, Describtion = await GetDescribtionAsync(id, cancellationToken) };
                await creationvideorepository.CraetionDateAsync(newvideo, cancellationToken);
                var newpostvideo = new PostVideo() { PostId = id, VideoId = newvideo.Id };
                await postvideorepository.AddAsync(newpostvideo, cancellationToken);
            }
        }

        public async Task<List<string>?> GetPhotoAsync(int id, CancellationToken cancellationToken)
        {
            var postphotos = await postphotorepository.TableNoTracking.Where(x => x.PostId == id).Include(x => x.Photo).ToListAsync();
            var post = await postrepository.GetByIdAsync(cancellationToken, id);
            List<string>? result = new List<string>();

            if (post != null && postphotos.Count != 0)
            {
                foreach (var postphoto in postphotos)
                    if (post.IsDeleted == true && postphoto.Photo.IsDeleted != true)
                    {
                        postphoto.Photo.Describtion = postphoto.Photo.Describtion + ", Is Deleted";
                        await deletephotorepository.DeletionDateAsync(postphoto.Photo, cancellationToken);
                        postphotos.Remove(postphoto);
                        result.Add(postphoto.Photo.Address);
                    }
            }

            return result;
        }

        public async Task<List<string>?> GetVideoAsync(int id, CancellationToken cancellationToken)
        {
            var postvideos = await postvideorepository.TableNoTracking.Where(x => x.PostId == id).Include(x => x.Video).ToListAsync();
            var post = await postrepository.GetByIdAsync(cancellationToken, id);
            List<string>? result = new List<string>();

            if (post != null && postvideos.Count != 0)
            {
                foreach (var postvideo in postvideos)
                    if (post.IsDeleted == true && postvideo.Video.IsDeleted != true)
                    {
                        postvideo.Video.Describtion = postvideo.Video.Describtion + ", Is Deleted";
                        await deletevideorepository.DeletionDateAsync(postvideo.Video, cancellationToken);
                        postvideos.Remove(postvideo);
                        result.Add(postvideo.Video.Address);
                    }
            }

            return result;
        }

        public async Task<List<Like>?> GetLikesAsync(int id, int pageNumer, int pageSize, CancellationToken cancellationToken) =>
            await likerepository.TableNoTracking.Where(x => x.PostId == id && x.IsDeleted == false).Skip((pageNumer - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        public async Task<int> GetLikesAsync(int id, CancellationToken cancellationToken) =>
          await likerepository.TableNoTracking.Where(x => x.PostId == id && x.IsDeleted == false).CountAsync(cancellationToken);

        public async Task<List<Comment>?> GetCommentsAsync(int id, int pageNumer, int pageSize, CancellationToken cancellationToken) =>
            await commentrepository.TableNoTracking.Where(x => x.PostId == id && x.IsDeleted == false).Skip((pageNumer - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        public async Task HashtagsHandlerAsync(int id, CancellationToken cancellationToken)
        {
            int index;
            var post = await postrepository.GetByIdAsync(cancellationToken, id);
            string? text = post.Text;
            if (text != null)
            {
                var hashtags = text.Split('#');
                if (text.StartsWith('#'))
                    index = 0;
                else
                    index = 1;
                for (int i = index; i < hashtags.Length; i++)
                {
                    var hashtag = await hashtagrepository.TableNoTracking.Where(u => u.Title.Equals(hashtags[i], StringComparison.OrdinalIgnoreCase)).SingleAsync();

                    if (hashtag.IsDeleted != true)
                    {
                        if (hashtag != null)
                        {
                            var newposthashtag = new PostHashtag() { PostId = id, HashtagId = hashtag.Id };
                            await posthashtagrepository.AddAsync(newposthashtag, cancellationToken);
                        }
                        else
                        {
                            var newhashtag = new Hashtag() { Title = hashtags[i], UserCreationId = post.UserCreationId };
                            await creationhashtagrepository.CraetionDateAsync(newhashtag, cancellationToken);
                            var newposthashtag = new PostHashtag() { PostId = id, HashtagId = newhashtag.Id };
                            await posthashtagrepository.AddAsync(newposthashtag, cancellationToken);
                        }
                    }
                }
            }
        }

        //public async Task<List<Post>?> LoadContentAsync(CancellationToken cancellationToken, params int[] ids)
        //{
        //    var result = new List<Post>();
        //    foreach (var id in ids)
        //    {
        //        var post = await postrepository.GetByIdAsync(cancellationToken, id);
        //        var photo = await GetPhotoAsync(id, cancellationToken);
        //        var video = await GetVideoAsync(id, cancellationToken);
        //        if (photo is not null)
        //            result.Add(photo);
        //        else if (video is not null)
        //            result.Add(video);
        //        else
        //            result.Add(post);
        //    }
        //    return result;
        //}

        public async Task ForwardAsync(int id, int userreceiverid, CancellationToken cancellationToken)
        {
            var post = await postrepository.GetByIdAsync(cancellationToken, id);
            await creationdirectorepository.CraetionDateAsync(new Direct()
            {
                Text = post.Text,
                PostId = post.Id,
                UserCreationId = post.UserCreationId,
                UserReceiverId = userreceiverid,
                Post = post
            }, cancellationToken);
        }

        public async Task<string> GetDescribtionAsync(int id, CancellationToken cancellationToken)
        {
            var post = await postrepository.GetByIdAsync(cancellationToken, id);
            return $"UserId: {post.UserCreationId} - PostId: {post.Id}";
        }
    }
}

