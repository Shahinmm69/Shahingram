using Common;
using Data.Contract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Contract;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class HashtagServices : IScopedDependency, IHashtagServices
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

        public async Task<List<Post>?> SearchPosts(Hashtag hashtag, int pageNumer, int pageSize, CancellationToken cancellationToken)
        {
            if (hashtag.IsDeleted == false)
            {
                var posthashtags = await posthashtagrepository.TableNoTracking.Where(x => x.HashtagId == hashtag.Id).Include(x => x.Hashtag).ToListAsync(cancellationToken);
                return await postrepository.TableNoTracking.Include(x => posthashtags).Skip((pageNumer - 1) * pageSize).Take(pageSize).ToListAsync();
            }
            return null;
        }

        public async Task<List<Comment>?> SearchComments(Hashtag hashtag, int pageNumer, int pageSize, CancellationToken cancellationToken)
        {
            if (hashtag.IsDeleted == false)
            {
                var commenthashtags = await commenthashtagrepository.TableNoTracking.Where(x => x.HashtagId == hashtag.Id).Include(x => x.Hashtag).ToListAsync(cancellationToken);
                return await commentrepository.TableNoTracking.Include(x => commenthashtags).Skip((pageNumer - 1) * pageSize).Take(pageSize).ToListAsync();
            }
            return null;
        }
    }
}
