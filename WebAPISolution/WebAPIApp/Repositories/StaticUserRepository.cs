using WebAPIApp.Models.Domain;

namespace WebAPIApp.Repositories
{
    public class StaticUserRepository : IUserRepository
    {
        //Create list of users
        private List<User> Users = new List<User>()
        {
           /* new User()
            {
                FirstName = "Read Only", LastName = "User", EmailAddress = "readonly@user.com",
                Id = Guid.NewGuid(), Username="readonly@user.com", Password="readonly@user.com",
                Roles=new List<string> {"reader"}
            },
             new User()
            {
                FirstName = "Read Write", LastName = "User", EmailAddress = "readwrite@user.com",
                Id = Guid.NewGuid(), Username="readwrite@user.com", Password="readwrite@user.com",
                Roles=new List<string> {"reader", "writer"}
            }*/
        };

        public async Task<User> AuthenticateAsync(string username, string password)
        {
           var user = Users.Find(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) && x.Password == password);
            return user;
        }
    }
}
