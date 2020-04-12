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
    public partial class UpcomingOrderPage : ContentPage
    {
        public UpcomingOrderVM ViewModel;
        public UpcomingOrderPage()
        {
            InitializeComponent();
            ViewModel = new UpcomingOrderVM();
            BindingContext = ViewModel;
            listUpcomingOrders.RefreshCommand = new Command(() =>
            {
                getData();
                listUpcomingOrders.IsRefreshing = false;
            });
            getData();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            listUpcomingOrders.SelectedItem = null;
        }

        async void getData()
        {
            try
            {
                if (Application.Current.Properties.ContainsKey("user_id"))
                {
                    Config.ShowDialog();
                    var response = await OrderLogic.GetUpcomingOrders(int.Parse(Application.Current.Properties["user_id"].ToString()));
                    if (response.status == 200)
                    {
                        if (response.data.Any())
                        {
                            mainContent.IsVisible = true;
                            emptyContent.IsVisible = false;
                            ViewModel.UpcomingOrderList = new ObservableCollection<OrderHistory.UpcomingOrder>(response.data);
                            Config.HideDialog();
                        }
                        else
                        {
                            Config.HideDialog();
                            EmptyOrder();
                        }
                    }
                    else
                    {
                        Config.HideDialog();
                        EmptyOrder();
                    }
                    Config.HideDialog();
                }
                else
                {
                    EmptyOrder();
                }
            }
            catch (Exception)
            {
                Config.HideDialog();
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

        private void listUpcomingOrders_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var listView = (ListView)sender;
                var item = (UpcomingOrder)listView.SelectedItem;
                if (item != null)
                {
                    Navigation.PushAsync(new OrderDetailPage(item.cart_id, "upcoming"));
                }
            }
            catch (Exception)
            {
                Config.HideDialog();
                //DisplayAlert("Alert", Config.ApiErrorMessage, "Ok");
            }
        }
    }
}