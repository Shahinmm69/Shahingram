using Data.Contract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Contract;

namespace Services.Services
{
    public class PostServices : IPostServices
    {
        protected readonly IRepository<PostPhoto> postphotorepository;
        protected readonly IRepository<PostVideo> postvideorepository;
        protected readonly IRepository<PostHashtag> posthashtagrepository;
        protected readonly IRepository<Photo> photorepository;
        protected readonly IRepository<Video> videorepository;
        protected readonly IRepository<Hashtag> hashtagrepository;
        protected readonly IRepository<Like> likerepository;
        protected readonly IRepository<Comment> commentrepository;
        protected readonly IRepository<Post> postepository;
        protected readonly IDeletionRepository<Photo> deletephotorepository;
        protected readonly IDeletionRepository<Video> deletevideorepository;
        protected readonly ICreationRepository<Photo> creationphotorepository;
        protected readonly ICreationRepository<Video> creationvideorepository;
        protected readonly ICreationRepository<Direct> creationdirectorepository;
        protected readonly ICreationRepository<Hashtag> creationhashtagrepository;
        public PostServices(IRepository<PostPhoto> postphotorepository, IRepository<PostVideo> postvideorepository
            , IRepository<PostHashtag> posthashtagrepository, IRepository<Photo> photorepository, IRepository<Video> videorepository, IRepository<Hashtag> hashtagrepository
            , IRepository<Like> likerepository, IRepository<Post> postepository, IDeletionRepository<Photo> deletephotorepository, IDeletionRepository<Video> deletevideorepository
            , ICreationRepository<Photo> creationphotorepository, ICreationRepository<Video> creationvideorepository, ICreationRepository<Direct> creationdirectorepository
            , ICreationRepository<Hashtag> creationhashtagrepository, IRepository<Comment> commentrepository)
        {
            this.postphotorepository = postphotorepository;
            this.postvideorepository = postvideorepository;
            this.posthashtagrepository = posthashtagrepository;
            this.videorepository = videorepository;
            this.photorepository = photorepository;
            this.hashtagrepository = hashtagrepository;
            this.likerepository = likerepository;
            this.postepository = postepository;
            this.deletephotorepository = deletephotorepository;
            this.deletevideorepository = deletevideorepository;
            this.creationphotorepository = creationphotorepository;
            this.creationvideorepository = creationvideorepository;
            this.creationdirectorepository = creationdirectorepository;
            this.creationhashtagrepository = creationhashtagrepository;
            this.commentrepository = commentrepository;
        }

        public async Task NewPhotoHandlerAsync(string address, int id, CancellationToken cancellationToken)
        {
            var post = await postepository.GetByIdAsync(cancellationToken, id);
            var newphoto = new Photo() { Address = address, UserCraetionId = post.UserId, Describtion = await GetDescribtionAsync(id, cancellationToken) };
            await creationphotorepository.CraetionDateAsync(newphoto, cancellationToken);
            var newpostphoto = new PostPhoto() { PostId = id, PhotoId = newphoto.Id };
            await postphotorepository.AddAsync(newpostphoto, cancellationToken);
        }

        public async Task NewVideoHandlerAsync(string address, int id, CancellationToken cancellationToken)
        {
            var post = await postepository.GetByIdAsync(cancellationToken, id);
            var newvideo = new Video() { Address = address, UserCraetionId = post.UserId, Describtion = await GetDescribtionAsync(id, cancellationToken) };
            await creationvideorepository.CraetionDateAsync(newvideo, cancellationToken);
            var newpostvideo = new PostVideo() { PostId = id, VideoId = newvideo.Id };
            await postvideorepository.AddAsync(newpostvideo, cancellationToken);
        }

