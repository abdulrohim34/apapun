using GroceryStore.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace GroceryStore.ViewModels
{
    public class NotificationVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Notifications> _notifications;

        public ObservableCollection<Notifications> NotificationList
        {
            get { return _notifications; }
            set { _notifications = value; OnPropertyChange("NotificationList"); }
        }

    }
}
