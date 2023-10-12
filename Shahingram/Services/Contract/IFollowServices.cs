using Entities.Models;

namespace Services.Contract
{
    public interface IFollowServices
    {
        Task CraetionConfigAsync(Follow entity, CancellationToken cancellationToken);
    }
}