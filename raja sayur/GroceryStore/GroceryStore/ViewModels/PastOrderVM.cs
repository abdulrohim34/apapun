using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using static GroceryStore.Models.OrderHistory;

namespace GroceryStore.ViewModels
{
    public class PastOrderVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private PastOrder _pastOrder;

        public PastOrder PastOrder
        {
            get { return _pastOrder; }
            set { _pastOrder = value; OnPropertyChange("PastOrder"); }
        }

        private ObservableCollection<PastOrder> _pastOrderList;

        public ObservableCollection<PastOrder> PastOrderList
        {
            get { return _pastOrderList; }
            set { _pastOrderList = value; OnPropertyChange("PastOrderList"); }
        }
    }
}
