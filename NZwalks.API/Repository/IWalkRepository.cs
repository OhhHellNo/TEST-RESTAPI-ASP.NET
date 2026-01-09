using NZwalks.API.Models.Domains;

namespace NZwalks.API.Repository
{
    public interface IWalkRepository
    {

        Task<Walk> CreateWalk(Walk walk);
        Task<List<Walk>> GetWalks();

        Task<Walk?> GetWalkbyid(Guid id);
        Task<Walk?> Updatewalk(Guid id, Walk walk);
        Task<Walk?> Deletewalkbyid(Guid id);
    }
}
