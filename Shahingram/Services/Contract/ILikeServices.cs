using Entities.Models;

namespace Services.Contract
{
    public interface ILikeServices
    {
        Task CraetionConfigAsync(Like entity, CancellationToken cancellationToken);
    }
}