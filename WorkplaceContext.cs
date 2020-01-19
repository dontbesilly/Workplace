using System;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using Workplace1c.Distribution1c;

namespace Workplace1c
{
    sealed class WorkplaceContext : DbContext
    {
        public TelegramSetting TelegramSetting;

        public DbSet<Base> Bases { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Release> Releases { get; set; }
        public DbSet<Distribution> Distributions { get; set; }
        public DbSet<DistributionAction> DistributionActions { get; set; }
        public DbSet<ChatId> ChatIds { get; set; }
        public DbSet<TelegramBot> TelegramBots { get; set; }
        public DbSet<TelegramSetting> TelegramSettings { get; set; }

        public WorkplaceContext()
        {
            this.Database.EnsureCreated();

            this.Bases.Load();
            this.Platforms.Load();
            this.Releases.Load();
            this.Distributions.Load();
            this.ChatIds.Load();
            this.DistributionActions.Load();
            this.TelegramBots.Load();
            this.TelegramSettings.Load();
            if (TelegramSettings.CountAsync().Result == 0)
            {
                var ts = new TelegramSetting();
                this.TelegramSettings.Add(ts);
                this.SaveChanges();
                TelegramSetting = ts;
            }
            else
            {
                TelegramSetting = this.TelegramSettings.FirstAsync().Result;
            }

        }

        public void AddEntity<T>(T entity)
        {
            this.Add(entity);
            this.SaveChanges();
        }

        public void RemoveEntity<T>(T entity)
        {
            this.Remove(entity);
            this.SaveChanges();
        }

        public void UpdateEntity<T>(T entity)
        {
            this.Update(entity);
            this.SaveChanges();
        }

        public ObservableCollection<Base> GetBasesLocal() => this.Bases.Local.ToObservableCollection();
        public ObservableCollection<Platform> GetPlatformsLocal() => this.Platforms.Local.ToObservableCollection();
        public ObservableCollection<Distribution> GetDistributionsLocal() => this.Distributions.Local.ToObservableCollection();
        public ObservableCollection<DistributionAction> GetDistributionActionsLocal() => this.DistributionActions.Local.ToObservableCollection();
        public ObservableCollection<TelegramBot> GetTelegramBotsLocal() => this.TelegramBots.Local.ToObservableCollection();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=workplace.db");
    }
}
