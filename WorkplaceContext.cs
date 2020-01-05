using Microsoft.EntityFrameworkCore;

namespace Workplace1c
{
    class WorkplaceContext : DbContext
    {
        public DbSet<Base> Bases { get; set; }
        public DbSet<Platform> Platforms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=workplace.db");
    }
}
