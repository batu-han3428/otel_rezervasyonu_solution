using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using otel_rezervasyonu.Models;

namespace otel_rezervasyonu.Identity
{
    public class CustomIdentityDbContext : IdentityDbContext<CustomUser>
    {
        public CustomIdentityDbContext(DbContextOptions<CustomIdentityDbContext> options) : base(options)
        {
            
        }

        public DbSet<Rooms> Rooms { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Rooms>().HasData(
                new Rooms { no = 101, numberofbeds = 2, situation = false, price = 500 },
                new Rooms { no = 102, numberofbeds = 2, situation = false, price = 500 },
                new Rooms { no = 103, numberofbeds = 2, situation = false, price = 500 },
                new Rooms { no = 104, numberofbeds = 2, situation = false, price = 500 },
                new Rooms { no = 105, numberofbeds = 2, situation = false, price = 500 },
                new Rooms { no = 201, numberofbeds = 2, situation = false, price = 600 },
                new Rooms { no = 202, numberofbeds = 2, situation = false, price = 600 },
                new Rooms { no = 203, numberofbeds = 2, situation = false, price = 600 },
                new Rooms { no = 204, numberofbeds = 2, situation = false, price = 600 },
                new Rooms { no = 205, numberofbeds = 2, situation = false, price = 600 }
            );
        }
    }
}
