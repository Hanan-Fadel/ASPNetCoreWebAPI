using WebAPIApp.Models.Domain;

namespace WebAPIApp.Repositories
{
    public interface ITokenHandler
    {
        Task<string> CreateTokenAsync(User user);
    }
}
