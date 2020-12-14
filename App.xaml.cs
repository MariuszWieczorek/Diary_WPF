using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using Diary.Views;
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

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //initialize the splash screen and set it as the application main window
            var splashScreen = new DiarySplashScreen();
            this.MainWindow = splashScreen;
            splashScreen.Show();

            //in order to ensure the UI stays responsive, we need to
            //do the work on a different thread
            Task.Factory.StartNew(() =>
            {
                //simulate some work being done
                //  System.Threading.Thread.Sleep(3000);

                //since we're not on the UI thread
                //once we're done we need to use the Dispatcher
                //to create and show the main window
                this.Dispatcher.Invoke(() =>
                {
                    //initialize the main window, set it as the application main window
                    //and close the splash screen
                    //var mainWindow = new MainWindow();
                    //this.MainWindow = mainWindow;
                   // mainWindow.Show();
                    splashScreen.Close();
                });
            });
        }
    }
}
