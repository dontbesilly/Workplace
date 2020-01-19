using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace Workplace1c.VewModels
{
    class BasesViewModel : INotifyPropertyChanged
    {
        private WorkplaceContext db;
        public ObservableCollection<Base> Bases { get; set; }

        public BasesViewModel(WorkplaceContext db)
        {
            Bases = db.GetBasesLocal();
            this.db = db;
        }

        public ICommand AddBaseCommand => new RelayCommand(AddBaseCommandExecuted);
        public ICommand DeleteBaseCommand => new RelayCommand(DeleteBaseCommandExecuted);
        public ICommand SaveBasesCommand => new RelayCommand(SaveBaseCommandExecuted);

        private Base selectedBase;
        public Base SelectedBase
        {
            get => selectedBase;
            set
            {
                selectedBase = value;
                BaseParamsCardVisibility = Visibility.Visible;
                OnPropertyChanged(nameof(SelectedBase));
                if (selectedBase is null) return;
                RepoCardVisibility = selectedBase.IsRepository ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void AddBaseCommandExecuted(object obj)
        {
            var base1C = new Base
            {
                Title = "Новая база"
            };
            db.AddEntity(base1C);
        }

        private void DeleteBaseCommandExecuted(object obj)
        {
            if (selectedBase is null) return;
            db.RemoveEntity(selectedBase);
        }

        private void SaveBaseCommandExecuted(object obj)
        {
            if (selectedBase is null) return;
            db.UpdateEntity(selectedBase);
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

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
