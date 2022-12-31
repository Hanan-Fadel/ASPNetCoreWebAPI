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
        public async Task<IEnumerable<Region>> GetAllRegionsAsync()
        {
            return await webAPIDbContext.Regions.ToListAsync();
        }

        /* public IEnumerable<Region> GetAllRegions()
         {
             return webAPIDbContext.Regions.ToList();
         }*/
    }
}
