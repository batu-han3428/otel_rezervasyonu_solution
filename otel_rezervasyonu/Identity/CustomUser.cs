using Microsoft.AspNetCore.Identity;

namespace otel_rezervasyonu.Identity
{
    public class CustomUser:IdentityUser
    {
        public string Tc { get; set; }
    }
}
