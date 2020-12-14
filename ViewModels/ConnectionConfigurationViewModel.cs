using Diary.Commands;
using Diary.Properties;
using Diary.Models.Wrappers;
using Diary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.Data;

namespace Diary.ViewModels
{
    class ConnectionConfigurationViewModel : ViewModelBase
    {
        public ICommand CloseCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }

        private ConnectionSettingsWrapper _connectionSettings;
        public ConnectionSettingsWrapper ConnectionSettings
        {
            get
            {
                return _connectionSettings;
            }
            set
            {
                _connectionSettings = value;
                OnPropertyChanged();
            }
        }


        public ConnectionConfigurationViewModel()
        {
            CloseCommand = new RelayCommand(Close);
            ConfirmCommand = new RelayCommand(Confirm);
            ReadSettings();
        }

        private void ReadSettings()
        {
            ConnectionSettings = new ConnectionSettingsWrapper();
            ConnectionSettings.ServerAddress = Settings.Default.ServerAddress;
            ConnectionSettings.ServerName = Settings.Default.ServerName;
            ConnectionSettings.Database = Settings.Default.Database;
            ConnectionSettings.User = Settings.Default.User;
            ConnectionSettings.Password = Settings.Default.Password;
        }

        private void SaveSettings()
        {
            Settings.Default.ServerAddress = ConnectionSettings.ServerAddress;
            Settings.Default.ServerName = ConnectionSettings.ServerName;
            Settings.Default.Database = ConnectionSettings.Database;
            Settings.Default.User = ConnectionSettings.User;
            Settings.Default.Password = ConnectionSettings.Password;
            Settings.Default.Save();
        }

        /// <summary>
        /// Przycisk Zapisz
        /// </summary>
        /// <param name="obj"></param>
        private void Confirm(object obj)
        {

            if (!ConnectionSettings.IsValid)
            {
                MessageBox.Show("Nie uzupełniłeś wszystkich wymaganych pól do ustanowienia połączenia!");
                return;
            }

            // zapisujemy ustawienia użytkownika dopiero wówczas
            // gdy ConnectionString z nich sklejony działa poprawnie
            // po zapisie restarturjmy aplikację
            string testConnectionString = DbHelper.ConnectionStringBuilder(ConnectionSettings.ServerAddress, ConnectionSettings.ServerName, ConnectionSettings.Database, ConnectionSettings.User, ConnectionSettings.Password);
            if (DbHelper.ConnectionSettingsTest(testConnectionString))
            {
                SaveSettings();
                var exeLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
                Process.Start(exeLocation);
                Application.Current.Shutdown();
                CloseWindow(obj as Window);
            }
        }
        
        /// <summary>
        /// Przycisk Zamknij
        /// </summary>
        /// <param name="obj"></param>
        private void Close(object obj)
        {
            if (!(String.IsNullOrWhiteSpace(ConnectionSettings.ServerAddress)
                && String.IsNullOrWhiteSpace(ConnectionSettings.Database)
                && String.IsNullOrWhiteSpace(ConnectionSettings.User)
                && String.IsNullOrWhiteSpace(ConnectionSettings.Password)))
                CloseWindow(obj as Window);
            else
                Application.Current.Shutdown();
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }
    }
}
