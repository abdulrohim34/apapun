using GroceryStore.Helpers;
using GroceryStore.Models;
using GroceryStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Services;

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetailPage : ContentPage
    {
        string _pageTitle = "Delivery Detail";
        public static OrderDetailVM ViewModel;
        int _cart_id;
        string _order_type;
        public OrderDetailPage(int cart_id, string order_type)
        {
            InitializeComponent();
            _cart_id = cart_id;
            _order_type = order_type;
            ViewModel = new OrderDetailVM();
            BindingContext = ViewModel;

            MessagingCenter.Subscribe<App>((App)Application.Current, "Closed", (sender) => {
                GetOrderDeliveryDates();
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
            GetOrderDeliveryDates();
        }

        public async void GetOrderDeliveryDates()
        {

            try
            {
                Config.ShowDialog();
                var status = "";
                if (_order_type == "past")
                {
                    status = "completed";
                }
                else
                {
                    status = "";
                }
                var response = await OrderDetail.GetOrderDetail(_cart_id, status);
                if (response.status == 200)
                {
                    ViewModel.OrderDetailList = new ObservableCollection<OrderDetail>(response.data);
                    listOrder.ItemsSource = ViewModel.OrderDetailList;
                    Config.HideDialog();
                }
                else
                {
                    Config.HideDialog();
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("OrderDetailPage-GetOrderDeliveryDates", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        private async void btnReschedule_Clicked(object sender, EventArgs e)
        {
            try
            {
                var button = (StackLayout)sender;
                if (button.GestureRecognizers.Count() > 0)
                {
                    var label = (TapGestureRecognizer)button.GestureRecognizers[0];
                    var item = (OrderDetail)label.CommandParameter;

                    await PopupNavigation.Instance.PushAsync(new ReSchedulePopup(item, ViewModel));
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("OrderDetailPage-btnReschedule_Clicked", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        private void listOrder_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;

            // Optionally pause a bit to allow the preselect hint.
            Task.Delay(500);

            // Deselect the item.
            if (sender is ListView lv) lv.SelectedItem = null;
        }
    }
}