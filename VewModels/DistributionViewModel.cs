using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Workplace1c.Distribution1c;

namespace Workplace1c.VewModels
{
    class DistributionViewModel
    {
        private WorkplaceContext db;

        public ObservableCollection<Distribution> Distributions { get; set; }

        public DistributionViewModel(WorkplaceContext db)
        {
            db.Distributions.Load();
            Distributions = db.Distributions.Local.ToObservableCollection();
            this.db = db;
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
                    db.Distributions.Add(distr);
                    db.SaveChanges();
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
