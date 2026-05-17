using ECommmerce.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace ECommmerce.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Define DbSet for your entities, e.g.:
        public DbSet<Product> Products => Set<Product>();
    }
}
