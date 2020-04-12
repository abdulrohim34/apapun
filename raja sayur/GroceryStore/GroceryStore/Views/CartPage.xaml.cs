using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GroceryStore.Views;
using Rg.Plugins.Popup.Extensions;
using GroceryStore.Controls;
using GroceryStore.Helpers;
using GroceryStore.Logic;
using GroceryStore.Models;
using GroceryStore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FFImageLoading.Forms;

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage : ContentPage
    {
        //public ObservableCollection<Product> products;
        string _pageTitle = "My Cart";
        decimal minOrderAmount = 0;
        public decimal TotalLimit { get; set; }
        public CartVM ViewModel;
        public CartPage()
        {
            InitializeComponent();
            Title = _pageTitle;
            ViewModel = new CartVM();
            BindingContext = ViewModel;
            listProducts.RefreshCommand = new Command(() =>
            {
                getData();
                listProducts.IsRefreshing = false;
            });
            getData();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CustomNavigationBarVM.PageName = "cart";
            CustomNavigationBarVM.MenuIcon = "back.png";
            MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
            MessagingCenter.Subscribe<App>((App)Application.Current, "refreshCartData", getData);
            //getData();
        }

        async void getData(App app = null)
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
                        emptyContent.IsVisible = false;
                        mainContent.IsVisible = true;
                        ViewModel.CartItems = new ObservableCollection<Cart>(cartItems.data.cart_data);
                        decimal total = 0;
                        int itemQuantity = 0;
                        foreach (var item in cartItems.data.cart_data)
                        {
                            total += decimal.Parse(item.total_without_tax);
                            itemQuantity += 1;
                        }
                        //t_total.Text = "Rp " + total;
                        decimal myTotal = (Convert.ToDecimal(cartItems.delivery_charges) + Convert.ToDecimal(total));
                        t_quantity.Text = (itemQuantity == 1) ? itemQuantity + " Item" : itemQuantity + " Items";
                        //t_delivery.Text = "Rp " + Convert.ToDecimal(cartItems.delivery_charges);
                        m_total.Text = "Rp " + total;
                        b_total.Text = "Rp " + total;
                        TotalLimit = total;
                        minOrderAmount = cartItems.minimum_order_amount;
                        //t_total.Text = "Rp " + myTotal;
                        //l_grand_total.Text = "Grand Total";
                        //b_quantity.Text = itemQuantity + " Items";
                        if (cartItems.data.cart_data.Any())
                        {
                            b_cart.IsVisible = true;
                            //header.IsVisible = true;
                        }
                        Config.HideDialog();
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
            }
            catch (Exception ex)
            {
                Config.HideDialog();
                EmptyCart();
                Config.ErrorStore("CartPage-getData", ex.Message);
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
                //await DisplayAlert("Alert", Config.ApiErrorMessage, "Ok");
            }
        }

        void EmptyCart()
        {
            Config.HideDialog();
            mainContent.IsVisible = false;
            emptyContent.IsVisible = true;
            //emptyLabel.Text = ValidationMessages.EmptyCart;
            //var size = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            //emptyLabel.FontSize = size;
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

        public async void updateDate(int id, string type, string date, string user_id)
        {
            await CartLogic.UpdateDate(id, type, date, user_id);
        }

        private void checkoutTap_Tapped(object sender, EventArgs e)
        {
            if (TotalLimit >= minOrderAmount)
            {
                CustomNavigationBarVM.MenuIcon = "back.png";
                Navigation.PushAsync(new CheckoutPage());
            }
            else
                Config.ErrorSnackbarMessage(ValidationMessages.TotalFixed + minOrderAmount);
        }

        private void deleteTap_Tapped(object sender, EventArgs e)
        {
            try
            {
                var image = (CachedImage)sender;
                if (image.GestureRecognizers.Count > 0)
                {
                    var tap = (TapGestureRecognizer)image.GestureRecognizers[0];
                    var Item = (Cart)tap.CommandParameter;
                    DeleteProduct(Item);
                }
            }
            catch (Exception)
            {
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        void DeleteProduct_Action(object sender, System.EventArgs e)
        {
            var menu = (MenuItem)sender;
            var cart = (Cart)menu.CommandParameter;
            DeleteProduct(cart);
        }

        async void DeleteProduct(Cart Item)
        {
            try
            {
                Config.ShowDialog();
                Dictionary<string, int> CartItem = new Dictionary<string, int>();
                CartItem.Add("cart_id", Item.id);
                CartItem.Add("product_id", Item.product_id);
                CartItem.Add("user_id", int.Parse(Application.Current.Properties["user_id"].ToString()));

                var response = await CartLogic.DeleteCartItem(CartItem);
                if (response.status == 200)
                {
                    grandTotalVisible.IsVisible = true;
                    HomeVM.MyCartCounter = response.cart_count;
                    MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
                    Config.HideDialog();
                    var removeItem = ViewModel.CartItems.Where(p => p.product_id == Item.product_id && p.id == Item.id).Single();
                    ViewModel.CartItems.Remove(removeItem);

                    decimal total = 0;
                    int itemQuantity = 0;
                    foreach (var item in response.data)
                    {
                        total += decimal.Parse(item.total_without_tax);
                        itemQuantity += 1;
                    }
                    //t_total.Text = "Rp " + total;
                    t_quantity.Text = itemQuantity + " Items";
                    m_total.Text = "Rp " + total;
                    b_total.Text = "Rp " + total;
                    TotalLimit = total;
                    //b_quantity.Text = itemQuantity + " Items";
                    if (response.data.Any())
                    {
                        grandTotalVisible.IsVisible = true;
                        b_cart.IsVisible = true;
                        //header.IsVisible = true;
                    }

                    if (ViewModel.CartItems.Count == 0)
                    {
                        EmptyCart();
                    }
                }
                else if (response.status == 220)
                {
                    HomeVM.MyCartCounter = response.cart_count;
                    MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
                    EmptyCart();
                }
                else
                {
                    HomeVM.MyCartCounter = response.cart_count;
                    EmptyCart();
                }
                Config.HideDialog();
                //Config.SnackbarMessage(response.message);

            }
            catch (Exception ex)
            {
                Config.ErrorStore("CartPage-DeleteProduct", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        private async void decreseQuantity_Tapped(object sender, EventArgs e)
        {
            try
            {

                var image = (Frame)sender;
                if (image.GestureRecognizers.Count > 0)
                {
                    var tapGesture = (TapGestureRecognizer)image.GestureRecognizers[0];
                    //int quantityItem = int.Parse(quantity.Text);
                    //var button = (ImageButton)sender;
                    var Item = (Cart)tapGesture.CommandParameter;
                    int quantityItem = Item.quantity;
                    if (quantityItem > 1)
                    {
                        Config.ShowDialog();
                        quantityItem -= 1;
                        Dictionary<string, string> CartItem = new Dictionary<string, string>();
                        CartItem.Add("cart_id", Item.id.ToString());
                        CartItem.Add("product_id", Item.product_id.ToString());
                        CartItem.Add("quantity", quantityItem.ToString());
                        CartItem.Add("update_type", "quantity");
                        CartItem.Add("user_id", Application.Current.Properties["user_id"].ToString());

                        var response = await CartLogic.UpdateCart(CartItem);
                        if (response.status == 200)
                        {
                            HomeVM.MyCartCounter = response.cart_count;
                            MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
                            ViewModel.CartItems = new ObservableCollection<Cart>(response.data);
                            decimal total = 0;
                            int itemQuantity = 0;
                            foreach (var item in response.data)
                            {
                                total += decimal.Parse(item.total_without_tax);
                                itemQuantity += 1;
                            }
                            //t_total.Text = "Rp " + total;
                            t_quantity.Text = itemQuantity + " Items";
                            m_total.Text = "Rp " + total;
                            b_total.Text = "Rp " + total;
                            TotalLimit = total;
                            //b_quantity.Text = itemQuantity + " Items";
                            if (response.data.Any())
                            {
                                b_cart.IsVisible = true;
                                //header.IsVisible = true;
                            }
                        }
                        else
                        {
                            Config.SnackbarMessage(response.message);
                        }
                        Config.HideDialog();
                    }
                    else
                    {
                        DeleteProduct(Item);
                    }
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("CartPage-decreseQuantity_Clicked", ex.Message);
                Config.HideDialog();
            }
        }

        private async void increseQuantity_Tapped(object sender, EventArgs e)
        {
            try
            {
                var image = (Frame)sender;
                if (image.GestureRecognizers.Count > 0)
                {
                    var tapGesture = (TapGestureRecognizer)image.GestureRecognizers[0];
                    var Item = (Cart)tapGesture.CommandParameter;

                    int quantityItem = Item.quantity;
                    if (quantityItem >= 1 && quantityItem < Item.product_quantity)
                    {
                        Config.ShowDialog();
                        quantityItem += 1;
                        Dictionary<string, string> CartItem = new Dictionary<string, string>();
                        CartItem.Add("cart_id", Item.id.ToString());
                        CartItem.Add("product_id", Item.product_id.ToString());
                        CartItem.Add("quantity", quantityItem.ToString());
                        CartItem.Add("update_type", "quantity");
                        CartItem.Add("user_id", Application.Current.Properties["user_id"].ToString());

                        var response = await CartLogic.UpdateCart(CartItem);
                        if (response.status == 200)
                        {
                            HomeVM.MyCartCounter = response.cart_count;
                            MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
                            ViewModel.CartItems = new ObservableCollection<Cart>(response.data);
                            decimal total = 0;
                            int itemQuantity = 0;
                            foreach (var item in response.data)
                            {
                                total += decimal.Parse(item.total_without_tax);
                                itemQuantity += 1;
                            }
                            //t_total.Text = "Rp " + total;
                            t_quantity.Text = itemQuantity + " Items";
                            m_total.Text = "Rp " + total;
                            b_total.Text = "Rp " + total;
                            TotalLimit = total;
                            //b_quantity.Text = itemQuantity + " Items";
                            if (response.data.Any())
                            {
                                b_cart.IsVisible = true;
                                //header.IsVisible = true;
                            }
                        }
                        else
                        {
                            Config.SnackbarMessage(response.message);
                        }
                        Config.HideDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("CartPage-increseQuantity_Clicked", ex.Message);
                Config.HideDialog();
            }
        }

        private void ScheduleChange_Tapped(object sender, EventArgs e)
        {
            if (sender is Label labelFontSize && labelFontSize.GestureRecognizers.Any())
            {
                var tap = (TapGestureRecognizer)labelFontSize.GestureRecognizers[0];
                var item = (Cart)tap.CommandParameter;
                Navigation.PushPopupAsync(new SchedulePopupPage(item));
            }
        }
    }
}