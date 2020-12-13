using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Diary.Models.Wrappers
{
    public class ConnectionSettingsWrapper : IDataErrorInfo
    {
        public string ServerAddress { get; set; }
        public string ServerName { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        private bool _isServerAddressValid;
        private bool _isDatabaseValid;
        private bool _isUserValid;
        private bool _isPasswordValid;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(ServerAddress):
                        {
                            if (string.IsNullOrWhiteSpace(ServerAddress))
                            {
                                Error = "Adres serwera jest wymagane !";
                                _isServerAddressValid = false;
                            }
                            else
                            {
                                Error = string.Empty;
                                _isServerAddressValid = true;
                            }
                        }
                        break;

                    case nameof(Database):
                        {
                            if (string.IsNullOrWhiteSpace(Database))
                            {
                                Error = "Nazwa bazy jest wymagana !";
                                _isDatabaseValid = false;
                            }
                            else
                            {
                                Error = string.Empty;
                                _isDatabaseValid = true;
                            }
                        }
                        break;

                    case nameof(User):
                        {
                            if (string.IsNullOrWhiteSpace(User))
                            {
                                Error = "Pole użytkownik jest wymagane !";
                                _isUserValid = false;
                            }
                            else
                            {
                                Error = string.Empty;
                                _isUserValid = true;
                            }
                        }
                        break;

                    case nameof(Password):
                        {
                            if (string.IsNullOrWhiteSpace(Password))
                            {
                                Error = "Pole hasło jest wymagane !";
                                _isPasswordValid = false;
                            }
                            else
                            {
                                Error = string.Empty;
                                _isPasswordValid = true;
                            }
                        }
                        break;

                    default:
                        break;
                }
                return Error;
            }
        }
        public string Error { get; set; }
        public bool IsValid
        {
            get
            {
                return _isServerAddressValid && _isDatabaseValid && _isUserValid && _isPasswordValid;
            }
        }
    }
}
