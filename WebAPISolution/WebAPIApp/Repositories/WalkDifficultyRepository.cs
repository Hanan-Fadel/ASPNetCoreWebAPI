using Microsoft.EntityFrameworkCore;
using WebAPIApp.Data;
using WebAPIApp.Models.Domain;

namespace WebAPIApp.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly WebAPIDbContext webAPIDbContext;

        public WalkDifficultyRepository(WebAPIDbContext webAPIDbContext)
        {
            this.webAPIDbContext = webAPIDbContext;
        }

        public async Task<List<WalkDifficulty>> GetAllAsync()
        {
            return await webAPIDbContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            var walkDifficultyDomain = await webAPIDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);

            if (walkDifficultyDomain == null)
            {
                return null;
            }
            return walkDifficultyDomain;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await webAPIDbContext.WalkDifficulty.AddAsync(walkDifficulty);
            await webAPIDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty = await webAPIDbContext.WalkDifficulty.FindAsync(id);

            if (existingWalkDifficulty == null)
            {
                return null;
            }

            //Update the walkDifficulty
            existingWalkDifficulty.Code = walkDifficulty.Code;

            //Save chages to db
            await webAPIDbContext.SaveChangesAsync();

            return existingWalkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingWalkDifficulty = await webAPIDbContext.WalkDifficulty.FindAsync(id);

            if (existingWalkDifficulty == null)
            {
                return null;
            }

            // delete the existing walk difficulty from db
            webAPIDbContext.WalkDifficulty.Remove(existingWalkDifficulty);
            await webAPIDbContext.SaveChangesAsync();

            return existingWalkDifficulty;
        }
    }
}
