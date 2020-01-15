using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Workplace1c.Distribution1c
{
    class DistributionAction : INotifyPropertyChanged
    {

        private Distribution distribution;
        private Release currentRelease;

        public int Id { get; set; }

        public Release CurrentRelease
        {
            get => currentRelease;
            set { currentRelease = value; OnPropertyChanged(nameof(CurrentRelease)); }
        }

        public Distribution Distribution
        {
            get => distribution;
            set { distribution = value; OnPropertyChanged(nameof(Distribution)); }
        }

        public ObservableCollection<Release> PreviousReleases { get; set; }

        public DistributionAction()
        {
            PreviousReleases = new ObservableCollection<Release>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    }
}
