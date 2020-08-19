using Microsoft.EntityFrameworkCore;

namespace SimpleCrm.SqlDbServices
{
    public class SimpleCrmDbContext : DbContext
    {
        public SimpleCrmDbContext(DbContextOptions<SimpleCrmDbContext> options)
          : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
