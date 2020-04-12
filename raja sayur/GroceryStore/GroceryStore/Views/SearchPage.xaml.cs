using GroceryStore.Helpers;
using GroceryStore.Logic;
using GroceryStore.Models;
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
    public partial class SearchPage : ContentPage
    {
        string _pageTitle = "Search";
        public SearchPage()
        {
            InitializeComponent();
            Title = "Search";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CustomNavigationBarVM.PageName = "search";
            CustomNavigationBarVM.MenuIcon = "back.png";
            searchEntry.Focus();
            MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
        }

        private void search_Tapped(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchEntry.Text) || string.IsNullOrEmpty(searchEntry.Text))
            {
                Config.SnackbarMessage(ValidationMessages.SearchTextRequired);
            }
            else
            {
                Navigation.PushAsync(new ProductsPage(searchEntry.Text));
            }
        }
    }
}