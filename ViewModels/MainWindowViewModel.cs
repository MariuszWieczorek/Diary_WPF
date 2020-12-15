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
        public ICommand ConectionConfigurationCommand { get; set; }
        public ICommand AddStudentsCommand { get; set; }
        public ICommand EditStudentsCommand { get; set; }
        public ICommand DeleteStudentsCommand { get; set; }

        public ICommand LoadedWindowCommand { get; set; }

        public ICommand ComboBoxChanged { get; set; }


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
            RefreshStudentsCommand = new RelayCommand(RefreshStudents);
            AddStudentsCommand = new RelayCommand(AddEditStudents);
            ConectionConfigurationCommand = new RelayCommand(ConectionConfiguration);
            EditStudentsCommand = new RelayCommand(AddEditStudents, CanEditDeleteStudents);
            DeleteStudentsCommand = new AsyncRelayCommand(DeleteStudents, CanEditDeleteStudents);
            LoadedWindowCommand = new RelayCommand(LoadedWindow);
            ComboBoxChanged = new RelayCommand(ComboBoxLostFocus);


            // Gdy test połączenie wypadnie negatywnie
            // wywołane zostaje okienko konfiguracyjne połączenia SQL
            // po zapisie ustawień w tym okienku zostaje wymuszony restart aplikacji
            // po anulowaniu nastepuje zamknięcie aplikacji
            if (!DbHelper.ConnectionSettingsTest())
            {
                var connectionConfigurationWindow = new ConnectionConfigurationView();
                connectionConfigurationWindow.ShowDialog();
            }

            InitGroups();
            RefreshDiary();
        }

        private void ComboBoxLostFocus(object obj)
        {
            RefreshDiary();
        }

        private void LoadedWindow(object arg)
        {
            // MessageBox.Show("Loaded Window AAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        }

        /// <summary>
        /// Pobiera słownik grup z bazy danych służący do filtrowania danych
        /// Dodaje rekord o Id == 0, któy oznacza, że nie filtrujemy po grupie
        /// </summary>
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
            Students = new ObservableCollection<StudentWrapper>(_repository.GetStudents(SelectedGroupId));
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

            _repository.DeleteStudent(SelectedStudent.Id);

            RefreshDiary();
        }

        /// <summary>
        /// Wywołanie okna konfiguracji połączenia
        /// </summary>
        /// <param name="obj"></param>
        private void ConectionConfiguration(object obj)
        {
            var connectionConfigurationWindow = new ConnectionConfigurationView();
            connectionConfigurationWindow.ShowDialog();
        }


    }
}
