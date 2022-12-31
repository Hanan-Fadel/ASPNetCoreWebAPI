using WebAPIApp.Models.Domain;

namespace WebAPIApp.Repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllRegionsAsync();
    }
}
