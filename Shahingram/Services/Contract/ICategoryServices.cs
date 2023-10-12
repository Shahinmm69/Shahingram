using Entities.Models;

namespace Services.Contract
{
    public interface ICategoryServices
    {
        Task CraetionConfigAsync(Category entity, CancellationToken cancellationToken);
    }
}