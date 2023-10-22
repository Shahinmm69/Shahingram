using Entities.Models;

namespace Services.Contract
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user);
    }
}