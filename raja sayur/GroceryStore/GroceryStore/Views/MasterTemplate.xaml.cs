using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GroceryStore.Models;
using GroceryStore.Services;
using GroceryStore.Logic;
using GroceryStore.ViewModels;
using GroceryStore.Helpers;
using System.Net;

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterTemplate : MasterDetailPage
    {
        public List<MasterPageItem> menuList { get; set; }
        public MasterTemplate()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<ContentView.CustomNavigationBar>(this, "presentMenu", (sender) => { IsPresented = !IsPresented; });


            MessagingCenter.Subscribe<App>((App)Application.Current, "MasterMenuUnselect", (sender) =>
            {
                navigationDrawerList.SelectedItem = null;
            });

            if (Application.Current.Properties.ContainsKey("isLoggedIn"))
            {
                menuList = new List<MasterPageItem>
                {
                    new MasterPageItem() {Title = "Home", TargetType = typeof(Home)},
                    //new MasterPageItem() {Title = "My Profile", TargetType = typeof(MyProfilePage)},
                    new MasterPageItem() {Title = "My Addresses", TargetType = typeof(ManageAddressPage)},
                    new MasterPageItem() {Title = "My Orders", TargetType = typeof(OrdersPage)},
                    //new MasterPageItem() {Title = "Upcoming Orders", TargetType = typeof(OrdersPage)},
                    //new MasterPageItem() {Title = "Past Orders", TargetType = typeof(OrdersPage)},
                    new MasterPageItem() {Title = "My Favourites", TargetType = typeof(FavouritesPage)},
                    new MasterPageItem() {Title = "Change Password", TargetType = typeof(ChangePasswordPage)},
                    new MasterPageItem() {Title = "Notification", TargetType = typeof(NotificationsPage)},
                    new MasterPageItem() {Title = "Faq", TargetType = typeof(HelpPage)},
                    new MasterPageItem() {Title = "Logout", TargetType = typeof(Home)}
                };

                VisibleProfileName.IsVisible = bool.Parse(Application.Current.Properties["isLoggedIn"].ToString());
                profileName.Text = Application.Current.Properties["name"].ToString();
                email.Text = Application.Current.Properties["email"].ToString();
                mobile_number.Text = Application.Current.Properties["mobile_number"].ToString();
            }
            else
            {
                menuList = new List<MasterPageItem>
                {
                    new MasterPageItem(){Title="Home", TargetType=typeof(Home)},
                    new MasterPageItem(){Title="Login", TargetType=typeof(LoginPage)},
                    new MasterPageItem(){Title="Sign Up", TargetType=typeof(RegisterPage)},
                    new MasterPageItem(){Title="Faq", TargetType=typeof(HelpPage)}
                };
            }

            navigationDrawerList.ItemsSource = menuList;
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Home)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Application.Current.Properties.ContainsKey("isLoggedIn"))
            {
                menuList = new List<MasterPageItem>
                {
                    new MasterPageItem() {Title = "Home", TargetType = typeof(Home)},
                    //new MasterPageItem() {Title = "My Profile", TargetType = typeof(MyProfilePage)},
                    new MasterPageItem() {Title = "My Addresses", TargetType = typeof(ManageAddressPage)},
                    new MasterPageItem() {Title = "My Orders", TargetType = typeof(OrdersPage)},
                    //new MasterPageItem() {Title = "Upcoming Orders", TargetType = typeof(OrdersPage)},
                    //new MasterPageItem() {Title = "Past Orders", TargetType = typeof(OrdersPage)},
                    new MasterPageItem() {Title = "My Favourites", TargetType = typeof(FavouritesPage)},
                    new MasterPageItem() {Title = "Change Password", TargetType = typeof(ChangePasswordPage)},
                    new MasterPageItem() {Title = "Notification", TargetType = typeof(NotificationsPage)},
                    new MasterPageItem() {Title = "Faq", TargetType = typeof(HelpPage)},
                    new MasterPageItem() {Title = "Logout", TargetType = typeof(Home)}
                };
                profileName.Text = Application.Current.Properties["name"].ToString();
                email.Text = Application.Current.Properties["email"].ToString();
                mobile_number.Text = Application.Current.Properties["mobile_number"].ToString();
                string profile = Application.Current.Properties["profile_picture"].ToString();
                if (CheckFileExist(profile))
                {
                    profileImage.Source = profile;
                }
            }
            else
            {
                menuList = new List<MasterPageItem>
                {
                    new MasterPageItem(){Title="Home", TargetType=typeof(Home)},
                    new MasterPageItem(){Title="Login", TargetType=typeof(LoginPage)},
                    new MasterPageItem(){Title="Sign Up", TargetType=typeof(RegisterPage)},
                    new MasterPageItem(){Title="Faq", TargetType=typeof(HelpPage)}
                };
            }

            navigationDrawerList.ItemsSource = menuList;
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Home)));

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

        private async void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var item = e.SelectedItem as MasterPageItem;
                if (item != null)
                {
                    if (item.Title == "Logout")
                    {
                        Config.ShowDialog();
                        CustomNavigationBarVM.PageName = "home";
                        CustomNavigationBarVM.MenuIcon = "menu.png";
                        item.ItemColor = "Black";
                        try
                        {
                            var response = await UserLogic.Logout(int.Parse(Application.Current.Properties["user_id"].ToString()));
                            if (response.status == 200)
                            {
                                Config.HideDialog();
                            }
                            Config.HideDialog();
                        }
                        catch (Exception)
                        {
                            Config.HideDialog();
                        }
                        Application.Current.Properties.Remove("isLoggedIn");
                        Application.Current.Properties.Remove("user_id");
                        Application.Current.Properties.Remove("name");
                        Application.Current.Properties.Remove("mobile_number");
                        Application.Current.Properties.Remove("email");
                        Application.Current.MainPage = new MasterTemplate();
                    }
                    else if (item.Title == "Login")
                    {
                        await Application.Current.MainPage.Navigation.PushModalAsync(new LoginPage());
                        IsPresented = false;
                    }
                    else if (item.Title == "Sign Up")
                    {
                        await Application.Current.MainPage.Navigation.PushModalAsync(new RegisterPage());
                        IsPresented = false;
                    }
                    else
                    {
                        CustomNavigationBarVM.MenuIcon = "menu.png";
                        CustomNavigationBarVM.PageName = "from_drawer";
                        Type page = item.TargetType;
                        Detail = new NavigationPage((Page)Activator.CreateInstance(page));
                        IsPresented = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("MasterTemplate-OnMenuItemSelected", ex.Message);
            }
        }

        private void toolCart_Clicked(object sender, EventArgs e)
        {
            Detail.Navigation.PushAsync((Page)Activator.CreateInstance(typeof(CartPage)));
        }

        private void toolFavourite_Clicked(object sender, EventArgs e)
        {
            if (Application.Current.Properties.ContainsKey("user_id"))
            {
                Detail.Navigation.PushAsync((Page)Activator.CreateInstance(typeof(FavouritesPage)));
            }
            else
            {
                Detail.Navigation.PushAsync((Page)Activator.CreateInstance(typeof(LoginPage)));
            }
        }

        private void toolSearch_Clicked(object sender, EventArgs e)
        {
            Detail.Navigation.PushAsync((Page)Activator.CreateInstance(typeof(SearchPage)));
        }

        private void companyUrlLink_Tapped(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(Config.BaseUrl));
        }

        private void editProfile_Tapped(object sender, EventArgs e)
        {
            CustomNavigationBarVM.MenuIcon = "back.png";
            //CustomNavigationBarVM.PageName = "from_drawer";
            Detail.Navigation.PushAsync(new ProfilePage());
            IsPresented = false;
        }
    }
}