using Entities.Models;

namespace Services.Contract
{
    public interface IHashtagServices
    {
        IAsyncEnumerable<object?> Search(Hashtag hashtag, CancellationToken cancellationToken);
    }
}