using GroceryStore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GroceryStore.ViewModels;

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderSuccessPage : ContentPage
    {
        public OrderSuccessPage()
        {
            InitializeComponent();
            Config.HideDialog();
            CustomNavigationBarVM.PageName = "home";
            try
            {
                if (Application.Current.Properties.ContainsKey("order_message"))
                {
                    string msg = Application.Current.Properties["order_message"].ToString();
                    orderMessage.Text = msg;
                }
            }
            catch (Exception ex)
            {
                //string msg = "Order should reach before 7:30 AM";
                string msg = "";
                orderMessage.Text = msg;
            }
        }

        private async void BackToHome_Clicked(object sender, EventArgs e)
        {
            CustomNavigationBarVM.MenuIcon = "menu.png";
            await Navigation.PopModalAsync();
        }
    }
}