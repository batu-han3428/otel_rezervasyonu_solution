using Microsoft.AspNetCore.Identity;
using otel_rezervasyonu.Models;

namespace otel_rezervasyonu.Identity
{
    public class CustomUser:IdentityUser
    {
        public string Tc { get; set; }
    }
}
