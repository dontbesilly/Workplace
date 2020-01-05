using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace Workplace1c.VewModels
{
    class PlatformViewModel : INotifyPropertyChanged
    {
        private WorkplaceContext db;

        public ObservableCollection<Platform> Platforms { get; set; }

        public PlatformViewModel(WorkplaceContext db)
        {
            db.Platforms.Load();
            Platforms = db.Platforms.Local.ToObservableCollection();
            this.db = db;
            CheckPlatforms();
        }

        private Platform selectedPlatform;
        public Platform SelectedPlatform
        {
            get => selectedPlatform;
            set
            {
                selectedPlatform = value;
                OnPropertyChanged(nameof(SelectedPlatform));
            }
        }

        private RelayCommand addPlatformCommand;
        public RelayCommand AddPlatformCommand
        {
            get
            {
                return addPlatformCommand ??= new RelayCommand(obj =>
                {
                    var platfrorm1c = new Platform
                    {
                        Name = "8.3.15.1700"
                    };
                    db.Platforms.Add(platfrorm1c);
                    db.SaveChanges();
                });
            }
        }

        private RelayCommand deletePlatformCommand;
        public RelayCommand DeletePlatformCommand
        {
            get
            {
                return deletePlatformCommand ??= new RelayCommand(obj =>
                {
                    if (selectedPlatform is null) return;
                    db.Platforms.Remove(selectedPlatform);
                    db.SaveChanges();
                });
            }
        }

        private RelayCommand savePlatformsCommand;
        public RelayCommand SavePlatformsCommand
        {
            get
            {
                return savePlatformsCommand ??= new RelayCommand(obj =>
                {
                    if (selectedPlatform is null) return;
                    db.Platforms.Update(selectedPlatform);
                    db.SaveChanges();
                });
            }
        }

        private RelayCommand checkPlatformsCommand;
        public RelayCommand CheckPlatformsCommand
        {
            get
            {
                return checkPlatformsCommand ??= new RelayCommand(obj => { CheckPlatforms(); });
            }
        }

        private void CheckPlatforms()
        {
            foreach (Platform platform in Platforms)
            {
                platform.Exist = File.Exists(platform.FullPath);
                db.Platforms.Update(platform);
            }
            db.SaveChanges();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
