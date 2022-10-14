using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using otel_rezervasyonu.Identity;

namespace otel_rezervasyonu.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection IdentityServerAyarlari(this IServiceCollection services)
        {
            services.AddDbContext<CustomIdentityDbContext>(opt => opt.UseSqlServer("Server=SQLTSTSRV;Database=Otel;User Id=bisuser;Password=p@ssword1"));


            services.AddIdentity<CustomUser, IdentityRole>().AddEntityFrameworkStores<CustomIdentityDbContext>().
            AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 2;
                options.Password.RequireNonAlphanumeric = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.User.AllowedUserNameCharacters = "abcçdefgğhıijklmnoöpqrsştuüvwxyzABCÇDEFGĞHIİJKLMNOÖPQRSTUÜVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            });

            return services;
        }

        public static IServiceCollection CookieAyarlari(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = "UyeCookie";
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(40);
            });

            return services;
        }
    }
}
