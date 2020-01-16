using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Workplace1c.Distribution1c;

namespace Workplace1c.VewModels
{
    class DistributionViewModel
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

        private RelayCommand addDistributionCommand;
        public RelayCommand AddDistributionCommand
        {
            get
            {
                return addDistributionCommand ??= new RelayCommand(obj =>
                {
                    var distr = new Distribution
                    {
                        Name = "Новая сборка"
                    };
                    db.AddDistribution(distr);
                });
            }
        }

        private RelayCommand addReleaseCommand;
        public RelayCommand AddReleaseCommand
        {
            get
            {
                return addReleaseCommand ??= new RelayCommand(obj =>
                {
                    if (selectedDistribution is null) return;
                    var release = new Release { Name = "0.0.0.0", Distribution = selectedDistribution };
                    db.AddRelease(release);
                });
            }
        }

        private RelayCommand deleteReleaseCommand;
        public RelayCommand DeleteReleaseCommand
        {
            get
            {
                return deleteReleaseCommand ??= new RelayCommand(obj =>
                {
                    if (selectedDistribution is null && selectedRelease is null) return;
                    db.RemoveRelease(selectedRelease);
                });
            }
        }

        private RelayCommand deleteDistributionCommand;
        public RelayCommand DeleteDistributionCommand
        {
            get
            {
                return deleteDistributionCommand ??= new RelayCommand(obj =>
                {
                    if (selectedDistribution is null) return;
                    db.Distributions.Remove(selectedDistribution);
                    db.SaveChanges();
                });
            }
        }

        private RelayCommand saveDistributionsCommand;
        public RelayCommand SaveDistributionsCommand
        {
            get
            {
                return saveDistributionsCommand ??= new RelayCommand(obj =>
                {
                    if (selectedDistribution is null) return;
                    db.Distributions.Update(selectedDistribution);
                    db.SaveChanges();
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
