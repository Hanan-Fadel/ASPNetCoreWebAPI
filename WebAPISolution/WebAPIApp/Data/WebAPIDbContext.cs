using Microsoft.EntityFrameworkCore;
using WebAPIApp.Models.Domain;

namespace WebAPIApp.Data
{
    public class WebAPIDbContext : DbContext
    {
        public WebAPIDbContext(DbContextOptions<WebAPIDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User_Role>()
                .HasOne(x => x.Role)
                .WithMany(y => y.UserRoles)
                .HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<User_Role>()
            .HasOne(x => x.User)
            .WithMany(y => y.UserRoles)
            .HasForeignKey(x => x.UserId);
        }

        //Create the tables in the database if they are not exist

        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User_Role> Users_Roles { get; set; }
    }
}
