using System;
using System.Collections.Generic;
using GroceryStore.Helpers;
using GroceryStore.Logic;
using GroceryStore.Models;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace GroceryStore.Views
{
    public partial class CouponPage : ContentPage
    {
        public CouponPage()
        {
            InitializeComponent();

            listCoupons.RefreshCommand = new Command(() =>
            {
                GetCoupons();
                listCoupons.IsRefreshing = false;
            });
            GetCoupons();
        }

#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
        private async void GetCoupons()
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        {
            try
            {
                Config.ShowDialog();
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await DisplayAlert("Alert", "Your device is not connected to internet. Please try again later.",
                        "Ok");
                }
                else
                {
                    var response = await CategoryLogic.CouponList();
                    if (response.status == 200)
                    {
                        Config.HideDialog();
                        listCoupons.ItemsSource = response.data;
                    }
                    else
                    {
                        Config.HideDialog();
                        EmptyCoupons();
                        //await DisplayAlert("Alert", response.message, "Ok");
                    }
                }
            }
            catch
            {
                Config.HideDialog();
                EmptyCoupons();
                await DisplayAlert("Alert", Config.ApiErrorMessage, "Ok");
            }
        }

        private void EmptyCoupons()
        {
            var size = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            Label emptyCoupon = new Label()
            {
                Text = ValidationMessages.EmptyCoupon,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontSize = size,
            };
            Content = emptyCoupon;
        }

        public async void ItemClicked(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                CheckoutPage.coupon = button.CommandParameter as Coupon;
                var response = await CartLogic.ApplyCoupon(Application.Current.Properties["user_id"].ToString(), CheckoutPage.coupon.id.ToString());
                if (response.status == 200)
                    await Navigation.PopAsync();
                else
                    await DisplayAlert("Alert", response.message, "Ok");
            }
            catch { }
        }
    }
}
