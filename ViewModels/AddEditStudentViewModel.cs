using Diary.Commands;
using Diary.Models;
using Diary.Models.Domains;
using Diary.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Diary.ViewModels
{
    class AddEditStudentViewModel : ViewModelBase
    {

        private Repository _repository = new Repository();
        public ICommand CloseCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }

        private StudentWrapper _student;
        public StudentWrapper Student
        {
            get { return _student; }
            set
            {
                _student = value;
                OnPropertyChanged();
            }
        }

        private bool _isUpdate;
        public bool IsUpdate
        {
            get { return _isUpdate; }
            set
            {
                _isUpdate = value;
                OnPropertyChanged();
            }
        }

        private int _selectedGroupId;
        public int SelectedGroupId
        {
            get { return _selectedGroupId; }
            set
            {
                _selectedGroupId = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<Group> _groups;
        public ObservableCollection<Group> Groups
        {
            get { return _groups; }
            set
            {
                _groups = value;
                OnPropertyChanged();
            }
        }


        private void InitGroups()
        {

            var groups = _repository.GetGroups();
            groups.Insert(0, new Group { Id = 0, Name = "--" });

            // tworzymy nowy obiekt ObservableCollection i przekazujemy
            // listę jako parametr
            Groups = new ObservableCollection<Group>(groups);
            Student.Group.Id = 0;
        }

     

        public AddEditStudentViewModel(StudentWrapper student = null)
        {
            CloseCommand = new RelayCommand(Close);
            ConfirmCommand = new RelayCommand(Confirm);

            if (student == null)
            {
                Student = new StudentWrapper();
            }
            else
            {
                Student = student;
                IsUpdate = true;
            }

            InitGroups();
        }

        private void Close(object obj)
        {
            // Obiekt rzutujemy na Window
            CloseWindow(obj as Window);
        }

        private void Confirm(object obj)
        {
            if (!IsUpdate)
                AddNewStudent();
            else
                UpdateStudent();

            CloseWindow(obj as Window);
        }

        private void AddNewStudent()
        {
            //TODO: Dodanie nowego studenta do bazy
        }

        private void UpdateStudent()
        {
            //TODO: Aktualizacja Studenta w bazie
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }


        /// <summary>
        /// Archiwalna metoda
        /// </summary>
        
           
    }
}
