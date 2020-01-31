using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Input;
using Workplace1c.Distribution1c;

namespace Workplace1c.VewModels
{
    class HomeViewModel : INotifyPropertyChanged
    {
        private readonly WorkplaceContext db;
        private Telega telega;
        private readonly Timer timer;

        public ObservableCollection<DistributionAction> DistributionActions { get; set; }
        public ObservableCollection<Distribution> Distributions { get; set; }
        public ObservableCollection<Base> Bases { get; set; }
        public TelegramSetting TelegramSetting { get; set; }

        private bool botReceiving;
        public bool BotReceiving
        {
            get => botReceiving;
            set
            {
                botReceiving = value;
                OnPropertyChanged(nameof(BotReceiving));
            }
        }

        private Distribution currentDistribution;
        public Distribution CurrentDistribution
        {
            get => currentDistribution;
            set
            {
                var distributionAction = DistributionActions.FirstOrDefault(x => x.Distribution == currentDistribution);
                if (distributionAction is null)
                {
                    var newDistributionAction = new DistributionAction { Distribution = currentDistribution };
                    db.AddEntity(newDistributionAction);
                    CurrentDistributionAction = newDistributionAction;
                }
                else
                {
                    CurrentDistributionAction = distributionAction;
                }

                currentDistribution = value;
                OnPropertyChanged(nameof(CurrentDistribution));
            }
        }

        private DistributionAction currentDistributionAction;
        public DistributionAction CurrentDistributionAction
        {
            get => currentDistributionAction;
            set
            {
                currentDistributionAction = value;
                OnPropertyChanged(nameof(CurrentDistributionAction));
            }
        }

        public HomeViewModel(WorkplaceContext db)
        {
            DistributionActions = db.GetDistributionActionsLocal();
            Distributions = db.GetDistributionsLocal();
            Bases = db.GetBasesLocal();
            this.db = db;
            TelegramSetting = db.TelegramSetting;
            timer = new Timer(5000);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }

        public ICommand StartTelegramCommand => new RelayCommand(StartTelegramCommandExecuted);
        public ICommand StopTelegramCommand => new RelayCommand(StopTelegramCommandExecuted);
        public ICommand CheckBotIsReceivingCommand => new RelayCommand(CheckBotIsReceivingCommandExecuted);
        public ICommand SaveDistributionActionCommand => new RelayCommand(SaveDistributionActionCommandExecuted);

        private void SaveDistributionActionCommandExecuted(object obj)
        {
            db.UpdateEntity(CurrentDistributionAction);
        }

        private void StartTelegramCommandExecuted(object obj)
        {
            telega = new Telega(Bases, TelegramSetting);
            telega.Start();
            BotReceiving = telega.IsReceiving;
            timer.Enabled = true;
        }

        private void StopTelegramCommandExecuted(object obj)
        {
            telega.Stop();
            BotReceiving = false;
            timer.Enabled = false;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            CheckBotIsReceivingCommandExecuted(source);
        }

        public void CheckBotIsReceivingCommandExecuted(object obj)
        {
            if (telega is null)
            {
                BotReceiving = false;
                return;
            }
            BotReceiving = telega.IsReceiving;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
