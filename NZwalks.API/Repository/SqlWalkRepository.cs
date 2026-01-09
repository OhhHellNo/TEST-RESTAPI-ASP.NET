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

        async Task<List<Walk>> IWalkRepository.GetWalks()
        {

            return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();

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
