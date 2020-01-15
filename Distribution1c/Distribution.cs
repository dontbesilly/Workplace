using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Workplace1c.Distribution1c
{
    class Distribution : INotifyPropertyChanged
    {
        private string name = "", rootFolder = "", prefix = "", testCatalog = "", licenseServerRelease = "";

        public int Id { get; set; }

        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string LicenseServerRelease
        {
            get => licenseServerRelease;
            set { licenseServerRelease = value; OnPropertyChanged(nameof(LicenseServerRelease)); }
        }

        public string RootFolder
        {
            get => rootFolder;
            set { rootFolder = value; OnPropertyChanged(nameof(RootFolder)); }
        }

        public string Prefix
        {
            get => prefix;
            set { prefix = value; OnPropertyChanged(nameof(Prefix)); }
        }

        public string TestCatalog
        {
            get => testCatalog;
            set { testCatalog = value; OnPropertyChanged(nameof(TestCatalog)); }
        }

        public Platform Platform { get; set; }

        public ObservableCollection<Release> Releases { get; set; }

        public Distribution()
        {
            Releases = new ObservableCollection<Release>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
