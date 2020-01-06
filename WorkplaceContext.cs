using Microsoft.EntityFrameworkCore;
using Workplace1c.Distribution1c;

namespace Workplace1c
{
    class WorkplaceContext : DbContext
    {
        public DbSet<Base> Bases { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Release> Releases { get; set; }
        public DbSet<Distribution1c.Distribution> Distributions { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=workplace.db");
    }
}
