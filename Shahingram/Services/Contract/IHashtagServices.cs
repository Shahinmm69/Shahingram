using Entities.Models;

namespace Services.Contract
{
    public interface IHashtagServices
    {
        Task<List<Comment>?> SearchComments(Hashtag hashtag, int pageNumer, int pageSize, CancellationToken cancellationToken);
        Task<List<Post>?> SearchPosts(Hashtag hashtag, int pageNumer, int pageSize, CancellationToken cancellationToken);
    }
}