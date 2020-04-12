using GroceryStore.Helpers;
using GroceryStore.Logic;
using GroceryStore.Models;
using GroceryStore.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReSchedulePopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public OrderDetailVM ViewModel;
        OrderDetail Item;
        public ReSchedulePopup(OrderDetail _Item, OrderDetailVM _ViewModel)
        {
            InitializeComponent();
            Item = _Item;
            ViewModel = _ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var result = ViewModel.OrderDetailList.OrderByDescending(d => d.order_date).FirstOrDefault();
            DateTime oDate = Convert.ToDateTime(result.order_date);
            rescheduleDatePicker.MinimumDate = oDate.AddDays(+1);
            rescheduleDatePicker.MaximumDate = oDate.AddDays(+15);
            //DateTime.Now.AddDays(+1).ToString("MM/dd/yy");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send((App)Application.Current, "Closed");
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                Config.ShowDialog();
                Dictionary<string, string> valuePairs = new Dictionary<string, string>();
                valuePairs.Add("order_delivery_id", Item.id.ToString());
                valuePairs.Add("date", rescheduleDatePicker.Date.ToString());
                valuePairs.Add("cart_id", Item.cart_id.ToString());
                var response = await CartLogic.RescheduleOrderItem(valuePairs);
                if (response.status == 200)
                {
                    Config.HideDialog();
                    OrderDetailPage.ViewModel.OrderDetailList = new ObservableCollection<OrderDetail>(response.data.ToList());
                    await PopupNavigation.Instance.PopAsync();
                }
                else
                {
                    Config.HideDialog();
                    Config.SnackbarMessage(response.message);
                }
                //var item = reasonsList.SelectedItem as Reason;
                //var response = await DeliveryStatus.UpdateDeliveryStatus(_OrderDeliveryId.ToString(), "FL", item.id.ToString());
                //if (response.status == 200)
                //{
                //    Config.HideDialog();
                //    Config.SnackbarMessage(response.message);
                //    CurrentOrderDetailPage.ViewModel.CurrentOrderDetailList.Remove(CurrentOrderDetailPage.ViewModel.CurrentOrderDetailList.Where(a => a.order_delivery_id == _OrderDeliveryId).Single());
                //    if (CurrentOrderDetailPage.ViewModel.CurrentOrderDetailList.Count == 0)
                //    {
                //        await PopupNavigation.Instance.PopAsync(true);
                //        Application.Current.MainPage = new MasterTemplate();
                //    }
                //    else
                //    {
                //        await PopupNavigation.Instance.PopAsync(true);
                //    }
                //}
                //else
                //{
                //    Config.HideDialog();
                //    Config.SnackbarMessage(response.message);
                //}
            }
            catch (Exception ex)
            {
                Config.ErrorStore("ReSchedulePopup-Button_Clicked", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }
    }
}