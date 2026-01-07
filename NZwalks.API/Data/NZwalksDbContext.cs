using Microsoft.EntityFrameworkCore;
using NZwalks.API.Models.Domains;

namespace NZwalks.API.Data
{
    public class NZwalksDbContext : DbContext
    {

        public NZwalksDbContext(DbContextOptions DbContextOptions) : base(DbContextOptions)
        {

        }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
    }
}
