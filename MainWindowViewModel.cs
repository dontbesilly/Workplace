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
        private WorkplaceContext db;

        public MainWindow MainWindow { get; private set; }
        public BasesView BasesView { get; private set; }
        public BasesViewModel BasesViewModel { get; private set; }
        public ActionsView ActionsView { get; private set; }
        public ActionsViewModel ActionsViewModel { get; private set; }
        public PlatformView PlatformView { get; private set; }
        public PlatformViewModel PlatformViewModel { get; private set; }
        public DistributionView DistributionView { get; private set; }
        public DistributionViewModel DistributionViewModel { get; private set; }

        public MainWindowViewModel()
        {
            db = new WorkplaceContext();

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
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow),
                new CommandBinding(NavigationCommands.OpenPlatformsCommand, OpenPlatformsCommandExecuted));
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow),
                new CommandBinding(NavigationCommands.OpenDistributionsCommand, OpenDistributionsCommandExecuted));
        }
        private void InitializeViews()
        {
            BasesViewModel = new BasesViewModel(db);
            BasesView = new BasesView { DataContext = BasesViewModel };
            PlatformViewModel = new PlatformViewModel(db);
            PlatformView = new PlatformView { DataContext = PlatformViewModel };
            ActionsViewModel = new ActionsViewModel(db);
            ActionsView = new ActionsView { DataContext = ActionsViewModel };
            DistributionViewModel = new DistributionViewModel(db);
            DistributionView = new DistributionView { DataContext = DistributionViewModel };
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

        private void OpenPlatformsCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindow.FrameBody.NavigationService.Navigate(PlatformView);
            MainWindow.MenuToggleButton.IsChecked = false;
        }

        private void OpenDistributionsCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindow.FrameBody.NavigationService.Navigate(DistributionView);
            MainWindow.MenuToggleButton.IsChecked = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
