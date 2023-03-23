using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Interfaces;
using UrlShortener.Core.Models.Identity;
using UrlShortener.Data;
using UrlShortener.Data.Implementations;
using UrlShortener.Services.Implementations;
using UrlShortener.Services.Interfaces;

namespace UrlShortener.Web.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUrlService, UrlService>();
            services.AddDbContext<MyDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services
                .AddIdentity<User, Role>()
                .AddEntityFrameworkStores<MyDbContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<User, Role, MyDbContext, Guid>>()
                .AddRoleStore<RoleStore<Role, MyDbContext, Guid>>();
            services.AddAuthorization();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Login/Login";
            });
            return services;
        }
    }
}
