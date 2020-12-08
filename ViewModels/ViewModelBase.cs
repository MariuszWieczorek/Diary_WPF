using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Diary.ViewModels
{
    class ViewModelBase : INotifyPropertyChanged
    {
        // przykładowa właściwość zgłaszająca, że została zmieniona
        // snipet propfull
        private int _myProperty;

        public int MyProperty
        {
            get { return _myProperty; }
            set 
            { 
                _myProperty = value;
                // w set zgłaszamy zdażenie jako parametr przekazujemy nazwę właściwości
                // onPropertyChanged("MyProperty");
                // dzięki atrybutowi [CallerMemberName] nie musimy jawnie wpisywać
                // a w zasadzie przepisywać nazwy metody
                OnPropertyChanged();
            }
        }
        // -----------------------------------------------------------------------------------------

        // klasa implementująca intrfejs INotifyPropertyChanged
        // musi mieć zadeklarowany event PropertyChanged
        // dodatkowo dodajemy metodę pomocniczą onPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // aby było łatwiej używać nam powyższego zdarzenia
        // tworzymy metodę pomocniczą onPropertyChanged
        // virtual - może zostać nadpisana w klasach pochodnych
        // [CallerMemberName] dzięki temu nie musimy wpisywać nazwy "MyProperty"
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // jezeli event nie jest null'em do zostanie on wyzwolony
            // i przekaże nazwę właściwości
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
