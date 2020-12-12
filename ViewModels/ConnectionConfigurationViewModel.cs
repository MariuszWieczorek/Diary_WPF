using Diary.Commands;
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

        public string xxx = "huj jebany";

        public ConnectionConfigurationViewModel()
        {
            CloseCommand = new RelayCommand(Close);
            ConfirmCommand = new RelayCommand(Confirm);
        }

        private void Confirm(object obj)
        {
            CloseWindow(obj as Window);
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
