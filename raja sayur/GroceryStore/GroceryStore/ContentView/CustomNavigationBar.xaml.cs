using System;
using System.Collections.Generic;
using GroceryStore.Logic;
using GroceryStore.ViewModels;
using GroceryStore.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using GroceryStore.Helpers;

namespace GroceryStore.ContentView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomNavigationBar : Xamarin.Forms.ContentView
    {
        public BadgesVM ViewModel;
        static string MyTitle = "";
        public static string MainImage = "menu.png";
        public CustomNavigationBar()
        {
            //ViewModel = new BadgesVM();
            //BindingContext = ViewModel;

            //InitializeComponent();
            //PageTitle.Text = MyTitle;
            //btnHamburger.Source = CustomNavigationBarVM.MenuIcon;
            //MessagingCenter.Subscribe<App, string>((App)Application.Current, "NavigationBar", SubscribeEvent);
            //MessagingCenter.Subscribe<App>((App)Application.Current, "ShowLogoTitle", ShowLogoTitle);
            //System.Diagnostics.Debug.WriteLine("-MTG1-" + HomeVM.MyCartCounter);
            //CartCountText.Text = HomeVM.MyCartCounter;
            //FavCountText.Text = HomeVM.MyFavCounter;
            //PageTitle.IsVisible = true;
        }

        //void SubscribeEvent(App app, String title)
        //{
        //    PageTitle.Text = title;
        //    if (Application.Current.Properties.ContainsKey("user_id"))
        //    {
        //        if (HomeVM.MyCartCounter == null || HomeVM.MyFavCounter == null || HomeVM.MyCartCounter == "0" || HomeVM.MyFavCounter == "0")
        //        {
        //            System.Diagnostics.Debug.WriteLine("-MTG2-");
        //            getCartCountHomeOnly();
        //        }
        //        else
        //        {
        //            System.Diagnostics.Debug.WriteLine("-MTG3-");
        //            CartCountText.Text = (HomeVM.MyCartCounter == null) ? "0" : HomeVM.MyCartCounter;
        //            FavCountText.Text = (HomeVM.MyFavCounter == null) ? "0" : HomeVM.MyFavCounter;
        //        }
        //    }
        //    else
        //    {
        //        System.Diagnostics.Debug.WriteLine("-MTG4-");
        //        HomeVM.MyCartCounter = "0";
        //        CartCountText.Text = "0";
        //        HomeVM.MyFavCounter = "0";
        //        FavCountText.Text = "0";
        //    }
        //}

        //void ShowLogoTitle(App app)
        //{
        //    PageTitle.IsVisible = false;
        //    LogoTitle.IsVisible = true;
        //    System.Diagnostics.Debug.WriteLine("-MTG5-");
        //    CartCountText.Text = (HomeVM.MyCartCounter == null) ? "0" : HomeVM.MyCartCounter;
        //    FavCountText.Text = (HomeVM.MyFavCounter == null) ? "0" : HomeVM.MyFavCounter;
        //}

        //void SlideOutDrawer_Tapped(object sender, System.EventArgs e)
        //{
        //    if (CustomNavigationBarVM.MenuIcon == "back.png")
        //    {
        //        Navigation.PopAsync();
        //    }
        //    else
        //    {
        //        MessagingCenter.Send(this, "presentMenu");
        //    }
        //}

        public static readonly BindableProperty TitleProperty = BindableProperty.Create("Title", typeof(string), typeof(CustomNavigationBar), "Grocery Store", propertyChanged: OnTitlePropertyChanged);

        Label a = new Label()
        {
            Text = "0"
        };
        public Label FirstNameLabel
        {
            get
            {
                return a;
            }
        }

        public static void getCartCount()
        {

        }

        public void getCartCountHomeOnly() { }

        //public string Title
        //{
        //    get
        //    {
        //        return (string)GetValue(TitleProperty);
        //    }
        //    set
        //    {
        //        SetValue(TitleProperty, value);
        //    }
        //}

        static void OnTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisView = bindable as CustomNavigationBar;
            MyTitle = newValue.ToString();
        }

        //void Favourites_Clicked(object sender, System.EventArgs e)
        //{
        //    CustomNavigationBarVM.PageName = "favourite";
        //    CustomNavigationBarVM.MenuIcon = "back.png";
        //    MessagingCenter.Send(this, "MasterMenuUnselect");
        //    if (Navigation.NavigationStack.Count == 0 || Navigation.NavigationStack.Last().GetType() != typeof(FavouritesPage))
        //        Navigation.PushAsync(new FavouritesPage());
        //}

        //void Search_Clicked(object sender, System.EventArgs e)
        //{
        //    //CustomNavigationBarVM.PageName = "search";
        //    //CustomNavigationBarVM.MenuIcon = "back.png";
        //    //MessagingCenter.Send(this, "MasterMenuUnselect");
        //    //if (Navigation.NavigationStack.Count == 0 || Navigation.NavigationStack.Last().GetType() != typeof(SearchPage))
        //    //Navigation.PushAsync(new SearchPage());
        //    mainGrid.IsVisible = false;
        //    searchGrid.IsVisible = true;
        //    searchEntry.Focus();
        //}

        //void CartNavigation_Tapped(object sender, System.EventArgs e)
        //{
        //    CustomNavigationBarVM.PageName = "cart";
        //    CustomNavigationBarVM.MenuIcon = "back.png";
        //    if (Navigation.NavigationStack.Count == 0 || Navigation.NavigationStack.Last().GetType() != typeof(CartPage))
        //        Navigation.PushAsync(new CartPage());
        //}

        //void FavNavigation_Tapped(object sender, System.EventArgs e)
        //{
        //    CustomNavigationBarVM.PageName = "fav";
        //    CustomNavigationBarVM.MenuIcon = "back.png";
        //    MessagingCenter.Send(this, "MasterMenuUnselect");
        //    if (Navigation.NavigationStack.Count == 0 || Navigation.NavigationStack.Last().GetType() != typeof(FavouritesPage))
        //        Navigation.PushAsync(new FavouritesPage());
        //}

        //public static async void getCartCount()
        //{
        //    try
        //    {
        //        if (Application.Current.Properties.ContainsKey("user_id"))
        //        {
        //            var response = await CartLogic.CartCount(Application.Current.Properties["user_id"].ToString());
        //            if (response.status == 200)
        //            {
        //                System.Diagnostics.Debug.WriteLine("-MTG6-");
        //                HomeVM.MyCartCounter = response.cart_count.ToString();
        //                HomeVM.MyFavCounter = response.fav_count.ToString();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Config.ErrorStore("CustomNavigationBar-getCartCount", ex.Message);
        //    }
        //}

        //public void updateCartBdges()
        //{

        //}

        //public static void updateNavBar()
        //{
        //    CustomNavigationBar customNavigationBar = new CustomNavigationBar();
        //    customNavigationBar.getCartCountHomeOnly();
        //}

        //public async void getCartCountHomeOnly()
        //{
        //    try
        //    {
        //        if (Application.Current.Properties.ContainsKey("user_id"))
        //        {
        //            var response = await CartLogic.CartCount(Application.Current.Properties["user_id"].ToString());
        //            if (response.status == 200)
        //            {
        //                System.Diagnostics.Debug.WriteLine("-MTG7-");
        //                HomeVM.MyCartCounter = response.cart_count.ToString();
        //                HomeVM.MyFavCounter = response.fav_count.ToString();
        //                if (HomeVM.MyCartCounter == null || HomeVM.MyFavCounter == null)
        //                {
        //                    System.Diagnostics.Debug.WriteLine("-MTG8-");
        //                    HomeVM.MyCartCounter = "0";
        //                    CartCountText.Text = "0";
        //                    HomeVM.MyFavCounter = "0";
        //                    FavCountText.Text = "0";
        //                }
        //                else
        //                {
        //                    System.Diagnostics.Debug.WriteLine("-MTG9-");
        //                    CartCountText.Text = response.cart_count.ToString();
        //                    FavCountText.Text = response.fav_count.ToString();
        //                }

        //                System.Diagnostics.Debug.WriteLine("-MTG10-");
        //                Application.Current.Properties["cart_count"] = response.cart_count;
        //                Application.Current.Properties["fav_count"] = response.fav_count;
        //            }
        //            else
        //            {
        //                System.Diagnostics.Debug.WriteLine("-MTG11-");
        //                HomeVM.MyCartCounter = "0";
        //                CartCountText.Text = "0";
        //                HomeVM.MyFavCounter = "0";
        //                FavCountText.Text = "0";
        //            }
        //        }
        //        else
        //        {
        //            HomeVM.MyCartCounter = "0";
        //            CartCountText.Text = "0";
        //            HomeVM.MyFavCounter = "0";
        //            FavCountText.Text = "0";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine("-MTG-12-");
        //        System.Diagnostics.Debug.WriteLine(ex.Message);
        //        Config.ErrorStore("CustomNavigationBar-getCartCountHomeOnly", ex.Message);
        //        HomeVM.MyCartCounter = "0";
        //        CartCountText.Text = "0";
        //        HomeVM.MyFavCounter = "0";
        //        FavCountText.Text = "0";
        //    }
        //}

        //private void Search_Tapped(object sender, EventArgs e)
        //{
        //    mainGrid.IsVisible = false;
        //    searchGrid.IsVisible = true;
        //    searchEntry.Focus();
        //}

        //private void Back_Tapped(object sender, EventArgs e)
        //{
        //    mainGrid.IsVisible = true;
        //    searchGrid.IsVisible = false;
        //    searchEntry.Unfocus();
        //}

        //private void Search_Tapped_1(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(searchEntry.Text) || string.IsNullOrEmpty(searchEntry.Text))
        //    {
        //        //searchEntry.Focus();
        //        Config.SnackbarMessage(ValidationMessages.SearchTextRequired);
        //    }
        //    else
        //    {
        //        mainGrid.IsVisible = true;
        //        searchGrid.IsVisible = false;
        //        Navigation.PushAsync(new ProductsPage(searchEntry.Text));
        //    }
        //}
    }
}
