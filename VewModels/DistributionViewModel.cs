using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Workplace1c.Distribution1c;

namespace Workplace1c.VewModels
{
    class DistributionViewModel : INotifyPropertyChanged
    {
        private WorkplaceContext db;

        public ObservableCollection<Distribution> Distributions { get; set; }
        public ObservableCollection<Base> Bases { get; }
        public ObservableCollection<Platform> Platforms { get; }

        public DistributionViewModel(WorkplaceContext db)
        {
            Distributions = db.GetDistributionsLocal();
            Bases = db.GetBasesLocal();
            Platforms = db.GetPlatformsLocal();
            this.db = db;
        }

        private Release selectedRelease;
        public Release SelectedRelease
        {
            get => selectedRelease;
            set
            {
                selectedRelease = value;
                OnPropertyChanged(nameof(SelectedRelease));
            }
        }

        private Distribution selectedDistribution;
        public Distribution SelectedDistribution
        {
            get => selectedDistribution;
            set
            {
                selectedDistribution = value;
                OnPropertyChanged(nameof(SelectedDistribution));
            }
        }

        public ICommand AddDistributionCommand => new RelayCommand(AddDistributionCommandExecuted);
        public ICommand AddReleaseCommand => new RelayCommand(AddReleaseCommandExecuted);
        public ICommand DeleteReleaseCommand => new RelayCommand(DeleteReleaseCommandExecuted);
        public ICommand DeleteDistributionCommand => new RelayCommand(DeleteDistributionCommandExecuted);
        public ICommand SaveDistributionsCommand => new RelayCommand(SaveDistributionsCommandExecuted);

        private void AddDistributionCommandExecuted(object obj)
        {
            var distr = new Distribution
            {
                Name = "Новая сборка"
            };
            db.AddEntity(distr);
        }

        private void AddReleaseCommandExecuted(object obj)
        {
            if (selectedDistribution is null) return;
            var release = new Release { Name = "0.0.0.0", Distribution = selectedDistribution };
            db.AddEntity(release);
        }

        private void DeleteReleaseCommandExecuted(object obj)
        {
            if (selectedDistribution is null && selectedRelease is null) return;
            db.RemoveEntity(selectedRelease);
        }

        private void DeleteDistributionCommandExecuted(object obj)
        {
            if (selectedDistribution is null) return;
            db.RemoveEntity(SelectedDistribution);
        }

        private void SaveDistributionsCommandExecuted(object obj)
        {
            if (selectedDistribution is null) return;
            db.UpdateEntity(selectedDistribution);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
