using Microsoft.AspNetCore.Identity;

namespace NZwalks.API.Repository
{
    public interface ItokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
