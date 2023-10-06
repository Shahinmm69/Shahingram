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
    public class Hashtagrepository : Repository<Hashtag>, IHashtagrepository
    {
        protected readonly ApplicationDbContext DbContext;
        protected readonly IRepository<Post> postrepository;
        protected readonly IRepository<Comment> commentrepository;
        public Hashtagrepository(ApplicationDbContext dbContext, IRepository<Post> postrepository, IRepository<Comment> commentrepository)
            : base(dbContext)
        {
            DbContext = dbContext;
            this.postrepository = postrepository;
            this.commentrepository = commentrepository;
        }

        public Task DeletionDateAsync(Hashtag entity, CancellationToken cancellationToken)
        {
            entity.DeletionDate = DateTime.Now;
            entity.IsDeleted = true;
            return DeleteAsync(entity, cancellationToken);
        }

        public IEnumerable<object?> Search(Hashtag hashtag, CancellationToken cancellationToken)
        {
            object? result;
            if (hashtag.IsDeleted == false)
            {
                var posthashtag = DbContext.Set<PostHashtag>()
                    .FindAsync(hashtag.Id).Result;
                var commenthashtag = DbContext.Set<CommentHashtag>()
                    .FindAsync(hashtag.Id).Result;
                if (posthashtag != null)
                {
                    result = postrepository.GetByIdAsync(cancellationToken, posthashtag.Id);
                    yield return new[] { result };
                }
                if (commenthashtag != null)
                {
                    result = commentrepository.GetByIdAsync(cancellationToken, commenthashtag.Id);
                    yield return new[] { result };
                }
                yield return Task.CompletedTask;
            }
        }
    }
}
