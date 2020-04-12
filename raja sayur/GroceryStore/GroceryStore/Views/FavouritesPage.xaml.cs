using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryStore.ContentView;
using GroceryStore.Helpers;
using GroceryStore.Logic;
using GroceryStore.Models;
using GroceryStore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavouritesPage : ContentPage
    {
        string _pageTitle = "Favourites";
        public FavouritesVM ViewModel;
        public FavouritesPage()
        {
            InitializeComponent();

            ViewModel = new FavouritesVM();
            BindingContext = ViewModel;
            Title = "Favourites";
            listFavourites.RefreshCommand = new Command(() =>
            {
                getData();
                listFavourites.IsRefreshing = false;
            });
            getData();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (CustomNavigationBarVM.PageName == "from_drawer")
            {
                CustomNavigationBarVM.MenuIcon = "menu.png";
            }
            else
            {
                CustomNavigationBarVM.MenuIcon = "back.png";
            }
            MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
        }

        async void getData()
        {
            try
            {
                if (Application.Current.Properties.ContainsKey("user_id"))
                {
                    Config.ShowDialog();
                    var response = await ProductLogic.GetFavouriteProducts(int.Parse(Application.Current.Properties["user_id"].ToString()));
                    if (response.status == 200)
                    {
                        Config.HideDialog();
                        HomeVM.MyFavCounter = response.fav_count;
                        MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
                        if (response.data != null)
                        {
                            emptyContent.IsVisible = false;
                            mainContent.IsVisible = true;
                            ViewModel.FavouriteProducts = new ObservableCollection<FavouriteProductList>(response.data.ToList());
                        }
                        else
                        {
                            EmptyFavouriteProducts();
                        }
                    }
                    else
                    {
                        Config.HideDialog();
                        EmptyFavouriteProducts();
                    }
                }
                else
                {
                    EmptyFavouriteProducts();
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("FavouritePage-getData", ex.Message);
                Config.HideDialog();
                EmptyFavouriteProducts();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        void EmptyFavouriteProducts()
        {
            Config.HideDialog();
            mainContent.IsVisible = false;
            emptyContent.IsVisible = true;
            //emptyLabel.Text = ValidationMessages.EmptyFavouriteProducts;
            //var size = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            //emptyLabel.FontSize = size;
            //Label emptyCart = new Label()
            //{
            //    Text = ValidationMessages.EmptyFavouriteProducts,
            //    FontAttributes = FontAttributes.Bold,
            //    HorizontalOptions = LayoutOptions.CenterAndExpand,
            //    VerticalOptions = LayoutOptions.CenterAndExpand,
            //    FontSize = size,
            //};
            //Content = emptyCart;
        }

        public void DeleteClicked(object sender, EventArgs e)
        {
            try
            {
                var deleteImage = (Image)sender;
                if (deleteImage.GestureRecognizers.Count > 0)
                {
                    var gesture = (TapGestureRecognizer)deleteImage.GestureRecognizers[0];
                    string ProductId = gesture.CommandParameter.ToString();
                    DeleteFavProduct(ProductId);
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("FavouritePage-DeleteClicked", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        void DeleteFavProductAction(object sender, System.EventArgs e)
        {
            var menu = (MenuItem)sender;
            string ProductId = menu.CommandParameter.ToString();
            DeleteFavProduct(ProductId);
        }

        async void DeleteFavProduct(string productId)
        {
            try
            {
                Config.ShowDialog();
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("product_id", productId);
                data.Add("user_id", Application.Current.Properties["user_id"].ToString());
                var response = await ProductLogic.DeleteFavouriteProduct(data);
                if (response.status == 200)
                {
                    Config.HideDialog();
                    HomeVM.MyFavCounter = response.fav_count;
                    MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
                    ViewModel.FavouriteProducts.Remove(ViewModel.FavouriteProducts.Where(p => p.id == int.Parse(productId)).Single());
                    Config.SnackbarMessage(response.message);
                    if (!response.data.Any())
                    {
                        Config.HideDialog();
                        EmptyFavouriteProducts();
                    }
                    Config.HideDialog();
                }
                else
                {
                    Config.HideDialog();
                    Config.SnackbarMessage(response.message);
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("FavouritePage-DeleteFavProduct", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }
    }
}