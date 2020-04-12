using System;
using System.Collections.Generic;
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
    public partial class ProductDetailPage : ContentPage
    {
        string _pageTitle;
        Product _product;
        int _variation_id;
        public ProductDetailPage()
        {
            InitializeComponent();
        }

        public ProductDetailPage(Product product)
        {
            InitializeComponent();
            try
            {
                _product = product;
                Title = product.name;
                name.Text = product.name;
                brand_name.Text = product.product_brand.brand_name;
                b_price.Text = "Rp " + product.price;
                description.Text = product.description;
                weight.ItemsSource = product.get_product_variations.ToList();
                weight.SelectedIndex = 0;
                image.Source = product.image;
                favouriteIcon.Source = product.favourite;
                _pageTitle = product.name;
                if (product.special_price == null)
                {
                    price.Text = "Rp " + product.price;
                    special_container.IsVisible = false;
                }
                else
                {
                    special_container.IsVisible = true;
                    price.IsVisible = false;
                    old_price.Text = "Rp " + product.price;
                    special_price.Text = "Rp " + product.special_price;
                }

            }
            catch (Exception ex)
            {
                Config.ErrorStore("ProductDetailPage-ProductDetailPage", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CustomNavigationBarVM.MenuIcon = "back.png";
            try
            {
                var variation = _product.get_product_variations[0];
                _variation_id = variation.id;
            }
            catch (Exception ex)
            {
                Config.ErrorStore("ProductDetailPage-OnAppearing", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
            MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
        }

        private void decreseQuantity_Tapped(object sender, EventArgs e)
        {
            try
            {
                var image = (Frame)sender;
                if (image.GestureRecognizers.Count > 0)
                {
                    var tapGesture = (TapGestureRecognizer)image.GestureRecognizers[0];
                    int quantityItem = int.Parse(quantity.Text);
                    if (quantityItem > 1)
                    {
                        quantityItem -= 1;
                        quantity.Text = quantityItem.ToString();
                        decimal priceItem = decimal.Parse(_product.get_product_variations.Where(a => a.id == _variation_id).Single().price);
                        b_price.Text = "Rp " + Math.Round((quantityItem * priceItem), 2);
                    }
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("ProductDetailPage-decreseQuantity_Tapped", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }


        public void increseQuantity_Tapped(object sender, EventArgs e)
        {
            try
            {
                var image = (Frame)sender;
                if (image.GestureRecognizers.Count > 0)
                {
                    var tapGesture = (TapGestureRecognizer)image.GestureRecognizers[0];
                    int quantityItem = int.Parse(quantity.Text);
                    if (quantityItem >= 1 && quantityItem < _product.quantity)
                    {
                        quantityItem += 1;
                        quantity.Text = quantityItem.ToString();
                        decimal priceItem = decimal.Parse(_product.get_product_variations.Where(a => a.id == _variation_id).Single().price);
                        b_price.Text = "Rp " + Math.Round((quantityItem * priceItem), 2);
                    }
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("ProductDetailPage-increseQuantity_Tapped", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        private async void addToCart_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (Application.Current.Properties.ContainsKey("isLoggedIn"))
                {
                    Config.ShowDialog();
                    var productVariation = _product.get_product_variations
                        .FirstOrDefault(x => x.weightUnit == weight.Items[weight.SelectedIndex]);
                    Dictionary<string, string> addToCart = new Dictionary<string, string>();
                    addToCart.Add("product_id", _product.id.ToString());
                    addToCart.Add("user_id", Application.Current.Properties["user_id"].ToString());
                    addToCart.Add("quantity", quantity.Text);
                    addToCart.Add("scheduled", "0");
                    if (productVariation != null) addToCart.Add("product_variation_id", productVariation.id.ToString());
                    addToCart.Add("from_date", DateTime.Now.ToString("yyyy-MM-dd"));
                    addToCart.Add("to_date", DateTime.Now.ToString("yyyy-MM-dd"));
                    var response = await CartLogic.AddToCart(addToCart);
                    if (response.status == 200)
                    {
                        Config.HideDialog();
                        Config.SnackbarMessage(response.message);
                        HomeVM.MyCartCounter = response.cart_count;
                        MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
                        MessagingCenter.Send((App)Application.Current, "getCartCountHomeOnly");
                        CustomNavigationBarVM.MenuIcon = "back.png";
                        await Navigation.PushAsync(new CartPage());
                        //await DisplayAlert("Success", response.message, "Ok");
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
                Config.ErrorStore("ProductDetailPage-addToCart_Tapped", ex.Message);
                Config.HideDialog();
                await DisplayAlert("Alert", Config.ApiErrorMessage, "Ok");
            }
        }

        private async void addFavouriteTap_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (Application.Current.Properties.ContainsKey("isLoggedIn"))
                {
                    Dictionary<string, int> favouriteProduct = new Dictionary<string, int>();
                    favouriteProduct.Add("user_id", int.Parse(Application.Current.Properties["user_id"].ToString()));
                    favouriteProduct.Add("product_id", _product.id);
                    var response = await ProductLogic.AddFavouriteProduct(favouriteProduct);
                    if (response.status == 200)
                    {
                        HomeVM.MyFavCounter = response.fav_count;
                        MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);

                        if (response.favourite_status != "0")
                        {
                            Config.HideDialog();
                            favouriteIcon.Source = "Favourites_selected.png";
                        }
                        else
                        {
                            Config.HideDialog();
                            favouriteIcon.Source = "Favourites.png";
                        }
                        //var prod = ProductsPage.ViewModel.Products.Where(p => p.id == _product.id).FirstOrDefault();
                        //if (prod != null)
                        //{
                        //    if (response.favourite_status == "0")
                        //    {
                        //        prod.favourite = "Favourites.png";
                        //    }
                        //    else
                        //    {
                        //        prod.favourite = "Favourites_selected.png";
                        //    }
                        //}

                        Config.HideDialog();
                        Config.SnackbarMessage(response.message);
                        //await DisplayAlert("Success", response.message, "Ok");
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
                Config.ErrorStore("ProductDetailPage-addFavouriteTap_Tapped", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        private void weight_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var picker = (Picker)sender;
                var variation = (GetProductVariation)picker.SelectedItem;
                _variation_id = variation.id;
                decimal priceItem = decimal.Parse(variation.price);
                b_price.Text = "Rp " + Math.Round((int.Parse(quantity.Text) * priceItem), 2);
                price.Text = "Rp " + Math.Round(priceItem, 2);
            }
            catch (Exception ex)
            {
                Config.ErrorStore("ProductDetailPage-weight_SelectedIndexChanged", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }
    }
}