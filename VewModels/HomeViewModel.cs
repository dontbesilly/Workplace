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
            db.DistributionActions.Load();
            DistributionActions = db.DistributionActions.Local.ToObservableCollection();
            this.db = db;
        }
    }
}
