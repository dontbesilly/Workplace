using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Workplace1c.VewModels;
using Workplace1c.Views;

namespace Workplace1c
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindow MainWindow { get; private set; }
        public BasesView BasesView { get; private set; }
        public ActionsView ActionsView { get; private set; }

        public MainWindowViewModel()
        {
            InitializeViews();
            InitializeNavigationCommands();
        }

        public void InitMainWindow(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            MainWindow.Show();
        }

        private void InitializeNavigationCommands()
        {
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow),
                new CommandBinding(NavigationCommands.OpenBasesCommand, OpenBasesCommandExecuted));
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow),
                new CommandBinding(NavigationCommands.OpenActionsCommand, OpenActionsCommandExecuted));
        }
        private void InitializeViews()
        {
            BasesView = new BasesView { DataContext = new BasesViewModel() };
            ActionsView = new ActionsView { DataContext = new ActionsViewModel() };
        }

        private void OpenActionsCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindow.FrameBody.NavigationService.Navigate(ActionsView);
            MainWindow.MenuToggleButton.IsChecked = false;
        }

        private void OpenBasesCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindow.FrameBody.NavigationService.Navigate(BasesView);
            MainWindow.MenuToggleButton.IsChecked = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
