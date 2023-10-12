using Data.Contract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class HashtagServices : IHashtagServices
    {
        protected readonly IRepository<Post> postrepository;
        protected readonly IRepository<Comment> commentrepository;
        protected readonly IRepository<PostHashtag> posthashtagrepository;
        protected readonly IRepository<CommentHashtag> commenthashtagrepository;
        public HashtagServices(IRepository<Post> postrepository, IRepository<Comment> commentrepository, IRepository<PostHashtag> posthashtagrepository
            , IRepository<CommentHashtag> commenthashtagrepository)
        {
            this.postrepository = postrepository;
            this.commentrepository = commentrepository;
            this.commenthashtagrepository = commenthashtagrepository;
            this.posthashtagrepository = posthashtagrepository;
        }

        public async IAsyncEnumerable<object?> Search(Hashtag hashtag, CancellationToken cancellationToken)
        {
            object? result;
            if (hashtag.IsDeleted == false)
            {
                var posthashtags = await posthashtagrepository.TableNoTracking.Where(x => x.HashtagId == hashtag.Id).ToListAsync();
                var commenthashtags = await commenthashtagrepository.TableNoTracking.Where(x => x.HashtagId == hashtag.Id).ToListAsync();
                if (posthashtags != null)
                {
                    foreach (var posthashtag in posthashtags)
                    {
                        result = await postrepository.GetByIdAsync(cancellationToken, posthashtag.PostId);
                        yield return result as PostHashtag;
                    }
                }
                if (commenthashtags != null)
                {
                    foreach (var commenthashtag in commenthashtags)
                    {
                        result = await commentrepository.GetByIdAsync(cancellationToken, commenthashtag.CommentId);
                        yield return result as CommentHashtag;
                    }
                }
            }
        }
    }
}
