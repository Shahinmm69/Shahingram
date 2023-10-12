using Entities.Models;

namespace Services.Contract
{
    public interface ICountryServices
    {
        Task CraetionConfigAsync(Country entity, CancellationToken cancellationToken);
    }
}