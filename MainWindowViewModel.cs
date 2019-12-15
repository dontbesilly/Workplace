using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Workplace1c
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private WorkplaceContext db;
        private Base selectedBase;

        public ObservableCollection<Base> Bases { get; set; }
        public Base SelectedBase { get => selectedBase; set { selectedBase = value; OnPropertyChanged(nameof(SelectedBase)); } }

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
                    foreach (Base b in Bases)
                    {
                        db.Bases.Update(selectedBase);
                        db.SaveChanges();
                    }
                });
            }
        }

        public MainWindowViewModel()
        {
            db = new WorkplaceContext();
            db.Database.EnsureCreated();

            db.Bases.Load();

            Bases = db.Bases.Local.ToObservableCollection();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
