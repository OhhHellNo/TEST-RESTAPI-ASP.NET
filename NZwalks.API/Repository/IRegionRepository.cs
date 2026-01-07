using NZwalks.API.Models.Domains;

namespace NZwalks.API.Repository
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetbyIdAsync(Guid id);
        Task<Region> CreateAsync(Region region);
        Task<Region?> UpdateAsync(Guid id, Region region);
        Task<Region?> DeleteByIdAsync(Guid id);
        object GetbyIdAsync(Func<object, bool> value);
    }
}
