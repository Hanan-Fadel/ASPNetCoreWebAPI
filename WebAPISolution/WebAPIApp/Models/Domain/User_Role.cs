namespace WebAPIApp.Models.Domain
{
    public class User_Role
    {
        public Guid Id { get; set; }

        //Navigation Properties for M-M between User & Role tables
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
