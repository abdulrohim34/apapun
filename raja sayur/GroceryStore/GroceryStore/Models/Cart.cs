using GroceryStore.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace GroceryStore.Models
{
    public class Cart : INotifyPropertyChanged
    {
        public int id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public int product_quantity { get; set; }
        public int scheduled { get; set; }
        public decimal product_price { get; set; }
        private DateTime _from_date;

        public DateTime from_date
        {
            get
            {
                return _from_date;
            }
            set
            {
                _from_date = value;
                OnPropertyChanged("from_date");
                // this.updateDate(id, "from_date", from_date, Application.Current.Properties["user_id"].ToString());
            }
        }

        private DateTime _to_date;

        public DateTime to_date
        {
            get { return _to_date; }
            set
            {
                _to_date = value;
                // this.updateDate(id, "to_date", to_date,Application.Current.Properties["user_id"].ToString());
                OnPropertyChanged("to_date");
            }
        }

        private string _special_price;

        public string special_price
        {
            get { return _special_price; }
            set { _special_price = value; OnPropertyChanged("_special_price"); }
        }

        public bool IsSpecialPriceVisible => (special_price != null);
        public bool IsPriceVisible => (special_price == null);

        public string gst { get; set; }
        public string product_gst { get; set; }

        public bool gst_visible
        {
            get => (product_gst == "0.00") ? false : true;
        }

        public string product_igst { get; set; }
        public string total_without_tax { get; set; }
        public string total_with_tax { get; set; }
        public string product_name { get; set; }
        public int weight { get; set; }
        public string image { get; set; }
        public string brand_name { get; set; }
        public string unit_name { get; set; }
        public string unit_code { get; set; }
        public bool FooterIsVisible = true;
        public string GrandTotal = "Grand Total";
        private string _minimumdate;

        public string weight_unit
        {
            get
            {
                return weight + " " + unit_code;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void updateDate(int id, string type, string date, string user_id)
        {
            await CartLogic.UpdateDate(id, type, date, user_id);
        }

        public string MinimumDate
        {
            get
            {
                return DateTime.Now.AddDays(+1).ToString("MM/dd/yy");
            }
            set { _minimumdate = value; }
        }
    }

    public class UserAddressDefault
    {
        public int id { get; set; }
        public string address_type { get; set; }
        public int default_address { get; set; }
        public string full_address { get; set; }
        public string mobile_number { get; set; }
    }

    public class Data
    {
        public IList<Cart> cart_data { get; set; }
        public UserAddressDefault user_address { get; set; }
    }

    public class CartResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
        public decimal delivery_charges { get; set; }
        public decimal minimum_order_amount { get; set; }
        public string promo_amount { get; set; }
        public string total_amount { get; set; }
        public string cost { get; set; }
        public string coupon_code_id { get; set; }
        public string coupon_code { get; set; }
    }
    public class CartDateResponse
    {
        public int status { get; set; }
        public string message { get; set; }
    }

    public class AddCart
    {
        public int status { get; set; }
        public string message { get; set; }
        public string cart_count { get; set; }
        public string fav_count { get; set; }
        public IList<Cart> data { get; set; }
    }

    public class OrderResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public string cart_count { get; set; }
        public string fav_count { get; set; }
        public IList<Cart> data { get; set; }
    }

    public class OrderStatusResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public string order_id { get; set; }
        public string amount { get; set; }
        public string cart_count { get; set; }
        public string fav_count { get; set; }
        public IList<Cart> data { get; set; }
    }
}
