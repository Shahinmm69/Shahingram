using Data.Contract;
using Data;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Services.Contract;

namespace Services.Services
{
    public class DirectServices : IDirectServices
    {
        protected readonly IRepository<DirectPhoto> directphotorepository;
        protected readonly IRepository<DirectVideo> directvideorepository;
        protected readonly IRepository<Photo> Photorepository;
        protected readonly IRepository<Video> Videorepository;
        protected readonly IRepository<Post> postrepository;
        protected readonly IRepository<Direct> directbaserepository;
        protected readonly IDeletionRepository<Photo> deletephotorepository;
        protected readonly IDeletionRepository<Video> deletevideorepository;
        protected readonly ICreationRepository<Photo> creationphotorepository;
        protected readonly ICreationRepository<Video> creationvideorepository;
        protected readonly ICreationRepository<Direct> creationdirectorepository;
        public DirectServices(IRepository<DirectPhoto> directphotorepository, IRepository<DirectVideo> directvideorepository, IRepository<Direct> directbaserepository
            , IRepository<Post> postrepository, ICreationRepository<Photo> creationphotorepository, ICreationRepository<Video> creationvideorepository
            , ICreationRepository<Direct> creationdirectorepository, IRepository<Photo> photorepository, IRepository<Video> videorepository, IDeletionRepository<Photo> deletephotorepository
            , IDeletionRepository<Video> deletevideorepository)
        {
            this.directphotorepository = directphotorepository;
            this.directvideorepository = directvideorepository;
            Photorepository = photorepository;
            Videorepository = videorepository;
            this.directbaserepository = directbaserepository;
            this.postrepository = postrepository;
            this.creationphotorepository = creationphotorepository;
            this.creationvideorepository = creationvideorepository;
            this.creationdirectorepository = creationdirectorepository;
            this.deletephotorepository = deletephotorepository;
            this.deletevideorepository = deletevideorepository;
        }

        //public Task DeletionDateAsync(Direct entity, CancellationToken cancellationToken)
        //{
        //    entity.DeletionDate = DateTime.Now;

        //    compelete after athentication
        //    else if (entity.Id == entity.UserReceiverId)
        //        entity.ReceiverIsDeleted = true;
        //    if (entity.ReceiverIsDeleted == true && entity.SenderIsDeleted == true)
        //        entity.IsDeleted = true;
        //    return DeleteAsync(entity, cancellationToken);
        //}

        public async Task NewPhotoHandlerAsync(string address, int id, int userid, CancellationToken cancellationToken)
        {
            var newphoto = new Photo() { Address = address, UserCraetionId = userid, Describtion = await GetDescribtionAsync(id, cancellationToken) };
            await creationphotorepository.CraetionDateAsync(newphoto, cancellationToken);
            var newdirectphoto = new DirectPhoto() { DirectId = id, PhotoId = newphoto.Id };
            await directphotorepository.AddAsync(newdirectphoto, cancellationToken);
        }

        public async Task NewVideoHandlerAsync(string address, int id, int userid, CancellationToken cancellationToken)
        {
            var newvideo = new Video() { Address = address, UserCraetionId = userid, Describtion = await GetDescribtionAsync(id, cancellationToken) };
            await creationvideorepository.CraetionDateAsync(newvideo, cancellationToken);
            var newdirectvideo = new DirectVideo() { DirectId = id, VideoId = newvideo.Id };
            await directvideorepository.AddAsync(newdirectvideo, cancellationToken);
        }

        public async Task<Photo> GetPhotoAsync(int id, CancellationToken cancellationToken)
        {
            var directphoto = await directphotorepository.TableNoTracking.Where(x => x.DirectId == id).SingleAsync();
            var photo = directphoto.Photo;
            var direct = await directbaserepository.GetByIdAsync(cancellationToken, id);

            if (photo is not null)
            {
                if (direct.IsDeleted == true && photo.IsDeleted != true)
                {
                    photo.Describtion = photo.Describtion + ", Is Deleted";
                    await deletephotorepository.DeletionDateAsync(photo, cancellationToken);
                }

                //one image that's show "This item is not exist!"
                if (photo.IsDeleted == true)
                    return await Photorepository.TableNoTracking.Where(x => x.Id == 1).SingleAsync();
            }

            return photo;
        }

        public async Task<Video> GetVideoAsync(int id, CancellationToken cancellationToken)
        {
            var directvideo = await directvideorepository.TableNoTracking.Where(x => x.DirectId == id).SingleAsync();
            var video = directvideo.Video;
            var direct = await directbaserepository.GetByIdAsync(cancellationToken, id);

            if (video is not null)
            {
                if (direct.IsDeleted == true && video.IsDeleted != true)
                {
                    video.Describtion = video.Describtion + ", Is Deleted";
                    await deletevideorepository.DeletionDateAsync(video, cancellationToken);
                }

                //one image that's show "This item is not exist!"
                if (video.IsDeleted == true)
                    return await Videorepository.TableNoTracking.Where(x => x.Id == 1).SingleAsync();
            }

            return video;
        }

        public async Task<Post> GetPostAsync(int id, CancellationToken cancellationToken)
        {
            var direct = await directbaserepository.GetByIdAsync(cancellationToken, id);
            var post = await postrepository.GetByIdAsync(cancellationToken, direct.PostId);

            if (post is not null)
            {
                // one image that's show "This item is not exist!"
                if (post.IsDeleted == true)
                    return await postrepository.TableNoTracking.Where(x => x.Id == 1).SingleAsync();
            }

            return post;
        }

        public async Task<object?> LoadContentAsync(CancellationToken cancellationToken, params int[] ids)
        {
            object result;
            foreach (var id in ids)
            {
                var direct = await directbaserepository.GetByIdAsync(cancellationToken, id);
                var photo = await GetPhotoAsync(direct.Id, cancellationToken);
                var video = await GetVideoAsync(direct.Id, cancellationToken);
                var post = await GetPostAsync(direct.Id, cancellationToken);
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
                else if (post is not null)
                {
                    result = post;
                    return result as Post;
                }
            }
            return null;
        }

        public async Task ForwardAsync(int id, int userreceiverid, CancellationToken cancellationToken)
        {
            var direct = await directbaserepository.GetByIdAsync(cancellationToken, id);
            await creationdirectorepository.CraetionDateAsync(new Direct() { Text = direct.Text, PostId = direct.PostId, UserCraetionId = direct.UserCraetionId, UserSenderId = direct.UserReceiverId, UserReceiverId = userreceiverid, DirectPhotos = direct.DirectPhotos, DirectVideos = direct.DirectVideos, Post = direct.Post, User = direct.User }, cancellationToken);
        }

        public async Task<string> GetDescribtionAsync(int id, CancellationToken cancellationToken)
        {
            var direct = await directbaserepository.GetByIdAsync(cancellationToken, id);
            return $"UserId: {direct.UserCraetionId} - DirectId: {direct.Id}";
        }
    }
}
