using Microsoft.EntityFrameworkCore;
using WebAPIApp.Data;
using WebAPIApp.Models.Domain;

namespace WebAPIApp.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly WebAPIDbContext webAPIDbContext;

        //use the constructor injection to inject the dbcontext inside the constructor to be able to access the db
        public WalkRepository(WebAPIDbContext webAPIDbContext)
        {
            this.webAPIDbContext = webAPIDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            // Assign new ID
            Guid id = Guid.NewGuid();
            await webAPIDbContext.Walks.AddAsync(walk);
            await webAPIDbContext.SaveChangesAsync();

            return walk; 
            
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await webAPIDbContext.Walks.FindAsync(id);

            if (existingWalk == null)
            {
                return null;
            }
            webAPIDbContext.Walks.Remove(existingWalk);

            await webAPIDbContext.SaveChangesAsync();

            return existingWalk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            //Fetch all walks from the database
            //and also include all itse related regions and walkdifficulty (navigation properties associated to this Walk)
           return await
                webAPIDbContext.Walks
                .Include(x=> x.Region)
                .Include(x=> x.WalkDifficulty)
                .ToListAsync();
           
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            return await webAPIDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await webAPIDbContext.Walks.FindAsync(id);
            if (existingWalk != null)
            {
                //update the Walk 
                existingWalk.Length = walk.Length;
                existingWalk.Name = walk.Name;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                existingWalk.RegionId = walk.RegionId;

                await webAPIDbContext.SaveChangesAsync();
                return existingWalk;
            }

            return null;
        }
    }
}
