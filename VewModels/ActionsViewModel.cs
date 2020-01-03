using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Workplace1c.Distribution;

namespace Workplace1c.VewModels
{
    class ActionsViewModel : INotifyPropertyChanged
    {
        public Activity[] Activities { get; private set; }

        private Activity selectedActivity;

        public Activity SelectedActivity
        {
            get => selectedActivity;
            set
            {
                selectedActivity = value;
                OnPropertyChanged(nameof(SelectedActivity));
            }
        }

        public ActionsViewModel()
        {
            Activities = new[]
            {
                new Activity{Name = "Выгрузка базы"},
                new Activity{Name = "Загрузка базы"}
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
