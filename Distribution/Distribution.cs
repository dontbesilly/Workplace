using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Workplace1c.Distribution
{
    class Distribution : INotifyPropertyChanged
    {
        private string name = "", rootFolder = "";
        // TODO сделать переменные окружения для заполнения файлов. serverLic, prefix, testCatalog, currentRelease mb.

        public int Id { get; set; }

        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string RootFolder
        {
            get => rootFolder;
            set { rootFolder = value; OnPropertyChanged(nameof(RootFolder)); }
        }

        public Platform Platform { get; set; }

        public ObservableCollection<string> Releases { get; set; }

        public Distribution()
        {
            Releases = new ObservableCollection<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
