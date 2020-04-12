using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using GroceryStore.Logic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GroceryStore.Models;
using System.Windows.Input;
using GroceryStore.ViewModels;
using GroceryStore.Helpers;
using FFImageLoading.Forms;
using DLToolkit.Forms.Controls;
using GroceryStore.Controls;

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsPage : ContentPage
    {
        string _pageTitle;
        public static ProductListVM ViewModel;
        public int _category_id;
        public string _search = null;
        //int _variation_id;
        public static string my_price;
        public static int FirstLoad = 1;
        public ProductsPage()
        {
            InitializeComponent();
            ViewModel = new ProductListVM();
            BindingContext = ViewModel;
        }
        public ProductsPage(string search)
        {
            InitializeComponent();
            ViewModel = new ProductListVM();
            BindingContext = ViewModel;
            _search = search;
            Title = search;
            listProducts.RefreshCommand = new Command(() =>
            {
                getData();
                listProducts.IsRefreshing = false;
            });
            //getData();
            _pageTitle = search;
        }

        public ProductsPage(int CategoryId, string pageTitle)
        {
            InitializeComponent();
            ViewModel = new ProductListVM();
            BindingContext = ViewModel;
            _category_id = CategoryId;
            Title = pageTitle;
            listProducts.RefreshCommand = new Command(() =>
            {
                getData();
                listProducts.IsRefreshing = false;
            });
            //getData();
            _pageTitle = pageTitle;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CustomNavigationBarVM.MenuIcon = "back.png";
            listProducts.SelectedItem = null;
            MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
            getData();
        }

        private async void getData()
        {
            try
            {
                if (_search == null)
                {
                    Config.ShowDialog();
                    var products = new ProductResponce();
                    if (Application.Current.Properties.ContainsKey("user_id"))
                    {
                        products = await ProductLogic.GetProducts(_category_id, Application.Current.Properties["user_id"].ToString());
                    }
                    else
                    {
                        products = await ProductLogic.GetProducts(_category_id, null);
                    }
                    if (products.status == 200)
                    {
                        Config.HideDialog();
                        if (products.data != null)
                        {
                            ViewModel.Products = new ObservableCollection<Product>(products.data.ToList());
                            //listProducts.FlowItemsSource = products.data.ToList();
                            listProducts.ItemsSource = products.data.ToList();
                        }
                        else
                        {
                            EmptyProducts();
                        }
                    }
                    else
                    {
                        EmptyProducts();
                    }
                }
                else
                {
                    Config.ShowDialog();
                    var products = new SearchProductResponce();
                    if (Application.Current.Properties.ContainsKey("user_id"))
                    {
                        products = await ProductLogic.GetSearchProducts(_search, Application.Current.Properties["user_id"].ToString());
                    }
                    else
                    {
                        products = await ProductLogic.GetSearchProducts(_search, null);
                    }
                    if (products.status == 200)
                    {
                        Config.HideDialog();
                        if (products.data != null)
                        {
                            listProducts.ItemsSource = products.data.ToList();
                            //listProducts.FlowItemsSource = products.data.ToList();
                            ViewModel.Products = new ObservableCollection<Product>(products.data.ToList());
                        }
                        else
                        {
                            EmptyProducts();
                        }
                    }
                    else
                    {
                        Config.HideDialog();
                        EmptyProducts();
                    }
                    Config.HideDialog();
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("ProductsPage-getData", ex.Message);
                Config.HideDialog();
                EmptyProducts();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        void EmptyProducts()
        {
            var size = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            Label emptyCart = new Label()
            {
                Text = ValidationMessages.EmptyProducts,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontSize = size,
            };
            Content = emptyCart;
        }

        private async void favouriteImage_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (Application.Current.Properties.ContainsKey("isLoggedIn"))
                {
                    Config.ShowDialog();
                    var image = (CachedImage)sender;
                    if (image.GestureRecognizers.Count > 0)
                    {
                        var tap = (TapGestureRecognizer)image.GestureRecognizers[0];
                        var product = (Product)tap.CommandParameter;

                        Dictionary<string, int> favouriteProduct = new Dictionary<string, int>();
                        favouriteProduct.Add("user_id", int.Parse(Application.Current.Properties["user_id"].ToString()));
                        favouriteProduct.Add("product_id", product.id);
                        var response = await ProductLogic.AddFavouriteProduct(favouriteProduct);
                        if (response.status == 200)
                        {
                            HomeVM.MyFavCounter = response.fav_count;
                            MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
                            //ViewModel.FavouriteProducts.Remove(ViewModel.FavouriteProducts.Where(p => p.id == int.Parse(productId)).Single());
                            var prod = ViewModel.Products.Where(p => p.id == product.id).FirstOrDefault();
                            if (prod != null)
                            {
                                if (response.favourite_status == "0")
                                {
                                    prod.favourite = "Favourites.png";
                                    image.Source = "Favourites.png";
                                }
                                else
                                {
                                    prod.favourite = "Favourites_selected.png";
                                    image.Source = "Favourites_selected.png";
                                }
                            }
                            Config.HideDialog();
                            Config.SnackbarMessage(response.message);
                        }
                        else
                        {
                            Config.HideDialog();
                            Config.SnackbarMessage(response.message);
                        }
                    }
                    Config.HideDialog();
                }
                else
                {
                    Config.HideDialog();
                    await Navigation.PushAsync(new LoginPage());
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("ProductsPage-favouriteImage_Tapped", ex.Message);
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
                        HomeVM.MyCartCounter = response.cart_count;
                        MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
                        MessagingCenter.Send((App)Application.Current, "getCartCountHomeOnly");
                        Config.HideDialog();
                        Config.SnackbarMessage(response.message);
                        getData();
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

        public void AddToCartAction(object sender, EventArgs e)
        {
            var menu = ((MenuItem)sender);
            var product = (Product)menu.CommandParameter;
            AddToCart(product);
        }

        public void Handle_Tapped(object sender, EventArgs e)
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
                        CustomNavigationBarVM.PageName = "product_detail";
                        CustomNavigationBarVM.MenuIcon = "back.png";
                        Navigation.PushAsync(new ProductDetailPage(product));
                    }
                }
            }
            catch (Exception ex)
            {
                //DisplayAlert("Alert", Config.ApiErrorMessage, "Ok");
            }
        }

        public void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var listview = (FlowListView)sender;
                var product = (Product)listview.SelectedItem;
                if (product != null)
                {
                    CustomNavigationBarVM.PageName = "product_detail";
                    CustomNavigationBarVM.MenuIcon = "back.png";
                    Navigation.PushAsync(new ProductDetailPage(product));
                }
            }
            catch (Exception)
            {
                //DisplayAlert("Alert", Config.ApiErrorMessage, "Ok");
            }
        }

        private void listProducts_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var listview = (ListView)sender;
                var product = (Product)listview.SelectedItem;
                if (product != null)
                {
                    CustomNavigationBarVM.PageName = "product_detail";
                    CustomNavigationBarVM.MenuIcon = "back.png";
                    Navigation.PushAsync(new ProductDetailPage(product));
                }
            }
            catch (Exception)
            {
                //DisplayAlert("Alert", Config.ApiErrorMessage, "Ok");
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

                var data = ViewModel.Products.Where(p => p.id == ProductVariant.product_id).Single();
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
                            getData();
                        }
                        else
                        {
                            Config.ErrorSnackbarMessage(response.message);
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
                            getData();
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
                    getData();
                }
                else if (response.status == 220)
                {
                    getData();
                }
                else
                {
                    getData();
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