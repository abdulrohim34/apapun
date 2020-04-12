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

namespace GroceryStore.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PastOrderDetail : ContentPage
	{
        string _pageTitle = "Delivery Detail";
        public OrderDetailVM ViewModel;
        int _cart_id;
        string _order_type;
        public PastOrderDetail(int cart_id, string order_type)
        {
            InitializeComponent();
            _cart_id = cart_id;
            _order_type = order_type;
            ViewModel = new OrderDetailVM();
            BindingContext = ViewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
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
                Config.ErrorStore("PastOrderDetail-OnAppearing", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }
    }
}