using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Workplace1c.Distribution1c
{
    class Release : INotifyPropertyChanged
    {
        private string name = "";
        private Distribution distribution;

        public int Id { get; set; }

        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        public Distribution Distribution
        {
            get => distribution;
            set { distribution = value; OnPropertyChanged(nameof(Distribution)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
