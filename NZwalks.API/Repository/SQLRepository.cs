using Microsoft.EntityFrameworkCore;
using NZwalks.API.Data;
using NZwalks.API.Models.Domains;

namespace NZwalks.API.Repository
{
    public class SQLRepository : IRegionRepository
    {
        private readonly NZwalksDbContext dbContext;

        public SQLRepository(NZwalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return (region);

        }



        public async Task<Region?> DeleteByIdAsync(Guid id)
        {
            var tobedelted = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (tobedelted == null)
            {
                return null;
            }
            dbContext.Regions.Remove(tobedelted);
            await dbContext.SaveChangesAsync();
            return (tobedelted);
        }


        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetbyIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public object GetbyIdAsync(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var ExistingId = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (ExistingId == null)
            {
                return null;
            }

            ExistingId.RegionImageUrl = region.RegionImageUrl;
            ExistingId.Code = region.Code;
            ExistingId.Name = region.Name;
            await dbContext.SaveChangesAsync();
            return ExistingId;


        }


    }
}
