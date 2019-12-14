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
        private Random random = new Random();

        public ObservableCollection<Base> Bases { get; set; }
        public Base SelectedBase { get => selectedBase; set { selectedBase = value; OnPropertyChanged(nameof(SelectedBase)); } }

        private RelayCommand addBaseCommand;
        public RelayCommand AddBaseCommand
        {
            get
            {
                return addBaseCommand ??= new RelayCommand(obj =>
                {
                    var base1c = new Base
                    {
                        Title = $"new base {random.Next()}"
                    };
                    db.Bases.Add(base1c);
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

    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
