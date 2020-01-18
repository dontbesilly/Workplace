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

        public ObservableCollection<DistributionAction> DistributionActions { get; set; }

        public HomeViewModel(WorkplaceContext db)
        {
            DistributionActions = db.GetDistributionActionsLocal();
            this.db = db;
        }

        public ICommand StartTelegramCommand => new RelayCommand(StartTelegramCommandExecuted);

        private void StartTelegramCommandExecuted(object obj)
        {
            // telega = new Telega(Constants.BitfinanceCommandToken, Bases, "8.3.15.1778");
            //telega.Start();

        }
    }
}
