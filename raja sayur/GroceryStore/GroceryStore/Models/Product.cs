using GroceryStore.Helpers;
using GroceryStore.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Models
{
    public class ProductUnits
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public partial class GetProductVariation : INotifyPropertyChanged
    {
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

        private int _unit_id;

        public int unit_id
        {
            get { return _unit_id; }
            set { _unit_id = value; OnPropertyChanged("unit_id"); }
        }

        private int _weight;

        public int weight
        {
            get { return _weight; }
            set { _weight = value; OnPropertyChanged("weight"); }
        }

        private string _price;

        public string price
        {
            get { return _price; }
            set { _price = value; OnPropertyChanged("price"); }
        }

        private string _special_price;

        public string special_price
        {
            get { return _special_price; }
            set { _special_price = value; OnPropertyChanged("_special_price"); }
        }

        public bool IsSpecialPriceVisible => (special_price != null);
        public bool IsCutPriceVisible => (special_price != null);

        private int _selected_index;

        public int selected_index
        {
            get { return _selected_index; }
            set
            {
                _selected_index = value;
                OnPropertyChanged("selected_index");
            }
        }

        public string weightUnit
        {
            get { return weight + " " + product_units.name; }
        }
        public int selectedWeight
        {
            get { return 0; }
        }

        private string _status;

        public string status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged("status"); }
        }

        private ProductUnits _product_units;

        public ProductUnits product_units
        {
            get { return _product_units; }
            set { _product_units = value; OnPropertyChanged("product_units"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    public class ProductBrand
    {
        public int id { get; set; }
        public string brand_name { get; set; }
    }

    public class Product : INotifyPropertyChanged
    {
        public int id { get; set; }
        public int category_id { get; set; }
        public int brand_id { get; set; }
        public string name { get; set; }
        public string category_name { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        private string _price;

        public string price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged("price");
            }
        }

        private string _special_price;

        public string special_price
        {
            get { return _special_price; }
            set { _special_price = value; OnPropertyChanged("_special_price"); }
        }

        private string _discount;

        public string discount
        {
            get { return _discount; }
            set { _discount = value; OnPropertyChanged("_discount"); }
        }

        public bool IsSpecialPriceVisible => (special_price != null);
        public bool IsPriceVisible => (special_price == null);

        public int quantity { get; set; }
        public int cart_quantity { get; set; }
        public bool is_add_cart_visible => (cart_quantity == 0);
        public bool is_quantity_visible => !is_add_cart_visible;
        public string unit { get; set; }
        public string weight { get; set; }
        public string unit_weight => weight + " " + unit;
        public string cgst { get; set; }
        public string sgst { get; set; }
        public string igst { get; set; }
        public string status { get; set; }
        public string favourite { get; set; }
        public int checkPickerLoad { get; set; }
        public int SecondLoad { get; set; }
        private int _selected_index;

        public int selected_index
        {
            get
            {
                if (checkPickerLoad == 0) { return 0; }
                else
                {
                    if (_selected_index < 0)
                        return 0;
                    else
                        return _selected_index;
                }
            }
            set { _selected_index = value; OnPropertyChanged("selected_index"); }
        }

        private int _selected_variant_id;

        public int selected_variant_id
        {
            get { return _selected_variant_id; }
            set { _selected_variant_id = value; OnPropertyChanged("selected_variant_id"); }
        }

        private IList<GetProductVariation> _get_product_variations;

        public IList<GetProductVariation> get_product_variations
        {
            get { return _get_product_variations; }
            set { _get_product_variations = value; OnPropertyChanged("get_product_variations"); }
        }

        public ProductBrand product_brand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ProductResponce
    {
        public int status { get; set; }
        public string message { get; set; }
        public IList<Product> data { get; set; }
    }

    public class SearchProductResponce
    {
        public int status { get; set; }
        public string message { get; set; }
        public IList<Product> data { get; set; }
    }

    public class FavouriteProductResponce
    {
        public int status { get; set; }
        public string message { get; set; }
        public string fav_count { get; set; }
        public string favourite_status { get; set; }
    }

    public class DeleteFavouriteProductResponce
    {
        public int status { get; set; }
        public string message { get; set; }
        public string fav_count { get; set; }
        public IList<FavouriteProductList> data { get; set; }
    }

    public class ProductDetailResponce
    {
        public int status { get; set; }
        public string message { get; set; }
        public Product data { get; set; }
    }

    public class FavouriteProductList
    {
        public int id { get; set; }
        public string product_name { get; set; }
        public string brand_name { get; set; }
        public string image { get; set; }
        public string price { get; set; }
        public string fav_count { get; set; }
    }

    public class FavouriteProductListResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public string fav_count { get; set; }
        public IList<FavouriteProductList> data { get; set; }
    }
}
