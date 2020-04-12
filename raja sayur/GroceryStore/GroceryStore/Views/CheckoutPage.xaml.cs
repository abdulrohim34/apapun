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

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckoutPage : ContentPage
    {
        string _pageTitle = "Checkout";
        private CartResponse _cart;
        private static int _address_id;
        private static string _default_address;
        private static string _full_address;
        private static string _address_type;
        public static Coupon coupon;
        public string CouponCodeId = null;
        public CheckoutPage()
        {
            InitializeComponent();

            listProducts.RefreshCommand = new Command(() =>
            {
                getData();
                listProducts.IsRefreshing = false;
            });
            //getData();
            coupon = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CustomNavigationBarVM.PageName = "checkout";
            CustomNavigationBarVM.MenuIcon = "back.png";
            MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
            getData();
            if (CheckoutPage.coupon == null)
            {
                IsCodeVisible.IsVisible = false;
                IsApplyVisible.IsVisible = true;
            }
            else
            {
                Code.Text = coupon.coupon_code;
                IsCodeVisible.IsVisible = true;
                IsApplyVisible.IsVisible = false;
            }
        }

        private void ApplyCouponCode_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CouponPage());
        }

        private async void RemoveCoupon_Tapped(object sender, EventArgs e)
        {
            try
            {
                string CouponId = null;
                if (CheckoutPage.coupon != null)
                {
                    CouponId = CheckoutPage.coupon.id.ToString();
                }
                if (!string.IsNullOrEmpty(CouponCodeId))
                {
                    CouponId = CouponCodeId;
                }

                Config.ShowDialog();
                var response = await CartLogic.RemoveCoupon(Application.Current.Properties["user_id"].ToString(), CouponId);
                if (response.status == 200)
                {
                    Config.HideDialog();
                    getData();
                    //Config.SnackbarMessage(response.message);
                }
                else
                {
                    Config.HideDialog();
                    //Config.ErrorSnackbarMessage(response.message);
                }

            }
            catch
            {
                Config.HideDialog();
            }
        }

        public static void SetAddress(int id, string full_address, string address_type, string default_address)
        {
            _address_id = id;
            _default_address = default_address;
            _full_address = full_address;
            _address_type = address_type;
        }

        async void getData()
        {
            try
            {
                if (Application.Current.Properties.ContainsKey("user_id"))
                {
                    Config.ShowDialog();
                    var cartItems =
                        await CartLogic.GetCartItems(int.Parse(Application.Current.Properties["user_id"].ToString()));
                    if (cartItems.status == 200)
                    {
                        if (!string.IsNullOrEmpty(cartItems.coupon_code_id))
                            CouponCodeId = cartItems.coupon_code_id;
                        if (string.IsNullOrEmpty(cartItems.coupon_code_id))
                        {
                            IsCodeVisible.IsVisible = false;
                            IsApplyVisible.IsVisible = true;
                        }
                        else
                        {
                            IsCodeVisible.IsVisible = true;
                            IsApplyVisible.IsVisible = false;
                            Code.Text = cartItems.coupon_code;
                        }
                        _cart = cartItems;
                        listProducts.ItemsSource = cartItems.data.cart_data;

                        if (_cart.data.user_address == null && _address_id == 0)
                        {
                            addAddress.IsVisible = false;
                        }
                        else
                        {
                            addAddress.IsVisible = true;
                            if (_address_id == 0)
                            {
                                _address_id = cartItems.data.user_address.id;
                                _full_address = cartItems.data.user_address.full_address;
                                _address_type = cartItems.data.user_address.address_type;
                                _default_address = cartItems.data.user_address.default_address == 0 ? "" : " (Default)";
                            }
                            addAddress.Text = _address_id != 0 ? _full_address : cartItems.data.user_address.full_address;
                        }
                        item_total.Text = "Rp " + cartItems.total_amount;
                        delivery_fee.Text = "Rp " + Convert.ToDecimal(cartItems.delivery_charges);
                        to_pay.Text = "Rp " + cartItems.cost;
                        total_discount.Text = "- Rp " + cartItems.promo_amount;
                        Config.HideDialog();
                        if (string.IsNullOrEmpty(cartItems.coupon_code_id))
                            discount_layout.IsVisible = false;
                        else
                            discount_layout.IsVisible = true;
                    }
                    else
                    {
                        EmptyCart();
                    }
                }
                else
                {
                    EmptyCart();
                }
                Config.HideDialog();
            }
            catch (Exception ex)
            {
                Config.ErrorStore("CheckoutPage-getData", ex.Message);
                Config.HideDialog();
                EmptyCart();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        void EmptyCart()
        {
            var size = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            Label emptyCart = new Label()
            {
                Text = "Empty Cart",
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontSize = size,
            };
            Content = emptyCart;
        }

        private async void placeOrderTap_Tapped(object sender, EventArgs e)
        {
            try
            {
                if ((_cart.data.user_address == null && _address_id.ToString() == null) || _address_id.ToString() == "0")
                {
                    await DisplayAlert("Alert", Message.addressBlank, "Ok");
                    return;
                }
                else
                {
                    Config.ShowDialog();
                    Dictionary<string, string> valuePairs = new Dictionary<string, string>();
                    valuePairs.Add("address_id", _address_id.ToString());
                    if (PaymentMode.SelectedIndex == 0)
                    {
                        valuePairs.Add("payment_method", "cod");
                    }
                    else
                    {
                        valuePairs.Add("payment_method", "online");
                    }
                    valuePairs.Add("coupon_code_id", CouponCodeId);
                    valuePairs.Add("user_id", Application.Current.Properties["user_id"].ToString());
                    var response = await CartLogic.PlaceOrder(valuePairs);
                    if (response.status == 200)
                    {
                        HomeVM.MyCartCounter = response.cart_count;
                        MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
                        //var accepted = await DisplayAlert("Success", response.message, "Ok","cancel");
                        //if (accepted || !accepted)
                        //{
                        //    //var existingPages = Navigation.NavigationStack.ToList();
                        //    //foreach (var page in existingPages)
                        //    //{
                        //    //    Navigation.RemovePage(page);
                        //    //}
                        //    await Navigation.PushAsync(new Home());
                        //}
                        Config.HideDialog();
                        if (PaymentMode.SelectedIndex == 0)
                        {
                            await Navigation.PushModalAsync(new OrderSuccessPage());
                        }
                        else
                        {
                            string url = String.Format("{0}/paypalConfigure?order_id={1}&amount={2}&user_id={3}&device_type={4}", Config.ApiUrl, response.order_id, response.amount, Application.Current.Properties["user_id"].ToString(), Application.Current.Properties["device_type"].ToString());
                            var browser = new WebView();
                            browser.Source = url;
                            Content = browser;
                            browser.Navigating += Browser_Navigating;
                            browser.Navigated += Browser_Navigated;
                        }
                    }
                    else
                    {
                        Config.HideDialog();
                        Config.ErrorSnackbarMessage(response.message);
                    }
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("CheckoutPage-placeOrderTap_Tapped", ex.Message);
                Config.HideDialog();
                EmptyCart();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        private void Browser_Navigated(object sender, WebNavigatedEventArgs e)
        {
            Config.HideDialog();
        }

        private void Browser_Navigating(object sender, WebNavigatingEventArgs e)
        {
            Config.ShowDialog();
        }
        private void changeAddressTap_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ManageAddressPage(true));
        }
    }
}