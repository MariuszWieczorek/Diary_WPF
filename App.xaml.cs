using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Diary
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // zalogowanie błędu do pliku 
            // dziedziczymy po metroWindow więc zawsze możemy rzutować na nie
            var metroWindow = Current.MainWindow as MetroWindow;
            metroWindow.ShowMessageAsync("Nieoczekiwany Wyjątek",
                "Wystąpił nieoczekiwany wyjątek" + Environment.NewLine +
                e.Exception.Message);

            e.Handled = true;

        }
    }
}
