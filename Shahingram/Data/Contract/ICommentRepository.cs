using Entities.Models;

namespace Data.Contract
{
    public interface ICommentRepository
    {
        Task CraetionDateAsync(Comment entity, CancellationToken cancellationToken);
        Task DeletionDateAsync(Comment entity, CancellationToken cancellationToken);
        Task<List<Comment>> GetReplies(int id, CancellationToken cancellationToken);
        Task HashtagsHandler(int id, CancellationToken cancellationToken);
        Task UpdatModificationDateAsync(Comment entity, CancellationToken cancellationToken);
    }
}