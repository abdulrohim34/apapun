using GroceryStore.Models;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();
            //this.Icon = "menu.png";
            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Home, Title="Home" },
                new HomeMenuItem {Id = MenuItemType.Login, Title="LoginPage" },
                new HomeMenuItem {Id = MenuItemType.Register, Title="Sign Up" },
                //new HomeMenuItem {Id = MenuItemType.ProductCategory, Title="Product Category" },
                //new HomeMenuItem {Id = MenuItemType.ProductDetail, Title="Product Detail" },
                //new HomeMenuItem {Id = MenuItemType.Cart, Title="Cart" },
                //new HomeMenuItem {Id = MenuItemType.Checkout, Title="Checkout" },
                //new HomeMenuItem {Id = MenuItemType.ManageAddress, Title="Manage Address" },
                //new HomeMenuItem {Id = MenuItemType.AddAddress, Title="Add New Address" },
                //new HomeMenuItem {Id = MenuItemType.ChangePassword, Title="Change Password" },
                //new HomeMenuItem {Id = MenuItemType.Notifications, Title="Notifications" },
                //new HomeMenuItem {Id = MenuItemType.VerifyCutomerInfo, Title="Verify Customer Info" },
                //new HomeMenuItem {Id = MenuItemType.Orders, Title="Orders" },
                //new HomeMenuItem {Id = MenuItemType.Favourites, Title="Favourites" },
                //new HomeMenuItem {Id = MenuItemType.MyProfile, Title="My Profile" },
                new HomeMenuItem {Id = MenuItemType.Help, Title="Help" }
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }
        //public List<MasterPageItem> menuList { get; set; }
        //public MenuPage()
        //{
        //    menuList = new List<MasterPageItem>();
        //    menuList.Add(new MasterPageItem()
        //    {
        //        Title = "Home",
        //        Icon = "",
        //        Type = typeof(Home)
        //    });
        //    menuList.Add(new MasterPageItem()
        //    {
        //        Title = "LoginPage",
        //        Icon = "",
        //        Type = typeof(LoginPage)
        //    });
        //    menuList.Add(new MasterPageItem()
        //    {
        //        Title = "Sign Up",
        //        Icon = "",
        //        Type = typeof(RegisterPage)
        //    });
        //    menuList.Add(new MasterPageItem()
        //    {
        //        Title = "Product Category",
        //        Icon = "",
        //        Type = typeof(LoginPage)
        //    });
        //    menuList.Add(new MasterPageItem()
        //    {
        //        Title = "Help",
        //        Icon = "",
        //        Type = typeof(LoginPage)
        //    });
        //    ListViewMenu.ItemsSource = menuList;
        //}
    }
}