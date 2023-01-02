using WebAPIApp.Models.Domain;

namespace WebAPIApp.Repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
        
        Task<Region> GetAsync(Guid Id);
        Task<Region> AddAsync(Region region);

        Task<Region> DeleteAsync(Guid Id);

        Task<Region> UpdateAsync(Guid Id, Region region);
    }
}
