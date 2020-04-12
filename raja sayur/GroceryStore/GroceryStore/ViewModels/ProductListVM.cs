using GroceryStore.Models;
using GroceryStore.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GroceryStore.ViewModels
{
    public class ProductListVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get { return products; }
            set { products = value; OnPropertyChnaged("Products"); }
        }

        private Product product;

        public Product Product
        {
            get { return product; }
            set { product = value; OnPropertyChnaged("Product"); }
        }

        private string Favourite;

        public string favourite
        {
            get { return Favourite; }
            set
            {
                Favourite = value; Product = new Product()
                {
                    favourite = this.Favourite
                };
                OnPropertyChnaged("favourite");
            }
        }

        private ObservableCollection<GetProductVariation> _getProductVariations;

        public ObservableCollection<GetProductVariation> get_product_variations
        {
            get { return _getProductVariations; }
            set { _getProductVariations = value; OnPropertyChnaged("get_product_variations"); }
        }

        //private string _price;

        //public string price
        //{
        //    get { return _price; }
        //    set { _price = value; OnPropertyChnaged("price"); }
        //}

        //private int _selectedProductVariation;

        //public int SelectedProductVariation
        //{
        //    get { return 0; }
        //    set { _selectedProductVariation = 0; }
        //}

        private int _selected_variant_id;

        public int selected_variant_id
        {
            get { return _selected_variant_id; }
            set { _selected_variant_id = value; OnPropertyChnaged("selected_variant_id"); }
        }

        private void OnPropertyChnaged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _selected_variant_index;

        public int selected_variant_index
        {
            get { return _selected_variant_index; }
            set { _selected_variant_index = value; OnPropertyChnaged(nameof(selected_variant_index)); }
        }

        public ICommand VariantChangeCommand
        {
            get
            {
                return new Command(() =>
                {

                });
            }
        }

        public ICommand ItemClickCommand
        {
            get
            {
                return new Command((item) =>
                {
                    // App.Current.MainPage.Navigation.PushModalAsync(new ProductDetailPage((Product)item));
                    //App.Current.MainPage = new NavigationPage(new ProductDetailPage((Product)item));
                    //ProductsPage.NavigateToProductPage((Product)item);
                    //this.Navigate();
                });
            }
        }
    }
}
