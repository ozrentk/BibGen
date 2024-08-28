//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BibGen.App.Viewmodel
//{
//    public abstract class ViewModelBase : INotifyPropertyChanged
//    {
//        public event PropertyChangedEventHandler PropertyChanged;

//        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }

//        protected bool SetProperty<T>(ref T backingField, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
//        {
//            if (EqualityComparer<T>.Default.Equals(backingField, value)) return false;
//            backingField = value;
//            OnPropertyChanged(propertyName);
//            return true;
//        }
//    }
//}
