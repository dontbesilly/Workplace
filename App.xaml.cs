using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Workplace1c
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var vm = new MainWindowViewModel();
            vm.InitMainWindow(new MainWindow { DataContext = vm });
        }
    }
}
