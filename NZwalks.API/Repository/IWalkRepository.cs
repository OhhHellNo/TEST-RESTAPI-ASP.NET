using NZwalks.API.Models.Domains;

namespace NZwalks.API.Repository
{
    public interface IWalkRepository
    {

        Task<Walk> CreateWalk(Walk walk);
        Task<List<Walk>> GetWalks(string? filterOn = null,
                                  string? filterQuery = null,
                                  string? SortBy = null, bool isAscending = true, int pageNum = 1, int PageSize = 1000);

        Task<Walk?> GetWalkbyid(Guid id);
        Task<Walk?> Updatewalk(Guid id, Walk walk);
        Task<Walk?> Deletewalkbyid(Guid id);

    }
}
