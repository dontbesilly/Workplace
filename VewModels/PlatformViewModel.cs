﻿using Microsoft.EntityFrameworkCore;
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

        private RelayCommand addPlatformCommand;
        public RelayCommand AddPlatformCommand
        {
            get
            {
                return addPlatformCommand ??= new RelayCommand(obj =>
                {
                    var platfrorm1c = new Platform
                    {
                        Name = "8.3.15.0000",
                        FullPath = "C:\\Program Files\\1cv8\\8.3.15.0000\\bin\\1cv8.exe"
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
