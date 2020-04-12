using GroceryStore.Helpers;
using GroceryStore.Models;
using GroceryStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelpPage : ContentPage
    {
        string _pageTitle = "Faq";
        public FaqVM ViewModel;
        public HelpPage()
        {
            InitializeComponent();
            ViewModel = new FaqVM();
            getData();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CustomNavigationBarVM.PageName = "help";
            CustomNavigationBarVM.MenuIcon = "menu.png";
            MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
        }

        async void getData()
        {
            try
            {
                Config.ShowDialog();
                var response = await Faq.GetFaq();
                if (response.status == 200)
                {
                    emptyContent.IsVisible = false;
                    mainContent.IsVisible = true;
                    Config.HideDialog();
                    if (response.data.Any())
                    {
                        ViewModel.FaqList = new System.Collections.ObjectModel.ObservableCollection<Faq>(response.data);
                        faqSource.ItemsSource = ViewModel.FaqList;
                    }
                    else
                    {
                        EmptyHelp();
                    }
                }
                else
                {
                    EmptyHelp();
                    Config.SnackbarMessage(response.message);
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("HelpPage-DeleteClicked", ex.Message);
                Config.HideDialog();
                EmptyHelp();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        void EmptyHelp()
        {
            Config.HideDialog();
            mainContent.IsVisible = false;
            emptyContent.IsVisible = true;
            emptyLabel.Text = ValidationMessages.EmptyHelp;
            var size = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            emptyLabel.FontSize = size;
            //Config.HideDialog();
            //var size = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            //Label emptyCart = new Label()
            //{
            //    Text = ValidationMessages.EmptyHelp,
            //    FontAttributes = FontAttributes.Bold,
            //    HorizontalOptions = LayoutOptions.CenterAndExpand,
            //    VerticalOptions = LayoutOptions.CenterAndExpand,
            //    FontSize = size,
            //};
            //Content = emptyCart;
        }
    }
}