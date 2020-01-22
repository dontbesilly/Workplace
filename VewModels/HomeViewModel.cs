using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Workplace1c.Distribution1c;

namespace Workplace1c.VewModels
{
    class HomeViewModel
    {
        private WorkplaceContext db;
        private readonly Telega telega;

        public ObservableCollection<DistributionAction> DistributionActions { get; set; }
        public ObservableCollection<Base> Bases { get; set; }
        public TelegramSetting TelegramSetting { get; set; }
        public bool BotReceiving { get; set; }

        public HomeViewModel(WorkplaceContext db)
        {
            DistributionActions = db.GetDistributionActionsLocal();
            Bases = db.GetBasesLocal();
            this.db = db;
            TelegramSetting = db.TelegramSetting;

            telega = new Telega(Bases, TelegramSetting);

        }

        public ICommand StartTelegramCommand => new RelayCommand(StartTelegramCommandExecuted);
        public ICommand StopTelegramCommand => new RelayCommand(StopTelegramCommandExecuted);

        private void StartTelegramCommandExecuted(object obj)
        {
            telega.Start();
        }

        private void StopTelegramCommandExecuted(object obj)
        {
            telega.Stop();
        }
    }
}
