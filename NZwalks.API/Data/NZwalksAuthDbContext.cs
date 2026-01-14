using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace NZwalks.API.Data
{
    public class NZwalksAuthDbContext : IdentityDbContext
    {
        public NZwalksAuthDbContext(DbContextOptions<NZwalksAuthDbContext> options) : base(options)
        {
        }

        protected NZwalksAuthDbContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var readerRoleId = "45c7074b-5780-46ff-8bac-b96d637131c9";
            var writerRoleId = "9606d22c-d28c-4c7d-aa67-80f8da204662";
            base.OnModelCreating(builder);
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                 new IdentityRole
                {
                    Id =writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }

            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
