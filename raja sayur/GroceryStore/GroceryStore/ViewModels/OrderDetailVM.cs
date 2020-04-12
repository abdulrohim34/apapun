using GroceryStore.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace GroceryStore.ViewModels
{
    public class OrderDetailVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<OrderDetail> _orderDetail;

        public ObservableCollection<OrderDetail> OrderDetailList
        {
            get { return _orderDetail; }
            set { _orderDetail = value; OnPropertyChange("OrderDetail"); }
        }

        private OrderDetailResponse _orderDetailResponse;

        public OrderDetailResponse OrderDetailResponse
        {
            get { return _orderDetailResponse; }
            set { _orderDetailResponse = value; OnPropertyChange("OrderDetailResponse"); }
        }

    }
}
