using GroceryStore.Models;
using GroceryStore.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace GroceryStore.ViewModels
{
    public class FavouritesVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public FavouriteCommand FavouriteCommand;

        private ObservableCollection<FavouriteProductList> favouriteProducts;

        public FavouritesVM()
        {
            FavouriteCommand = new FavouriteCommand(this);
        }

        public ObservableCollection<FavouriteProductList> FavouriteProducts
        {
            get { return favouriteProducts; }
            set { favouriteProducts = value; OnPropertyChanged("FavouriteProducts"); }
        }

        private int Id;

        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        private string ProductName;

        public string product_name
        {
            get { return ProductName; }
            set { ProductName = value; OnPropertyChanged("product_name"); }
        }

        private string BrandName;

        public string brand_name
        {
            get { return BrandName; }
            set { BrandName = value; OnPropertyChanged("brand_name"); }
        }

        private string Image;

        public string image
        {
            get { return Image; }
            set { Image = value; OnPropertyChanged("image"); }
        }

        private string Price;

        public string price
        {
            get { return Price; }
            set { Price = value; OnPropertyChanged("price"); }
        }
        
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
