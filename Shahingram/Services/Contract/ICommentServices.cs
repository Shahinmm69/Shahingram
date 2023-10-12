using Entities.Models;

namespace Services.Contract
{
    public interface ICommentServices
    {
        IAsyncEnumerable<Comment> GetReplies(int id, CancellationToken cancellationToken);
        Task HashtagsHandler(int id, CancellationToken cancellationToken);
    }
}