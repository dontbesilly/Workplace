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

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??= new RelayCommand(obj =>
                {
                    var base1c = new Base
                    {
                        Title = "new base"
                    };
                    db.Bases.Add(base1c);
                    selectedBase = base1c;
                    OnPropertyChanged(nameof(SelectedBase));
                    db.SaveChanges();
                });
            }
        }

        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??= new RelayCommand(obj =>
                {
                    if (!(obj is Base base1c)) return;
                    db.Bases.Remove(base1c);
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
