using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GroceryStore.Models;

namespace GroceryStore.ViewModels
{
    public class CartVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ObservableCollection<Cart> _CartItems;
        public ObservableCollection<Cart> CartItems
        {
            get { return _CartItems; }
            set { _CartItems = value; OnPropertyChanged("CartItems"); }
        }

        private Cart _Cart;
        public Cart Cart
        {
            get { return _Cart; }
            set { _Cart = value; OnPropertyChanged("Cart"); }
        }

        private int _id;
        public int id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("id"); }
        }

        private int _product_id;
        public int product_id
        {
            get { return _product_id; }
            set { _product_id = value; OnPropertyChanged("product_id"); }
        }

        private int _quantity;
        public int quantity
        {
            get { return _quantity; }
            set { _quantity = value; OnPropertyChanged("quantity"); }
        }

        private int _product_quantity;
        public int product_quantity
        {
            get { return _product_quantity; }
            set { _product_quantity = value; OnPropertyChanged("product_quantity"); }
        }

        private int _scheduled;
        public int scheduled
        {
            get { return _scheduled; }
            set { _scheduled = value; OnPropertyChanged("scheduled"); }
        }

        private string _from_date;
        public string from_date
        {
            get { return _from_date; }
            set { _from_date = value; OnPropertyChanged("from_date"); }
        }

        private string _to_date { get; set; }
        public string to_date
        {
            get { return _to_date; }
            set { _to_date = value; OnPropertyChanged("to_date"); }
        }

        private string _gst { get; set; }
        public string gst
        {
            get { return _gst; }
            set { _gst = value; OnPropertyChanged("gst"); }
        }

        private string _product_gst { get; set; }
        public string product_gst
        {
            get { return _product_gst; }
            set { _product_gst = value; OnPropertyChanged("product_gst"); }
        }

        private string _product_igst { get; set; }
        public string product_igst
        {
            get { return _product_igst; }
            set { _product_igst = value; OnPropertyChanged("product_igst"); }
        }

        private string _total_without_tax { get; set; }
        public string total_without_tax
        {
            get { return _total_without_tax; }
            set { _total_without_tax = value; OnPropertyChanged("total_without_tax"); }
        }

        private string _total_with_tax { get; set; }
        public string total_with_tax
        {
            get { return _total_with_tax; }
            set { _total_with_tax = value; OnPropertyChanged("total_with_tax"); }
        }

        private string _product_name { get; set; }
        public string product_name
        {
            get { return _product_name; }
            set { _product_name = value; OnPropertyChanged("product_name"); }
        }

        private int _weight { get; set; }
        public int weight
        {
            get { return _weight; }
            set { _weight = value; OnPropertyChanged("weight"); }
        }

        private string _image { get; set; }
        public string image
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged("image"); }
        }

        private string _brand_name { get; set; }
        public string brand_name
        {
            get { return _brand_name; }
            set { _brand_name = value; OnPropertyChanged("brand_name"); }
        }

        private string unit_name { get; set; }
        public string _unit_name
        {
            get { return _unit_name; }
            set { _unit_name = value; OnPropertyChanged("unit_name"); }
        }

        private string _unit_code { get; set; }
        public string unit_code
        {
            get { return _unit_code; }
            set { _unit_code = value; OnPropertyChanged("unit_code"); }
        }
    }
}