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
        // snipet propfull
        private int _myProperty;

        public int MyProperty
        {
            get { return _myProperty; }
            set 
            { 
                _myProperty = value;
                onPropertyChanged();
                // onPropertyChanged("MyProperty");
            }
        }


        // klasa implementująca intrfejs INotifyPropertyChanged
        // musi mieć zadeklarowany event PropertyChanged
        // dodatkowo dodajemy metodę pomocniczą onPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // virtual - może zostać nadpisana w klasach pochodnych
        // [CallerMemberName] dzięki temu nie musimy wpisywać nazwy "MyProperty"
        protected virtual void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
