using System;
using System.ComponentModel;
using GroceryStore.Helpers;
using GroceryStore.Logic;
using Xamarin.Forms;

namespace GroceryStore.ViewModels
{
    public class BadgesVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public BadgesVM()
        {
            MessagingCenter.Subscribe<App>((App)Application.Current, "getCartCountHomeOnly", getCartCountHomeOnly);
            if (Application.Current.Properties.ContainsKey("cart_count"))
                CartCount = Application.Current.Properties["cart_count"].ToString();
            else
                CartCount = "0";
            if (Application.Current.Properties.ContainsKey("fav_count"))
                FavCount = Application.Current.Properties["fav_count"].ToString();
            else
                FavCount = "0";

            OnPropertyChanged(nameof(CartCount));
            OnPropertyChanged(nameof(FavCount));
        }

        private string _cartCount;
        public string CartCount
        {
            get { return _cartCount; }
            set { _cartCount = value; OnPropertyChanged(nameof(CartCount)); }
        }

        private string _favCount;
        public string FavCount
        {
            get { return _favCount; }
            set { _favCount = value; OnPropertyChanged(nameof(FavCount)); }
        }

        async void getCartCountHomeOnly(App app = null)
        {
            Application.Current.Properties["cart_count"] = "0";
            Application.Current.Properties["fav_count"] = "0";
            try
            {
                if (Application.Current.Properties.ContainsKey("user_id"))
                {
                    var response = await CartLogic.CartCount(Application.Current.Properties["user_id"].ToString());
                    if (response.status == 200)
                    {
                        Application.Current.Properties["cart_count"] = response.cart_count.ToString();
                        Application.Current.Properties["fav_count"] = response.fav_count.ToString();
                        CartCount = Application.Current.Properties["cart_count"].ToString();
                        FavCount = Application.Current.Properties["fav_count"].ToString();
                        OnPropertyChanged(nameof(CartCount));
                        OnPropertyChanged(nameof(FavCount));
                    }
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("BadgesVM-getCartCountHomeOnly", ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
