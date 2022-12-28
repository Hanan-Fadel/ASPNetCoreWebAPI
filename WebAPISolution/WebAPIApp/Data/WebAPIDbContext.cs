using Microsoft.EntityFrameworkCore;
using WebAPIApp.Models.Domain;

namespace WebAPIApp.Data
{
    public class WebAPIDbContext : DbContext
    {
        public WebAPIDbContext(DbContextOptions<WebAPIDbContext> options) : base(options)
        {

        }

        //Create the tables in the database if they are not exist

        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }
    }
}
