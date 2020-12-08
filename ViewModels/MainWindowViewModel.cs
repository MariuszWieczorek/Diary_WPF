using Diary.Commands;
using Diary.Models;
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
    class MainWindowViewModel : ViewModelBase
    {

        public ICommand RefreshStudentsCommand { get; set; }

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set
            { 
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }

        // używamy zamiast List<> zachowuje się jak zwykła lista
        // implementuje dodatkowo interfejsy INotifyCollectionChanged, INotifyPropertyChanged
        // dzięki temu datagrid będzie informowany o tym
        // czy jekiś element został dodany lub zmieniony
        private ObservableCollection<Student> _students;
        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set
            {
                _students = value;
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


        public MainWindowViewModel()
        {
            RefreshStudentsCommand = new RelayCommand(RefreshStudents, CanRefreshStudents);

            PopulateStudents();
            PopulateGroups();

        }

        private void PopulateStudents()
        {
            Students = new ObservableCollection<Student>
            {
            new Student
            {
                Id = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                Group = new Group{Id = 1}
            },

            new Student
            {
                Id = 2,
                FirstName = "Jan",
                LastName = "Nowak",
                Group = new Group{Id = 1}
            },

            new Student
            {
                Id = 3,
                FirstName = "Alfred",
                LastName = "Kowalski",
                Group = new Group{Id = 1}
            },

             new Student
             {
                 Id = 4,
                 FirstName = "Joanna",
                 LastName = "Bartkowiak",
                 Group = new Group { Id = 1 }
             },
            };
        }

        private void PopulateGroups()
        {
            Groups = new ObservableCollection<Group>
            {
                new Group {Id = 0, Name = "Wszyscy" },
                new Group {Id = 1, Name = "1A" },
                new Group {Id = 2, Name = "2A" },
                new Group {Id = 3, Name = "2A" },
                new Group {Id = 4, Name = "2B" },
            };

            SelectedGroupId = 0;
        }

            private bool CanRefreshStudents(object obj)
        {
            return true; // zawsze będzie możliwość kliknięcia tego przycisku
        }

        private void RefreshStudents(object obj)
        {
           
        }

    }
}
