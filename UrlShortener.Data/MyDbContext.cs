using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Core.Enums;
using UrlShortener.Core.Models;
using UrlShortener.Core.Models.Identity;

namespace UrlShortener.Data
{
    public class MyDbContext : IdentityDbContext<User, Role, Guid>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<URL> URLs { get; set; }
    }
}
