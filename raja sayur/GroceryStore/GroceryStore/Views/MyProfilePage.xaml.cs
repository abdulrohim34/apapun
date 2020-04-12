using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GroceryStore.Helpers;
using GroceryStore.Models;
using GroceryStore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyProfilePage : ContentPage
    {
        //public ObservableCollection<ProfileMenu> profileMenus;
        public MyProfilePage()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            InitializeComponent();
            //var profileMenus = new List<ProfileMenu>
            //{
            //    new ProfileMenu() { Id = 1, Title = "Manage Address" },
            //    new ProfileMenu() { Id = 1, Title = "Orders" },
            //    //new ProfileMenu() { Id = 1, Title = "Upcoming Orders" },
            //    //new ProfileMenu() { Id = 1, Title = "Past Orders" },
            //    new ProfileMenu() { Id = 1, Title = "Favourites" },
            //    new ProfileMenu() { Id = 1, Title = "Change Password" },
            //    new ProfileMenu() { Id = 1, Title = "Help" },
            //    new ProfileMenu() { Id = 1, Title = "Logout" }
            //};


            //profileMenuList.ItemsSource = profileMenus;

            stopwatch.Stop();
            System.Diagnostics.Debug.WriteLine("-MTG21-");
            System.Diagnostics.Debug.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            MessagingCenter.Send((App)Application.Current, "NavigationBar", "");
            CustomNavigationBarVM.MenuIcon = "menu.png";
            try
            {
                name.Text = Application.Current.Properties["name"].ToString();
                email.Text = Application.Current.Properties["email"].ToString();
                mobile.Text = Application.Current.Properties["mobile_number"].ToString();
                locFullAddress.Text = Application.Current.Properties["full_address"].ToString();
                if (Application.Current.Properties["mobile_number"].ToString() == null)
                {
                    locIcon.IsVisible = false;
                    changeAddressLbl.Text = "Manage Address";
                }
                if (string.IsNullOrEmpty(Application.Current.Properties["full_address"].ToString()))
                {
                    MyAddress.IsVisible = false;
                    MyAddressBox.IsVisible = false;
                }
                else
                {
                    MyAddress.IsVisible = true;
                    MyAddressBox.IsVisible = true;
                }
                string profile = Application.Current.Properties["profile_picture"].ToString();
                if (CheckFileExist(profile))
                {
                    profileImage.Source = profile;
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("MyProfilePage-OnAppearing", ex.Message);
                DisplayAlert("Alert", Config.ApiErrorMessage, "Ok");
            }
            stopwatch.Stop();
            System.Diagnostics.Debug.WriteLine("-MTG22-");
            System.Diagnostics.Debug.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        public bool CheckFileExist(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "HEAD";

            bool exists;
            try
            {
                request.GetResponse();
                exists = true;
            }
            catch
            {
                exists = false;
            }
            return exists;
        }

        private void OnEditClicked(object sender, EventArgs e)
        {
            CustomNavigationBarVM.PageName = "profile";
            CustomNavigationBarVM.MenuIcon = "back.png";
            Navigation.PushAsync(new ProfilePage());
        }

        private async void OnMenuClicked(object sender, EventArgs e)
        {
            var entity = ((Label)sender);
            //entity.BackgroundColor = Color.FromHex("#ef3938");
            if (entity.Text == "Orders")
            {
                CustomNavigationBarVM.PageName = "orders";
                CustomNavigationBarVM.MenuIcon = "menu.png";
                await Navigation.PushAsync(new OrdersPage());
            }
            //if (entity.Text == "Upcoming Orders")
            //{
            //    await Navigation.PushAsync(new OrdersPage(moveId: 2));
            //}
            //else if (entity.Text == "Past Orders")
            //{
            //    await Navigation.PushAsync(new OrdersPage(moveId: 0));
            //}
            else if (entity.Text == "Manage Address")
            {
                CustomNavigationBarVM.PageName = "manage_address";
                CustomNavigationBarVM.MenuIcon = "back.png";
                await Navigation.PushAsync(new ManageAddressPage());
            }
            else if (entity.Text == "Favourites")
            {
                CustomNavigationBarVM.PageName = "favourites";
                CustomNavigationBarVM.MenuIcon = "menu.png";
                await Navigation.PushAsync(new FavouritesPage());
            }
            else if (entity.Text == "Change Password")
            {
                CustomNavigationBarVM.PageName = "change_password";
                CustomNavigationBarVM.MenuIcon = "menu.png";
                await Navigation.PushAsync(new ChangePasswordPage());
            }
            else if (entity.Text == "Help")
            {
                CustomNavigationBarVM.PageName = "help";
                CustomNavigationBarVM.MenuIcon = "menu.png";
                await Navigation.PushAsync(new HelpPage());
            }
            else if (entity.Text == "Logout")
            {
                CustomNavigationBarVM.PageName = "home";
                CustomNavigationBarVM.MenuIcon = "menu.png";
                Application.Current.Properties.Remove("isLoggedIn");
                Application.Current.Properties.Remove("user_id");
                Application.Current.Properties.Remove("name");
                Application.Current.Properties.Remove("mobile_number");
                Application.Current.Properties.Remove("email");
                App.Current.MainPage = new MasterTemplate();
            }
            else
            {
                //  await DisplayAlert("Alert", "Select right menu", "Ok");
            }
        }

        private void changeAddress_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ManageAddressPage());
        }
    }

    public class ProfileMenu
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}