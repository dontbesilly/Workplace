using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using Workplace1c.Distribution1c;
using Workplace1c.Views;

namespace Workplace1c.VewModels
{
    class ActionsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Base> Bases { get; }
        
        public ActionsViewModel(MainWindowViewModel mainWindowViewModel)
        {
            Bases = mainWindowViewModel.BasesViewModel.Bases;
        }

        #region UnloadBaseCard

        private Base selectedBaseUnload;
        public Base SelectedBaseUnload
        {
            get => selectedBaseUnload;
            set
            {
                selectedBaseUnload = value;
                OnPropertyChanged(nameof(SelectedBaseUnload));
            }
        }

        private string unloadBasePath = "C:\\1cv8.dt";
        public string UnloadBasePath
        {
            get => unloadBasePath;
            set
            {
                unloadBasePath = value;
                OnPropertyChanged(nameof(UnloadBasePath));
            }
        }

        private RelayCommand unloadBaseCommand;
        public RelayCommand UnloadBaseCommand
        {
            get
            {
                return unloadBaseCommand ??= new RelayCommand(obj =>
                {
                    if (selectedBaseUnload is null) return;
                    Console.WriteLine(selectedBaseUnload.Title);
                    Console.WriteLine(unloadBasePath);
                });
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
