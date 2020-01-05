using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Workplace1c
{
    class Platform : INotifyPropertyChanged
    {
        private string name = "", fullPath = "";
        private bool exist = false;

        public int Id { get; set; }

        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string FullPath
        {
            get => fullPath;
            set { fullPath = value; OnPropertyChanged(nameof(FullPath)); }
        }

        public bool Exist
        {
            get => exist;
            set { exist = value; OnPropertyChanged(nameof(Exist)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    }
}
