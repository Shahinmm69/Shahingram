using Data.Contract;
using Data;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Services.Contract;
using Common;

namespace Services.Services
{
    public class CommentServices : ICommentServices, IScopedDependency
    {
        protected readonly IRepository<CommentHashtag> commenthashtagrepository;
        protected readonly IRepository<Hashtag> hashtagrepository;
        protected readonly IRepository<Comment> commentbaserepository;
        protected readonly ICreationRepository<Hashtag> creationhashtagrepository;
        public CommentServices(IRepository<CommentHashtag> commenthashtagrepository, IRepository<Hashtag> hashtagrepository, IRepository<Comment> commentbaserepository
            , ICreationRepository<Hashtag> creationhashtagrepository)
        {
            this.commenthashtagrepository = commenthashtagrepository;
            this.hashtagrepository = hashtagrepository;
            this.commentbaserepository = commentbaserepository;
            this.creationhashtagrepository = creationhashtagrepository;
        }

        public async IAsyncEnumerable<Comment> GetReplies(int id, CancellationToken cancellationToken)
        {
            var comment = await commentbaserepository.GetByIdAsync(cancellationToken, id);
            var children = comment.Children;
            var replies = children.Where(x => x.IsDeleted == false);
            foreach (var reply in replies)
                yield return reply;
        }

        public async Task HashtagsHandler(int id, CancellationToken cancellationToken)
        {
            int index;
            var comment = await commentbaserepository.GetByIdAsync(cancellationToken, id);
            string text = comment.Text;
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
                        var newcommenthashtag = new CommentHashtag() { CommentId = id, HashtagId = hashtag.Id };
                        await commenthashtagrepository.AddAsync(newcommenthashtag, cancellationToken);
                    }
                    else
                    {
                        var newhashtag = new Hashtag() { Title = hashtags[i], UserCraetionId = comment.UserCraetionId };
                        await creationhashtagrepository.CraetionDateAsync(newhashtag, cancellationToken);
                        var newcommenthashtag = new CommentHashtag() { CommentId = id, HashtagId = newhashtag.Id };
                        await commenthashtagrepository.AddAsync(newcommenthashtag, cancellationToken);
                    }
                }
            }
        }
    }
}
