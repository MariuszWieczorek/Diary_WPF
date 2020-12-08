using Diary.Commands;
using Diary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Diary.ViewModels
{
    class AddEditStudentViewModel : ViewModelBase
    {

        public ICommand CloseCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }

        public AddEditStudentViewModel(Student student = null)
        {
            CloseCommand = new RelayCommand(Close);
            ConfirmCommand = new RelayCommand(Confirm);
        }

        private void Close(object obj)
        {
            
           
        }

        private void Confirm(object obj)
        {


        }
    }
}
