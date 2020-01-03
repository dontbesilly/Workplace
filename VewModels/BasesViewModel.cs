﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace Workplace1c.VewModels
{
    class BasesViewModel : INotifyPropertyChanged
    {
        private WorkplaceContext db;
        private Telega telega;
        public ObservableCollection<Base> Bases { get; set; }

        private Base selectedBase;
        public Base SelectedBase
        {
            get => selectedBase;
            set
            {
                selectedBase = value;
                BaseParamsCardVisibility = Visibility.Visible;
                RepoCardVisibility = selectedBase.IsRepository ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged(nameof(SelectedBase));
            }
        }

        private RelayCommand addBaseCommand;
        public RelayCommand AddBaseCommand
        {
            get
            {
                return addBaseCommand ??= new RelayCommand(obj =>
                {
                    var base1C = new Base
                    {
                        Title = "Новая база"
                    };
                    db.Bases.Add(base1C);
                    db.SaveChanges();
                });
            }
        }

        private RelayCommand deleteBaseCommand;
        public RelayCommand DeleteBaseCommand
        {
            get
            {
                return deleteBaseCommand ??= new RelayCommand(obj =>
                {
                    if (selectedBase is null) return;
                    db.Bases.Remove(selectedBase);
                    db.SaveChanges();
                });
            }
        }

        private RelayCommand saveBasesCommand;
        public RelayCommand SaveBasesCommand
        {
            get
            {
                return saveBasesCommand ??= new RelayCommand(obj =>
                {
                    if (selectedBase is null) return;
                    db.Bases.Update(selectedBase);
                    db.SaveChanges();
                });
            }
        }

        private Visibility baseParamsCardVisibility = Visibility.Hidden;
        public Visibility BaseParamsCardVisibility
        {
            get => baseParamsCardVisibility;
            set
            {
                baseParamsCardVisibility = value;
                OnPropertyChanged(nameof(BaseParamsCardVisibility));
            }
        }

        private Visibility repoCardVisibility = Visibility.Hidden;
        public Visibility RepoCardVisibility
        {
            get => repoCardVisibility;
            set
            {
                repoCardVisibility = value;
                OnPropertyChanged(nameof(RepoCardVisibility));
            }
        }

        public BasesViewModel()
        {
            db = new WorkplaceContext();
            db.Database.EnsureCreated();

            db.Bases.Load();

            Bases = db.Bases.Local.ToObservableCollection();
            telega = new Telega(Constants.BitfinanceCommandToken, Bases, "8.3.15.1778");
            //telega.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}