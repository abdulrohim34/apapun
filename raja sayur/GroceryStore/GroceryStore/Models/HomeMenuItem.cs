using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryStore.Models
{
    public enum MenuItemType
    {
        Home,
        Login,
        Register,
        ProductCategory,
        ProductDetail,
        Cart,
        Checkout,
        MyProfile,
        ManageAddress,
        AddAddress,
        ChangePassword,
        Notifications,
        VerifyCutomerInfo,
        Orders,
        Favourites,
        Help

    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
