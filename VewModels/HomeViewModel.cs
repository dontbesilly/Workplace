using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
    }
}
