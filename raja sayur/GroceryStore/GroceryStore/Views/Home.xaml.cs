using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GroceryStore.Models;
using CarouselView.FormsPlugin.Abstractions;
using GroceryStore.Logic;
using Plugin.Connectivity;
using DLToolkit.Forms.Controls;
using GroceryStore.Helpers;
using GroceryStore.ViewModels;
using GroceryStore.ContentView;
using FFImageLoading.Forms;
using GroceryStore.Controls;

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        public HomeVM ViewModel;
        int SlidePosition = 0;

        public Home()
        {
            //MessagingCenter.Send((App)Application.Current, "getCartCountHomeOnly");
            InitializeComponent();
            featuredLabel.IsVisible = false;
            quickLabel.IsVisible = false;
            offeredLabel.IsVisible = false;
            categoryLabel.IsVisible = false;
            mainContent.IsVisible = false;
            IsBusy.IsVisible = true;

            //((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.Black;
            //((NavigationPage)Application.Current.MainPage).BarTextColor = Color.OrangeRed;

            ViewModel = new HomeVM();
            BindingContext = ViewModel;
            //CustomNavigationBar.getCartCount();
            itemCategoryList.RefreshCommand = new Command(() =>
            {
                getData();
                itemCategoryList.IsRefreshing = false;
            });
            //getData();
            if (!string.IsNullOrEmpty(App.AndroidDeviceToken))
            {
                Application.Current.Properties["device_token"] = App.AndroidDeviceToken;
            }
            //HomeVM.MyCartCounter = "3";
            ViewModel.CartCounter = "0";
            //NavigationBarView.FirstNameLabel.SetBinding(Label.TextProperty, "CartCounter");
        }

        private void ProductTap_Tapped(object sender, EventArgs e)
        {
            try
            {
                var image = (CustomFrame)sender;
                if (image.GestureRecognizers.Count > 0)
                {
                    var tapGesture = (TapGestureRecognizer)image.GestureRecognizers[0];
                    var product = (Product)tapGesture.CommandParameter;

                    if (product != null)
                    {
                        Navigation.PushAsync(new ProductDetailPage(product));
                    }
                }
            }
            catch (Exception ex)
            {
                //DisplayAlert("Alert", Config.ApiErrorMessage, "Ok");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            getData();
            //CustomNavigationBarVM.PageName = "home";
            //CustomNavigationBarVM.MenuIcon = "menu.png";
            //MessagingCenter.Send((App)Application.Current, "MasterMenuUnselect");
            //MessagingCenter.Send((App)Application.Current, "ShowLogoTitle");
            //MessagingCenter.Send((App)Application.Current, "NavigationBar", "Grocery Store");
            //CustomNavigationBar customNavigationBar = new CustomNavigationBar();
            //customNavigationBar.getCartCountHomeOnly();
        }

        async void getData(bool from = false, string msg = null)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("MTG - Home GetData()");
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await DisplayAlert("Alert", "Your device is not connected to internet. Please try again later.",
                        "Ok");
                }
                else
                {
                    if (from)
                        Config.ShowDialog();
                    var response = await CategoryLogic.CategoryList();
                    if (response.status == "200")
                    {
                        //HomeVM.MyFavCounter = response.fav_count;
                        //HomeVM.MyCartCounter = response.cart_count;
                        //MessagingCenter.Send((App)Application.Current, "NavigationBar", "");
                        mainContent.IsVisible = true;
                        emptyContent.IsVisible = false;
                        if (response.data != null)
                            itemCategoryList.FlowItemsSource = response.data.ToList();
                        //headingTitle.IsVisible = true;
                        myView.ItemsSource = response.sliders;
                        myView.Position = 0;
                        myView.InterPageSpacing = 10;
                        myView.AnimateTransition = true;
                        myView.Orientation = CarouselViewOrientation.Horizontal;

                        if (response.featured != null && response.featured.Count > 0)
                        {
                            featuredLabel.IsVisible = true;
                            ViewModel.Featureds = new ObservableCollection<Product>(response.featured.ToList());
                            featuredProducts.ItemsSource = response.featured.ToList();
                        }
                        if (response.quick_products != null && response.quick_products.Count > 0)
                        {
                            quickLabel.IsVisible = true;
                            ViewModel.Quicks = new ObservableCollection<Product>(response.quick_products.ToList());
                            quickProducts.ItemsSource = response.quick_products.ToList();
                        }
                        if (response.offered_products != null && response.offered_products.Count > 0)
                        {
                            offeredLabel.IsVisible = true;
                            ViewModel.Offered = new ObservableCollection<Product>(response.offered_products.ToList());
                            offeredProducts.ItemsSource = response.offered_products.ToList();
                        }

                        Device.StartTimer(TimeSpan.FromSeconds(5), () =>
                        {
                            SlidePosition++;
                            if (SlidePosition == response.sliders.Count) SlidePosition = 0;
                            myView.Position = SlidePosition;
                            return true;
                        });
                        categoryLabel.IsVisible = true;
                        mainContent.IsVisible = true;
                        IsBusy.IsVisible = false;
                    }
                    else
                    {
                        mainContent.IsVisible = true;
                        IsBusy.IsVisible = false;
                        await DisplayAlert("Alert", response.message, "Ok");
                    }
                    if (from)
                    {
                        Config.HideDialog();
                        if (msg != null)
                            Config.SnackbarMessage(msg);
                    }
                }
                if (from)
                    Config.HideDialog();
            }
            catch (Exception ex)
            {
                mainContent.IsVisible = true;
                IsBusy.IsVisible = false;
                Config.ErrorStore("Home-getData", ex.Message);
                Config.HideDialog();
                await DisplayAlert("Alert", Config.ApiErrorMessage, "Ok");
            }
        }

        void Handle_Focused(object sender, Xamarin.Forms.FocusEventArgs e)
        {
            CustomNavigationBarVM.MenuIcon = "back.png";
            Navigation.PushAsync(new SearchPage());
        }

        //private void ProductDetail_Tapped(object sender, EventArgs e)
        //{
        //    var send = (Xamarin.Forms.StackLayout)sender;
        //    if (send.GestureRecognizers.Count > 0)
        //    {

        //    }
        //    Navigation.PushAsync(new ProductsPage(27, "Rice"));
        //}

        private void imageTap_Tapped(object sender, EventArgs e)
        {
            var image = (CachedImage)sender;
            if (image.GestureRecognizers.Count > 0)
            {
                var tapGesture = (TapGestureRecognizer)image.GestureRecognizers[0];
                var category = (Category)tapGesture.CommandParameter;
                MessagingCenter.Send(this, "MasterMenuUnselect");
                CustomNavigationBarVM.PageName = "products";
                CustomNavigationBarVM.MenuIcon = "back.png";
                Navigation.PushAsync(new ProductsPage(category.Id, category.Name));
            }
        }

        private void pickerVariation_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var ProductVariant = picker.SelectedItem as GetProductVariation;
            if (ProductVariant != null)
            {
                //var Product = ViewModel.Products.Where(p => p.id == ProductVariant.id).Single();
                //if (Product.SecondLoad != 1)
                //{
                //    Product.checkPickerLoad = 0;
                //}
                //else
                //{
                //    Product.checkPickerLoad = 1;
                //}
                //Product.SecondLoad = 1;

                var data = ViewModel.Featureds.Where(p => p.id == ProductVariant.product_id).Single();
                data.selected_variant_id = ProductVariant.id;
                //data.selected_index = 0;
                //var q = ViewModel.Products.Where(x => x.id == data.id).Single();
                data.price = ProductVariant.price;
                //var abc = ViewModel;
                if (data.SecondLoad != 1)
                {
                    data.checkPickerLoad = 0;
                }
                else
                {
                    data.checkPickerLoad = 1;
                }
                data.SecondLoad = 1;
            }
        }


        public async void AddToCart(Product product)
        {
            try
            {
                if (Application.Current.Properties.ContainsKey("isLoggedIn"))
                {
                    Config.ShowDialog();
                    Dictionary<string, string> addToCart = new Dictionary<string, string>();
                    addToCart.Add("product_id", product.id.ToString());
                    addToCart.Add("user_id", Application.Current.Properties["user_id"].ToString());
                    addToCart.Add("quantity", "1");
                    addToCart.Add("scheduled", "0");
                    //addToCart.Add("product_variation_id", product.selected_variant_id.ToString());
                    if (product.selected_variant_id == 0)
                        addToCart.Add("product_variation_id", product.get_product_variations[0].id.ToString());
                    else
                        addToCart.Add("product_variation_id", product.selected_variant_id.ToString());

                    //if (productVariation != null) addToCart.Add("product_variation_id", productVariation.id.ToString());
                    addToCart.Add("from_date", DateTime.Now.ToString("yyyy-MM-dd"));
                    addToCart.Add("to_date", DateTime.Now.ToString("yyyy-MM-dd"));
                    var response = await CartLogic.AddToCart(addToCart);
                    if (response.status == 200)
                    {
                        //HomeVM.MyCartCounter = response.cart_count;
                        Config.HideDialog();
                        //Config.SnackbarMessage(response.message);
                        getData(true, response.message);
                    }
                    else
                    {
                        Config.HideDialog();
                        Config.SnackbarMessage(response.message);
                    }
                }
                else
                {
                    Config.HideDialog();
                    await Navigation.PushAsync(new LoginPage());
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("ProductsPage-AddToCart", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        private void addToCart_Tapped(object sender, EventArgs e)
        {
            try
            {
                var label = (StackLayout)sender;
                if (label.GestureRecognizers.Count > 0)
                {
                    var tap = (TapGestureRecognizer)label.GestureRecognizers[0];
                    var product = (Product)tap.CommandParameter;
                    AddToCart(product);
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("ProductsPage-addToCart_Tapped", ex.Message);
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
                    var Item = (Product)tapGesture.CommandParameter;
                    int quantityItem = Item.cart_quantity;
                    if (quantityItem > 1)
                    {
                        Config.ShowDialog();
                        quantityItem -= 1;
                        Dictionary<string, string> CartItem = new Dictionary<string, string>();
                        CartItem.Add("product_id", Item.id.ToString());
                        CartItem.Add("quantity", quantityItem.ToString());
                        CartItem.Add("update_type", "quantity");
                        CartItem.Add("user_id", Application.Current.Properties["user_id"].ToString());

                        var response = await CartLogic.UpdateCart(CartItem);
                        if (response.status == 200)
                        {
                            getData(true);
                        }
                        else
                        {
                            Config.ErrorSnackbarMessage(response.message);
                        }
                        //Config.HideDialog();
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
                    var Item = (Product)tapGesture.CommandParameter;

                    int quantityItem = Item.cart_quantity;
                    if (quantityItem >= 1 && quantityItem < Item.quantity)
                    {
                        Config.ShowDialog();
                        quantityItem += 1;
                        Dictionary<string, string> CartItem = new Dictionary<string, string>();
                        CartItem.Add("product_id", Item.id.ToString());
                        CartItem.Add("quantity", quantityItem.ToString());
                        CartItem.Add("update_type", "quantity");
                        CartItem.Add("user_id", Application.Current.Properties["user_id"].ToString());

                        var response = await CartLogic.UpdateCart(CartItem);
                        if (response.status == 200)
                        {
                            getData(true);
                        }
                        else
                        {
                            Config.SnackbarMessage(response.message);
                        }
                        //Config.HideDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("CartPage-increseQuantity_Clicked", ex.Message);
                Config.HideDialog();
            }
        }

        async void DeleteProduct(Product Item)
        {
            try
            {
                Config.ShowDialog();
                Dictionary<string, int> CartItem = new Dictionary<string, int>();
                CartItem.Add("cart_id", Item.id);
                CartItem.Add("product_id", Item.id);
                CartItem.Add("user_id", int.Parse(Application.Current.Properties["user_id"].ToString()));

                var response = await CartLogic.DeleteCartItem(CartItem);
                if (response.status == 200)
                {
                    getData(true);
                }
                else if (response.status == 220)
                {
                    getData(true);
                }
                else
                {
                    getData(true);
                }
                Config.HideDialog();
            }
            catch (Exception ex)
            {
                Config.ErrorStore("CartPage-DeleteProduct", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }
    }
}