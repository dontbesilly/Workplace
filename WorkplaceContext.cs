using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using Workplace1c.Distribution1c;

namespace Workplace1c
{
    class WorkplaceContext : DbContext
    {
        public DbSet<Base> Bases { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Release> Releases { get; set; }
        public DbSet<Distribution> Distributions { get; set; }
        public DbSet<DistributionAction> DistributionActions { get; set; }

        public WorkplaceContext()
        {
            this.Database.EnsureCreated();

            this.Bases.Load();
            this.Platforms.Load();
            this.Releases.Load();
            this.Distributions.Load();
            this.DistributionActions.Load();
        }

        public void AddDistribution(Distribution distr)
        {
            this.Distributions.Add(distr);
            this.SaveChanges();
        }

        public void AddRelease(Release release)
        {
            this.Releases.Add(release);
            this.SaveChanges();
        }

        public void RemoveRelease(Release release)
        {
            this.Releases.Remove(release);
            this.SaveChanges();
        }

        public ObservableCollection<Base> GetBasesLocal() => this.Bases.Local.ToObservableCollection();
        public ObservableCollection<Platform> GetPlatformsLocal() => this.Platforms.Local.ToObservableCollection();
        public ObservableCollection<Distribution> GetDistributionsLocal() => this.Distributions.Local.ToObservableCollection();
        public ObservableCollection<DistributionAction> GetDistributionActionsLocal() => this.DistributionActions.Local.ToObservableCollection();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=workplace.db");
    }
}
