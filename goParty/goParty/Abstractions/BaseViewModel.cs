using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace goParty.Abstractions
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        string _propTitle = string.Empty;
        bool _propIsBusy;
        public View parent;
        public string Title
        {
            get { return _propTitle; }
            set { SetProperty(ref _propTitle, value, "Title"); }
        }

        public bool IsBusy
        {
            get { return _propIsBusy; }
            set { SetProperty(ref _propIsBusy, value, "IsBusy"); }
        }

        protected void SetProperty<T>(ref T store, T value, string propName, Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(store, value))
                return;
            store = value;
            if (onChanged != null)
                onChanged();
            OnPropertyChanged(propName);
        }

        

        public void OnPropertyChanged(string propName)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }


        Command backIconClickedCmd;
        public Command BackIconClickedCommand => backIconClickedCmd ?? (backIconClickedCmd = new Command(() => ExecuteBackButtonClickedCommand()));

        public virtual void ExecuteBackButtonClickedCommand()
        {
            if (parent == null)
                throw new NotImplementedException();
            Tapped(parent, 1);
        }

        public delegate void TappedEventHandler(object sender, float e);
        public event TappedEventHandler Tapped;

    }
}
