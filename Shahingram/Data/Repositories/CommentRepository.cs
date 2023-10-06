using Data.Common;
using Data.Contract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommon<Comment>, ICommentRepository
    {
        protected readonly ApplicationDbContext DbContext;
        protected readonly IRepository<CommentHashtag> commenthashtagrepository;
        protected readonly IRepository<Hashtag> hashtagrepository;
        public CommentRepository(ApplicationDbContext dbContext, IRepository<CommentHashtag> commenthashtagrepository, IRepository<Hashtag> hashtagrepository)
            : base(dbContext)
        {
            DbContext = dbContext;
            this.commenthashtagrepository = commenthashtagrepository;
            this.hashtagrepository = hashtagrepository;
        }

        public Task CraetionDateAsync(Comment entity, CancellationToken cancellationToken)
        {
            entity.CrationDate = DateTime.Now;
            return AddAsync(entity, cancellationToken);
        }

        public Task DeletionDateAsync(Comment entity, CancellationToken cancellationToken)
        {
            entity.DeletionDate = DateTime.Now;
            entity.IsDeleted = true;
            return DeleteAsync(entity, cancellationToken);
        }

        public Task UpdatModificationDateAsync(Comment entity, CancellationToken cancellationToken)
        {
            entity.ModificationDate = DateTime.Now;
            return UpdateAsync(entity, cancellationToken);
        }

        public Task<List<Comment>> GetReplies(int id, CancellationToken cancellationToken)
        {
            var replies = DbContext.Set<Comment>()
                .AsNoTracking()
                .Where(u => u.Id == id)
                .Include(u => u.Children)
                .Where(u => u.IsDeleted != false)
                .ToListAsync();
            return replies;
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
                        var newcommenthashtag = new CommentHashtag() { CommentId = id, HashtagId = hashtag.Id };
                        commenthashtagrepository.AddAsync(newcommenthashtag, cancellationToken);
                    }
                    else
                    {
                        var newhashtag = new Hashtag() { Address = hashtags[i], CrationDate = DateTime.Now, UserCraetionId = base.GetById(id, cancellationToken).UserCraetionId };
                        hashtagrepository.AddAsync(newhashtag, cancellationToken);
                        var newcommenthashtag = new CommentHashtag() { CommentId = id, HashtagId = newhashtag.Id };
                        commenthashtagrepository.AddAsync(newcommenthashtag, cancellationToken);
                    }
                }
            }
            return Task.CompletedTask;
        }
    }

}
