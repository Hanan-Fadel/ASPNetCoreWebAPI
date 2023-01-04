using Microsoft.EntityFrameworkCore;
using WebAPIApp.Data;
using WebAPIApp.Models.Domain;

namespace WebAPIApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly WebAPIDbContext webAPIDbContext;

        public UserRepository(WebAPIDbContext webAPIDbContext)
        {
            this.webAPIDbContext = webAPIDbContext;
        }
        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await webAPIDbContext.Users.FirstOrDefaultAsync(
                x => x.Username.ToLower() == username.ToLower() && x.Password == password);

            if (user == null)
            {
                return null;
            }

            //Add the roles of the user from roles tanles
            var userRoles = await webAPIDbContext.Users_Roles.Where(x=> x.UserId == user.Id).ToListAsync();

            if (userRoles.Any())
            {
                user.Roles = new List<string>();

                foreach (var userRole in userRoles)
                {
                    var role = await webAPIDbContext.Roles.FirstOrDefaultAsync(x=> x.Id == userRole.RoleId);

                    if(role != null)
                    {
                        user.Roles.Add(role.Name);
                    }
                }
            }

            user.Password = null;
            return user;
        }
    }
}
