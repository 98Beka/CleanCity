using CleanCity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanCity.Data
{
    public class DataContext : IdentityDbContext<IdentityUser, IdentityRole, string> {

        public DbSet<PointOnTheMap> PointOnTheMaps { get; set; }
        public DbSet<Photo> Photos { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options) {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        public DataContext() {
        }
    }
}
