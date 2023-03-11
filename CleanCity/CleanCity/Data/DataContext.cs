using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanCity.Data
{
    public class DataContext : IdentityDbContext<IdentityUser, IdentityRole, string> {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }
        public DataContext() {
        }
    }
}
