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
using Common;
using Common.Exceptions;

namespace Services.Services
{
    public class DirectServices : IScopedDependency, IDirectServices
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
        protected readonly IModificationRepository<Direct> modificationdirectRepository;
        public DirectServices(IRepository<DirectPhoto> directphotorepository, IRepository<DirectVideo> directvideorepository, IRepository<Direct> directbaserepository
            , IRepository<Post> postrepository, ICreationRepository<Photo> creationphotorepository, ICreationRepository<Video> creationvideorepository
            , ICreationRepository<Direct> creationdirectorepository, IRepository<Photo> photorepository, IRepository<Video> videorepository
            , IDeletionRepository<Photo> deletephotorepository, IDeletionRepository<Video> deletevideorepository, IModificationRepository<Direct> modificationdirectRepository)
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
            this.modificationdirectRepository = modificationdirectRepository;
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
            var direct = await directbaserepository.GetByIdAsync(cancellationToken, id);
            if (direct.DirectVideos == null && direct.Post == null)
            {
                var newphoto = new Photo() { Address = address, UserCreationId = userid, Describtion = await GetDescribtionAsync(id, cancellationToken) };
                await creationphotorepository.CraetionDateAsync(newphoto, cancellationToken);
                var newdirectphoto = new DirectPhoto() { DirectId = id, PhotoId = newphoto.Id };
                await directphotorepository.AddAsync(newdirectphoto, cancellationToken);
            }
        }

        public async Task NewVideoHandlerAsync(string address, int id, int userid, CancellationToken cancellationToken)
        {
            var direct = await directbaserepository.GetByIdAsync(cancellationToken, id);
            if (direct.DirectPhotos == null && direct.Post == null)
            {
                var newvideo = new Video() { Address = address, UserCreationId = userid, Describtion = await GetDescribtionAsync(id, cancellationToken) };
                await creationvideorepository.CraetionDateAsync(newvideo, cancellationToken);
                var newdirectvideo = new DirectVideo() { DirectId = id, VideoId = newvideo.Id };
                await directvideorepository.AddAsync(newdirectvideo, cancellationToken);
            }
        }

        public async Task<Direct> GetPhotoAsync(int id, CancellationToken cancellationToken)
        {
            var directphotos = await directphotorepository.TableNoTracking.Where(x => x.DirectId == id).Include(x => x.Photo).ToListAsync();
            var direct = await directbaserepository.TableNoTracking.Where(x => x.Id == id).Include(x => directphotos).SingleAsync();

            if (directphotos is not null)
            {
                foreach (var directphoto in directphotos)
                    if (direct.IsDeleted == true && directphoto.Photo.IsDeleted != true)
                    {
                        directphoto.Photo.Describtion = directphoto.Photo.Describtion + ", Is Deleted";
                        await deletephotorepository.DeletionDateAsync(directphoto.Photo, cancellationToken);
                    }

                return direct;
            }

            return null;

        }

        public async Task<Direct> GetVideoAsync(int id, CancellationToken cancellationToken)
        {
            var directvideos = await directvideorepository.TableNoTracking.Where(x => x.DirectId == id).Include(x => x.Video).ToListAsync();
            var direct = await directbaserepository.TableNoTracking.Where(x => x.Id == id).Include(x => directvideos).SingleAsync();

            if (directvideos is not null)
            {
                foreach (var directvideo in directvideos)
                    if (direct.IsDeleted == true && directvideo.Video.IsDeleted != true)
                    {
                        directvideo.Video.Describtion = directvideo.Video.Describtion + ", Is Deleted";
                        await deletevideorepository.DeletionDateAsync(directvideo.Video, cancellationToken);
                    }

                return direct;
            }

            return null;
        }

        public async Task<Direct> GetPostAsync(int id, CancellationToken cancellationToken)
        {
            var direct = await directbaserepository.TableNoTracking.Where(x => x.Id == id).Include(x => x.Post).SingleAsync();
            var post = direct.Post;

            if (post is not null)
            {
                // one image that's show "This item is not exist!"
                if (post.IsDeleted == true)
                {
                    direct.PostId = 1;
                    _ = modificationdirectRepository.UpdatModificationDateAsync(direct, cancellationToken);
                }

                return await directbaserepository.TableNoTracking
                    .Where(x => x.Id == id)
                    .Include(x => x.Post)
                    .SingleAsync();
            }

            return null;
        }

        public async Task<List<Direct>?> LoadContentAsync(CancellationToken cancellationToken, int pageNumer, int pageSize, params int[] ids)
        {
            var index = (pageNumer - 1) * pageSize;
            var result = new List<Direct>();
            for (int i = index; i <= index + pageSize; i++)
            {
                var direct = await directbaserepository.GetByIdAsync(cancellationToken, ids[i]);
                var photo = await GetPhotoAsync(ids[i], cancellationToken);
                var video = await GetVideoAsync(ids[i], cancellationToken);
                var post = await GetPostAsync(ids[i], cancellationToken);
                if (photo is not null)
                    result.Add(photo);
                else if (video is not null)
                    result.Add(video);
                else if (post is not null)
                    result.Add(post);
                else
                    result.Add(direct);
            }
            return result;
        }

        public async Task ForwardAsync(int id, int userreceiverid, CancellationToken cancellationToken)
        {
            var direct = await directbaserepository.GetByIdAsync(cancellationToken, id);
            direct.UserCreationId = direct.UserReceiverId;
            direct.UserReceiverId = userreceiverid;
            await creationdirectorepository.CraetionDateAsync(direct, cancellationToken);
        }

        public async Task<string> GetDescribtionAsync(int id, CancellationToken cancellationToken)
        {
            var direct = await directbaserepository.GetByIdAsync(cancellationToken, id);
            return $"UserId: {direct.UserCreationId} - DirectId: {direct.Id}";
        }
    }
}
