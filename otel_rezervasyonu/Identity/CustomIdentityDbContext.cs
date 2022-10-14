using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace otel_rezervasyonu.Identity
{
    public class CustomIdentityDbContext:IdentityDbContext<CustomUser>
    {
        public CustomIdentityDbContext(DbContextOptions<CustomIdentityDbContext> options):base(options)
        {

        }
    }
}