        public async Task<Photo> GetPhotoAsync(int id, CancellationToken cancellationToken)
        {
            var postphoto = await postphotorepository.TableNoTracking.Where(x => x.PostId == id).SingleAsync();
            var photo = postphoto.Photo;
            var post = await postepository.GetByIdAsync(cancellationToken, id);

            if (photo is not null)
            {
                if (post.IsDeleted == true && photo.IsDeleted != true)
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

        public async Task<Video> GetVideoAsync(int id, CancellationToken cancellationToken)
        {
            var postvideo = await postvideorepository.TableNoTracking.Where(x => x.PostId == id).SingleAsync();
            var video = postvideo.Video;
            var post = await postepository.GetByIdAsync(cancellationToken, id);

            if (video is not null)
            {
                if (post.IsDeleted == true && video.IsDeleted != true)
                {
                    video.Describtion = video.Describtion + ", Is Deleted";
                    await deletevideorepository.DeletionDateAsync(video, cancellationToken);
                }

                //one image that's show "This item is not exist!"
                if (video.IsDeleted == true)
                    return await videorepository.TableNoTracking.Where(x => x.Id == 1).SingleAsync();
            }

            return video;
        }

        public async IAsyncEnumerable<Like>? GetLike(int id, CancellationToken cancellationToken)
        {
            var likes = await likerepository.TableNoTracking.Where(x => x.PostId == id && x.IsDeleted == false).ToListAsync();
            foreach (var like in likes)
                yield return await likerepository.GetByIdAsync(cancellationToken, like.Id);
        }


        public async IAsyncEnumerable<Comment>? GetComment(int id, CancellationToken cancellationToken)
        {
            var comments = await commentrepository.TableNoTracking.Where(x => x.PostId == id && x.IsDeleted == false).ToListAsync();
            foreach (var comment in comments)
                yield return await commentrepository.GetByIdAsync(cancellationToken, comment.Id);
        }

        public async Task HashtagsHandler(int id, CancellationToken cancellationToken)
        {
            int index;
            var post = await postepository.GetByIdAsync(cancellationToken, id);
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
                    var hashtag = await hashtagrepository.TableNoTracking.Where(u => u.Title == hashtags[i]).SingleAsync();

                    if (hashtag.IsDeleted != true)
                    {
                        if (hashtag != null)
                        {
                            var newposthashtag = new PostHashtag() { PostId = id, HashtagId = hashtag.Id };
                            await posthashtagrepository.AddAsync(newposthashtag, cancellationToken);
                        }
                        else
                        {
                            var newhashtag = new Hashtag() { Title = hashtags[i], UserCraetionId = post.UserCraetionId };
                            await creationhashtagrepository.CraetionDateAsync(newhashtag, cancellationToken);
                            var newposthashtag = new PostHashtag() { PostId = id, HashtagId = newhashtag.Id };
                            await posthashtagrepository.AddAsync(newposthashtag, cancellationToken);
                        }
                    }
                }
            }
        }

        public async Task<object?> LoadContentAsync(CancellationToken cancellationToken, params int[] ids)
        {
            object result;
            foreach (var id in ids)
            {
                var post = await postepository.GetByIdAsync(cancellationToken, id);
                var photo = await GetPhotoAsync(post.Id, cancellationToken);
                var video = await GetVideoAsync(post.Id, cancellationToken);
                if (photo is not null)
                {
                    result = photo;
                    return result as Photo;
                }
                else if (video is not null)
                {
                    result = video;
                    return result as Video;
                }
            }
            //one image that's show "This item is not exist!"
            return await photorepository.TableNoTracking.Where(x => x.Id == 1).SingleAsync() as Photo;
        }

        public async Task ForwardAsync(int id, int userreceiverid, CancellationToken cancellationToken)
        {
            var post = await postepository.GetByIdAsync(cancellationToken, id);
            await creationdirectorepository.CraetionDateAsync(new Direct() { Text = post.Text, PostId = post.Id, UserCraetionId = post.UserId/*, UserSenderId*/, UserReceiverId = userreceiverid, Post = post }, cancellationToken);
        }

        public async Task<string> GetDescribtionAsync(int id, CancellationToken cancellationToken)
        {
            var post = await postepository.GetByIdAsync(cancellationToken, id);
            return $"UserId: {post.UserCraetionId} - PostId: {post.Id}";
        }
    }
}

