using Diary.Commands;
using Diary.Models;
using Diary.Models.Converters;
using Diary.Models.Domains;
using Diary.Models.Wrappers;
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

        private Repository _repository = new Repository();
        public ICommand RefreshStudentsCommand { get; set; }
        public ICommand AddStudentsCommand { get; set; }
        public ICommand EditStudentsCommand { get; set; }
        public ICommand DeleteStudentsCommand { get; set; }

        private StudentWrapper _selectedStudent;
        public StudentWrapper SelectedStudent
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
        private ObservableCollection<StudentWrapper> _students;
        public ObservableCollection<StudentWrapper> Students
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

            var str = "test".ToUpper().Replace("T", "123").Trim().PadRight(20, '0').AddXXX();
            //MessageBox.Show(str);

            // zostanie utworzone pierwsze zapytanie i utworzone bazy
            using ( var context = new ApplicationBbContext())
            {
                var students = context.Students.ToList();
            }

            RefreshStudentsCommand = new RelayCommand(RefreshStudents);
            AddStudentsCommand = new RelayCommand(AddEditStudents);
            EditStudentsCommand = new RelayCommand(AddEditStudents, CanEditDeleteStudents);
            DeleteStudentsCommand = new AsyncRelayCommand(DeleteStudents, CanEditDeleteStudents);

            // throw new Exception("błędzik");

            RefreshDiary();

        }

        private void PopulateStudents()
        {
            Students = new ObservableCollection<StudentWrapper>
            {
            new StudentWrapper
            {
                Id = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                Group = new GroupWrapper{Id = 1}
            },

            new StudentWrapper
            {
                Id = 2,
                FirstName = "Jan",
                LastName = "Nowak",
                Group = new GroupWrapper{Id = 1}
            },

            new StudentWrapper
            {
                Id = 3,
                FirstName = "Alfred",
                LastName = "Kowalski",
                Group = new GroupWrapper{Id = 1}
            },

             new StudentWrapper
             {
                 Id = 4,
                 FirstName = "Joanna",
                 LastName = "Bartkowiak",
                 Group = new GroupWrapper { Id = 1 }
             },
            };
        }

        private void InitGroups()
        {

            var groups = _repository.GetGroups();
            // wstawiamy grupę domyślną
            groups.Insert(0, new Group { Id = 0, Name = "Wszystkie" });

            // tworzymy nowy obiekt ObservableCollection i przekazujemy
            // listę jako parametr do konstruktora
            Groups = new ObservableCollection<Group>(groups);
            SelectedGroupId = 0;
        }

        private void RefreshDiary()
        {
            InitGroups();
            PopulateStudents();
          
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
            var addEditStudentWindow = new AddEditStudentView(obj as StudentWrapper);
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
