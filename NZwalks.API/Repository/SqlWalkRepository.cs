using Microsoft.EntityFrameworkCore;
using NZwalks.API.Data;
using NZwalks.API.Models.Domains;

namespace NZwalks.API.Repository
{
    public class SqlWalkRepository : IWalkRepository

    {
        private readonly NZwalksDbContext dbContext;

        public SqlWalkRepository(NZwalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Walk> CreateWalk(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;

        }

        public async Task<List<Walk>> GetWalks(string? filterOn = null, string? filterQuery = null, string? SortBy = null, bool isAscending = true, int pageNum = 1, int PageSize = 1000)
        {
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
            //filtering based on coloum and qurey 
            if (string.IsNullOrEmpty(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            //sorting. 
            if (string.IsNullOrEmpty(SortBy) == false)
            {
                if (SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (SortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            var PageSkips = (pageNum - 1) * PageSize;
            return await walks.Skip(PageSkips).Take(PageSize).ToListAsync();
        }

        async Task<Walk?> IWalkRepository.Deletewalkbyid(Guid id)
        {
            var tobedelted = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (tobedelted == null)
            {
                return null;
            }
            dbContext.Walks.Remove(tobedelted);
            await dbContext.SaveChangesAsync();
            return (tobedelted);
        }

        async Task<Walk> IWalkRepository.GetWalkbyid(Guid id)


        {
            return await dbContext.Walks.Include("Region").Include("Difficulty").FirstOrDefaultAsync(x => x.Id == id);
        }




        async Task<Walk?> IWalkRepository.Updatewalk(Guid id, Walk walk)
        {
            var existingwalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingwalk == null)
            {
                return null;
            }
            existingwalk.Name = walk.Name;
            existingwalk.Description = walk.Description;
            existingwalk.LengthInKm = walk.LengthInKm;
            existingwalk.WalkImageUrl = walk.WalkImageUrl;
            existingwalk.DifficultyId = walk.DifficultyId;
            existingwalk.RegionId = walk.RegionId;
            await dbContext.SaveChangesAsync();
            return existingwalk;
        }
    }
}
