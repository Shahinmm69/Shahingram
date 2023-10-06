using Data.Common;
using Data.Contract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Owin.BuilderProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Data.Repositories
{
    public class DirectRepository : Repository<Direct>, ICommon<Direct>, IDirectRepository
    {
        protected readonly ApplicationDbContext DbContext;
        protected readonly IRepository<DirectPhoto> directphotorepository;
        protected readonly IRepository<DirectVideo> directvideorepository;
        protected readonly IRepository<Photo> photorepository;
        protected readonly IRepository<Video> videorepository;
        public DirectRepository(ApplicationDbContext dbContext, IRepository<DirectPhoto> directphotorepository, IRepository<DirectVideo> directvideorepository
            , IRepository<Photo> photorepository, IRepository<Video> videorepository)
            : base(dbContext)
        {
            DbContext = dbContext;
            this.directphotorepository = directphotorepository;
            this.directvideorepository = directvideorepository;
            this.photorepository = photorepository;
            this.videorepository = videorepository;
        }

        public Task CraetionDateAsync(Direct entity, CancellationToken cancellationToken)
        {
            entity.CrationDate = DateTime.Now;
            return AddAsync(entity, cancellationToken);
        }

        public Task DeletionDateAsync(Direct entity, CancellationToken cancellationToken)
        {
            entity.DeletionDate = DateTime.Now;
            var p = DbContext.Set<DirectPhoto>()
                .AsNoTracking()
                .Where(p => p.DirectId == entity.Id)
                .Include(p => p.Photo.IsDeleted != false).Single();
            var photo = photorepository.GetById(p.PhotoId);
            var v = DbContext.Set<DirectVideo>()
                .AsNoTracking()
                .Where(v => v.DirectId == entity.Id)
                .Include(v => v.Video.IsDeleted != false).Single();
            var video = videorepository.GetById(v.VideoId);
            if (photo != null)
            {
                photo.DeletionDate = DateTime.Now;
                photo.Describtion = photo.Describtion + ", Is Deleted";
                photo.IsDeleted = true;
                photorepository.DeleteAsync(photo, cancellationToken);
            }
            if (video != null)
            {
                video.DeletionDate = DateTime.Now;
                video.Describtion = video.Describtion + ", Is Deleted";
                video.IsDeleted = true;
                videorepository.DeleteAsync(video, cancellationToken);
            }
            // compelete after athentication
            //else if (entity.Id == entity.UserReceiverId)
            //    entity.ReceiverIsDeleted = true;
            //if (entity.ReceiverIsDeleted == true && entity.SenderIsDeleted == true)
            //    entity.IsDeleted = true;
            return DeleteAsync(entity, cancellationToken);
        }

        public Task UpdatModificationDateAsync(Direct entity, CancellationToken cancellationToken)
        {
            entity.ModificationDate = DateTime.Now;
            return UpdateAsync(entity, cancellationToken);
        }

        public Task NewPhotoHandler(string address, int id, int userid, CancellationToken cancellationToken)
        {
            var newphoto = new Photo() { Address = address, CrationDate = DateTime.Now, UserCraetionId = userid, Describtion = GetDescribtion(id, cancellationToken) };
            photorepository.AddAsync(newphoto, cancellationToken);
            var newdirectphoto = new DirectPhoto() { DirectId = id, PhotoId = newphoto.Id };
            directphotorepository.AddAsync(newdirectphoto, cancellationToken);
            return Task.CompletedTask;
        }

        public Task NewVideoHandler(string address, int id, int userid, CancellationToken cancellationToken)
        {
            var newvideo = new Video() { Address = address, CrationDate = DateTime.Now, UserCraetionId = userid, Describtion = GetDescribtion(id, cancellationToken) };
            videorepository.AddAsync(newvideo, cancellationToken);
            var newdirectvideo = new DirectVideo() { DirectId = id, VideoId = newvideo.Id };
            directvideorepository.AddAsync(newdirectvideo, cancellationToken);
            return Task.CompletedTask;
        }

        public Task<Photo> GetPhoto(int id, CancellationToken cancellationToken)
        {
            var u = DbContext.Set<DirectPhoto>()
                .AsNoTracking()
                .Where(u => u.DirectId == id)
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
            var u = DbContext.Set<DirectVideo>()
                .AsNoTracking()
                .Where(u => u.DirectId == id)
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

        public Task<Post> GetPost(int id, CancellationToken cancellationToken)
        {
            var direct = DbContext.Set<Direct>()
                .AsNoTracking()
                .Where(u => u.Id == id).SingleAsync();
            var post = DbContext.Set<Post>()
            .Where(u => u.Id == direct.Result.PostId).SingleAsync();

            // one image that's show "This item is not exist!"
            if (post.Result.IsDeleted == true)
                return DbContext.Set<Post>()
                .AsNoTracking()
                .Where(p => p.Id == 1)
                .SingleAsync();

            return post;
        }

        public async Task<object?> LoadContentAsync(CancellationToken cancellationToken, params int[] ids)
        {
            object result;
            foreach (var id in ids)
            {
                var direct = DbContext.Set<Direct>()
                    .AsNoTracking()
                    .Where(u => u.Id == id).SingleAsync();
                if (GetPhoto(direct.Id, cancellationToken) is not null)
                {
                    result = await GetPhoto(direct.Id, cancellationToken);
                    return result as Photo;
                }
                else if (GetVideo(direct.Id, cancellationToken) is not null)
                {
                    result = await GetVideo(direct.Id, cancellationToken);
                    return result as Video;
                }
                else if (GetPost(direct.Id, cancellationToken) is not null)
                {
                    result = await GetPost(direct.Id, cancellationToken);
                    return result as Post;
                }
            }
            return Task.CompletedTask;
        }

        public Task Forward(int id, int userreceiverid, CancellationToken cancellationToken)
        {
            var direct = base.GetById(id);
            _ = base.AddAsync(new Direct() { Text = direct.Text, CrationDate = DateTime.Now, PostId = direct.PostId, UserCraetionId = direct.UserCraetionId, UserSenderId = direct.UserReceiverId, UserReceiverId = userreceiverid, DirectPhotos = direct.DirectPhotos, DirectVideos = direct.DirectVideos, Post = direct.Post, User = direct.User }, cancellationToken);
            return Task.CompletedTask;
        }

        public string GetDescribtion(int id, CancellationToken cancellationToken)
        {
            var direct = base.GetById(id, cancellationToken);
            return $"UserId: {direct.UserCraetionId} - DirectId: {direct.Id}";
        }

    }
}
