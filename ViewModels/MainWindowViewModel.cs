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
    class MainWindowViewModel : ViewModelBase
    {
        
        public ICommand RefreshStudentsCommand { get; set; }

        public MainWindowViewModel()
        {
            RefreshStudentsCommand = new RelayCommand(RefreshStudents,CanRefreshStudents);
        }
                
        private bool CanRefreshStudents(object obj)
        {
            return true; // zawsze będzie możliwość kliknięcia tego przycisku
        }

        private void RefreshStudents(object obj)
        {
            MessageBox.Show("RefreshStudents");
        }

    }
}
