using Entities.Models;

namespace Data.Contract
{
    public interface IHashtagrepository
    {
        Task DeletionDateAsync(Hashtag entity, CancellationToken cancellationToken);
        IEnumerable<object?> Search(Hashtag hashtag, CancellationToken cancellationToken);
    }
}