using Diary.Commands;
using Diary.Models;
using Diary.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
        public ICommand AddStudentsCommand { get; set; }
        public ICommand EditStudentsCommand { get; set; }
        public ICommand DeleteStudentsCommand { get; set; }

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get 
            {
              
               return _selectedStudent;
            }
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
            RefreshStudentsCommand = new RelayCommand(RefreshStudents);
            AddStudentsCommand = new RelayCommand(AddEditStudents);
            EditStudentsCommand = new RelayCommand(AddEditStudents, CanEditDeleteStudents);
            DeleteStudentsCommand = new AsyncRelayCommand(DeleteStudents, CanEditDeleteStudents);
             
            RefreshDiary();

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

        private void RefreshDiary()
        {
            PopulateStudents();
            PopulateGroups();
        }


  
        private void RefreshStudents(object obj)
        {
            RefreshDiary();
        }

    

 
        private void AddEditStudents(object obj)
        {
            // nie jest to dobre rozwiązanie
            // Powinno się zastosować Dependency Injection
            // Utrudnia to stosowanie testów jednostkowych
            var addEditStudentWindow = new AddEditStudentView(obj as Student);
            // Subskrybujemy zdarzenie closed
            addEditStudentWindow.Closed += AddEditStudentWindow_Closed;
            addEditStudentWindow.ShowDialog();
            addEditStudentWindow.Closed -= AddEditStudentWindow_Closed;
        }

        private void AddEditStudentWindow_Closed(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private bool CanEditDeleteStudents(object obj)
        {
            return SelectedStudent != null;
        }

        private async Task DeleteStudents(object obj)
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            
            var dialog = 
                await metroWindow.ShowMessageAsync("Usuwanie ucznia",
                $"Czy na pewno chcesz usunąć ucznia {SelectedStudent.FirstName} {SelectedStudent.LastName}",
                MessageDialogStyle.AffirmativeAndNegative);

            if (dialog != MessageDialogResult.Affirmative)
            {
                return;
            }

            // TODO : usuwanie z bazy

            RefreshDiary();
        }


    }
}
