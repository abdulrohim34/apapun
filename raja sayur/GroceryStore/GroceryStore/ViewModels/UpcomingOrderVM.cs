using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using static GroceryStore.Models.OrderHistory;

namespace GroceryStore.ViewModels
{
    public class UpcomingOrderVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private UpcomingOrder _upcomingOrder;

        public UpcomingOrder UpcomingOrder
        {
            get { return _upcomingOrder; }
            set { _upcomingOrder = value; OnPropertyChange("UpcomingOrder"); }
        }

        private ObservableCollection<UpcomingOrder> _upcomingOrderList;

        public ObservableCollection<UpcomingOrder> UpcomingOrderList
        {
            get { return _upcomingOrderList; }
            set { _upcomingOrderList = value; OnPropertyChange("UpcomingOrderList"); }
        }
    }
}
