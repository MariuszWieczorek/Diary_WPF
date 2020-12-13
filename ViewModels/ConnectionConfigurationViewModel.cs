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

        private bool TestSettings()
        {
            MessageBox.Show(ConnectionSettings.ServerAddress);

            if (!ConnectionSettings.IsValid)
            {
                MessageBox.Show("Nie uzupełniłeś wszystkich wymaganych pól !");
                // return false;
            }

            bool isConnectionOk = true;


            string testConnectionString = DbHelper.ConnectionStringBuilder(ConnectionSettings.ServerAddress,ConnectionSettings.ServerName,ConnectionSettings.Database,ConnectionSettings.User,ConnectionSettings.Password);

            using (var context = new ApplicationBbContext(testConnectionString))
            {
                try
                {
                    context.Groups.ToList();
                }
                catch (Exception e)
                {

                    MessageBox.Show(e.Message);
                    isConnectionOk = false;
                }
                
                
                return isConnectionOk;
            }

        }

        private void Confirm(object obj)
        {
            if (TestSettings())
            {
                SaveSettings();
                
                // System.Windows.Application.Current.Shutdown();
                CloseWindow(obj as Window);
            }

        }

        private void Close(object obj)
        {
            CloseWindow(obj as Window);
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }
    }
}
