using WebAPIApp.Models.Domain;

namespace WebAPIApp.Repositories
{
    public interface IUserRepository
    {
        Task<User> AuthenticateAsync(string username, string password);
    }
}
