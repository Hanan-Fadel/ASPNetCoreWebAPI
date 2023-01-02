using Microsoft.EntityFrameworkCore;
using WebAPIApp.Data;
using WebAPIApp.Models.Domain;

namespace WebAPIApp.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        //We will use this private property inside the method GetAllRegions to get all the regions
        private readonly WebAPIDbContext webAPIDbContext;

        //since we already injected the dbcontext inside the Services collection, we can Inject it here through the constructor
        public RegionRepository(WebAPIDbContext webAPIDbContext)
        {
            this.webAPIDbContext = webAPIDbContext;
        }

        //Use async 
        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await webAPIDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid Id)
        {
            return await webAPIDbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await webAPIDbContext.AddAsync(region);
            await webAPIDbContext.SaveChangesAsync();
            return region;
        }


        public async Task<Region> DeleteAsync(Guid Id)
        {
            var region = await webAPIDbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);

            if (region == null)
            {
                return null;
            }

            //Delete the region from the database
            webAPIDbContext.Regions.Remove(region);

            //Save the changes to the database
            await webAPIDbContext.SaveChangesAsync();

            return region;
        }

        public async Task<Region> UpdateAsync(Guid Id, Region region)
        {
            var existingRegion = await webAPIDbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id );

            if (existingRegion == null)
            {
                return null;
            }

            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.Area = region.Area;
            existingRegion.Lat = region.Lat;
            existingRegion.Long= region.Long;
            existingRegion.Population = region.Population;

            await webAPIDbContext.SaveChangesAsync();

            return existingRegion;

        }
    }
}
