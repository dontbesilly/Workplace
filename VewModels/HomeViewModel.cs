using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Workplace1c.Distribution1c;

namespace Workplace1c.VewModels
{
    class HomeViewModel : INotifyPropertyChanged
    {
        private WorkplaceContext db;
        private Telega telega;

        public ObservableCollection<DistributionAction> DistributionActions { get; set; }
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

        public HomeViewModel(WorkplaceContext db)
        {
            DistributionActions = db.GetDistributionActionsLocal();
            Bases = db.GetBasesLocal();
            this.db = db;
            TelegramSetting = db.TelegramSetting;

            
        }

        public ICommand StartTelegramCommand => new RelayCommand(StartTelegramCommandExecuted);
        public ICommand StopTelegramCommand => new RelayCommand(StopTelegramCommandExecuted);

        private void StartTelegramCommandExecuted(object obj)
        {
            telega = new Telega(Bases, TelegramSetting);
            telega.Start();
        }

        private void StopTelegramCommandExecuted(object obj)
        {
            telega.Stop();
        }

        public void CheckBotIsReceiving(bool isReceiving)
        {
            BotReceiving = isReceiving;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
