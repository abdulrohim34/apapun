using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryStore.Helpers;
using GroceryStore.Logic;
using GroceryStore.Models;
using GroceryStore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static GroceryStore.Models.OrderHistory;

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PastOrderPage : ContentPage
    {
        public ObservableCollection<Product> products;
        public PastOrderVM ViewModel;
        public PastOrderPage()
        {
            InitializeComponent();
            ViewModel = new PastOrderVM();
            BindingContext = ViewModel;
            listPastOrders.RefreshCommand = new Command(() =>
            {
                getData();
                listPastOrders.IsRefreshing = false;
            });
            getData();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            listPastOrders.SelectedItem = null;
        }

        async void getData()
        {
            try
            {
                if (Application.Current.Properties.ContainsKey("user_id"))
                {
                    Config.ShowDialog();
                    var response = await OrderLogic.GetPastOrders(int.Parse(Application.Current.Properties["user_id"].ToString()));
                    if (response.status == 200)
                    {
                        if (response.data.Any())
                        {
                            mainContent.IsVisible = true;
                            emptyContent.IsVisible = false;
                            ViewModel.PastOrderList = new ObservableCollection<OrderHistory.PastOrder>(response.data);
                        }
                        else
                        {
                            EmptyOrder();
                        }
                    }
                    else
                    {
                        EmptyOrder();
                    }
                    Config.HideDialog();
                }
                else
                {
                    EmptyOrder();
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("PastOrderPage-getData", ex.Message);
                Config.HideDialog();
                EmptyOrder();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        void EmptyOrder()
        {
            Config.HideDialog();
            mainContent.IsVisible = false;
            emptyContent.IsVisible = true;
            //var size = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            //Label emptyCart = new Label()
            //{
            //    Text = ValidationMessages.EmptyCart,
            //    FontAttributes = FontAttributes.Bold,
            //    HorizontalOptions = LayoutOptions.CenterAndExpand,
            //    VerticalOptions = LayoutOptions.CenterAndExpand,
            //    FontSize = size,
            //};
            //Content = emptyCart;
        }

        private void listPastOrders_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var listView = (ListView)sender;
                var item = (PastOrder)listView.SelectedItem;
                if (item != null)
                {
                    Navigation.PushAsync(new PastOrderDetail(item.cart_id, "past"));
                }
            }
            catch (Exception)
            {
                Config.HideDialog();
                //Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
                //DisplayAlert("Alert", Config.ApiErrorMessage, "Ok");
            }
        }

        private async void btnOrder_Clicked(object sender, EventArgs e)
        {
            try
            {
                var button = (StackLayout)sender;
                if (button.GestureRecognizers.Count() > 0)
                {
                    Config.ShowDialog();
                    var label = (TapGestureRecognizer)button.GestureRecognizers[0];
                    var product = (PastOrder)label.CommandParameter;

                    Dictionary<string, string> addToCart = new Dictionary<string, string>();
                    addToCart.Add("product_id", product.product_id.ToString());
                    addToCart.Add("user_id", Application.Current.Properties["user_id"].ToString());
                    addToCart.Add("quantity", product.quantity.ToString());
                    addToCart.Add("scheduled", "0");
                    addToCart.Add("product_variation_id", product.product_variation_id);
                    addToCart.Add("from_date", DateTime.Now.ToString("yyyy-MM-dd"));
                    addToCart.Add("to_date", DateTime.Now.ToString("yyyy-MM-dd"));
                    var response = await CartLogic.AddToCart(addToCart);
                    if (response.status == 200)
                    {
                        Config.HideDialog();
                        HomeVM.MyCartCounter = response.cart_count;
                        MessagingCenter.Send((App)Application.Current, "NavigationBar", "");
                        Config.SnackbarMessage(response.message);
                    }
                    else
                    {
                        Config.HideDialog();
                        Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("PastOrderPage-btnOrder_Clicked", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }
    }
}