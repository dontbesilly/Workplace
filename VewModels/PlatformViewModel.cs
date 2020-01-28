using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using File = System.IO.File;

namespace Workplace1c.VewModels
{
    class PlatformViewModel : INotifyPropertyChanged
    {
        private WorkplaceContext db;

        public ObservableCollection<Platform> Platforms { get; set; }

        public PlatformViewModel(WorkplaceContext db)
        {
            Platforms = db.GetPlatformsLocal();
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

        public ICommand AddPlatformCommand => new RelayCommand(AddPlatformCommandExecuted);
        public ICommand DeletePlatformCommand => new RelayCommand(DeletePlatformCommandExecuted);
        public ICommand SavePlatformsCommand => new RelayCommand(SavePlatformsCommandExecuted);
        public ICommand CheckPlatformsCommand => new RelayCommand(CheckPlatformsCommandExecuted);

        private void CheckPlatformsCommandExecuted(object obj)
        {
            CheckPlatforms();
        }

        private void SavePlatformsCommandExecuted(object obj)
        {
            if (selectedPlatform is null) return;
            db.UpdateEntity(selectedPlatform);
        }

        private void DeletePlatformCommandExecuted(object obj)
        {
            if (selectedPlatform is null) return;
            db.RemoveEntity(selectedPlatform);
        }

        private void AddPlatformCommandExecuted(object obj)
        {
            var platfrorm1c = new Platform
            {
                Name = "8.3.15.7777",
                FullPath = "C:\\Program Files\\1cv8\\8.3.15.7777\\bin\\1cv8.exe"
            };
            db.AddEntity(platfrorm1c);
        }
        
        private void CheckPlatforms()
        {
            foreach (Platform platform in Platforms)
            {
                platform.Exist = File.Exists(platform.FullPath);
                db.Platforms.Update(platform);
            }
            // TODO добавить обход по всем платформам сравнивая фулл патс в папках PF x86 и PF 
            db.SaveChanges();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
