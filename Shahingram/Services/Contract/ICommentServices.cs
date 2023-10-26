using Entities.Models;

namespace Services.Contract
{
    public interface ICommentServices
    {
        Task<List<Comment>>? GetReplies(int id, int pageNumer, int pageSize, CancellationToken cancellationToken);
        Task HashtagsHandler(int id, CancellationToken cancellationToken);
    }
}