using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using GroceryStore.Helpers;
using GroceryStore.Logic;
using GroceryStore.Models;
using GroceryStore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageAddressPage : ContentPage
    {
        string _pageTitle = "Manage Address";
        public AddressVM ViewModel;
        bool _fromCheckoutPage;
        public ManageAddressPage()
        {
            InitializeComponent();
            ViewModel = new AddressVM();
            BindingContext = ViewModel;
        }

        public ManageAddressPage(bool fromCheckout)
        {
            InitializeComponent();
            ViewModel = new AddressVM();
            BindingContext = ViewModel;
            _fromCheckoutPage = fromCheckout;
            if (fromCheckout)
            {
                //noteLabel.Text = "Note : Click on list to set address for checkout";
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (CustomNavigationBarVM.PageName == "from_drawer")
            {
                CustomNavigationBarVM.MenuIcon = "menu.png";
            }
            else
            {
                CustomNavigationBarVM.MenuIcon = "back.png";
            }

            MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
            try
            {
                Config.ShowDialog();
                var userAddressList = await UserLogic.GetAddressList(int.Parse(Application.Current.Properties["user_id"].ToString()));
                if (userAddressList.status == 200)
                {
                    ViewModel.UserAddressList = new ObservableCollection<UserAddress>(userAddressList.data.ToList());
                    Config.HideDialog();
                    mainContent.IsVisible = true;
                    emptyContent.IsVisible = false;
                    saveAddress.IsVisible = true;
                    //note.IsVisible = true;
                    if (!ViewModel.UserAddressList.Any())
                    {
                        EmptyAddress();
                    }
                }
                else
                {
                    Config.HideDialog();
                    Config.SnackbarMessage(userAddressList.message);
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("ManageAddressPage-OnAppearing", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        private void addAddressTap_Tapped(object sender, EventArgs e)
        {
            CustomNavigationBarVM.MenuIcon = "back.png";
            Navigation.PushAsync(new AddAddressPage());
        }

        private void editAddressTap_Tapped(object sender, EventArgs e)
        {
            try
            {
                var image = (CachedImage)sender;
                if (image.GestureRecognizers.Count > 0)
                {
                    CustomNavigationBarVM.MenuIcon = "back.png";
                    var tap = (TapGestureRecognizer)image.GestureRecognizers[0];
                    var address = (UserAddress)tap.CommandParameter;
                    Navigation.PushAsync(new AddAddressPage(address));
                }
            }
            catch (Exception)
            {
                Config.SnackbarMessage(Config.ApiErrorMessage);
            }

        }

        private async void deleteTap_Tapped(object sender, EventArgs e)
        {
            try
            {
                var image = (CachedImage)sender;
                if (image.GestureRecognizers.Count > 0)
                {
                    Config.ShowDialog();
                    var tap = (TapGestureRecognizer)image.GestureRecognizers[0];
                    var address = (UserAddress)tap.CommandParameter;
                    var response = await UserLogic.DeleteAddress(address.id);
                    if (response.status == 200)
                    {
                        ViewModel.UserAddressList.Remove(ViewModel.UserAddressList.Where(a => a.id == address.id).SingleOrDefault());
                        if (!ViewModel.UserAddressList.Any())
                        {
                            EmptyAddress();
                        }
                    }
                    Config.HideDialog();
                    Config.SnackbarMessage(response.message);
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("ManageAddressPage-deleteTap_Tapped", ex.Message);
                Config.HideDialog();
                Config.SnackbarMessage(Config.ApiErrorMessage);
            }
        }

        private void listUserAddresses_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (_fromCheckoutPage)
                {
                    var listview = (ListView)sender;
                    var address = (UserAddress)listview.SelectedItem;
                    string dafault = address.default_address == 1 ? " (Default)" : "";
                    CheckoutPage.SetAddress(address.id, address.full_address, address.address_type, dafault);
                    Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("ManageAddressPage-listUserAddresses_ItemSelected", ex.Message);
                Config.HideDialog();
                Config.SnackbarMessage(Config.ApiErrorMessage);
            }
        }

        private async void addressSelected_Clicked(object sender, EventArgs e)
        {
            try
            {
                Config.ShowDialog();
                var radio = (Plugin.InputKit.Shared.Controls.RadioButton)sender;
                var address = (UserAddress)radio.CommandParameter;
                var response = await UserLogic.SetDefaultAddress(address.id, int.Parse(Application.Current.Properties["user_id"].ToString()));
                if (response.status == 200)
                {
                    Application.Current.Properties["full_address"] = address.full_address;
                    ViewModel.UserAddressList = new ObservableCollection<UserAddress>(response.data);
                }
                Config.HideDialog();
                Config.SnackbarMessage(response.message);
            }
            catch (Exception ex)
            {
                Config.ErrorStore("ManageAddressPage-addressSelected_Clicked", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        void EmptyAddress()
        {
            Config.HideDialog();
            mainContent.IsVisible = false;
            emptyContent.IsVisible = true;
            saveAddress.IsVisible = false;
            //note.IsVisible = false;
            //var size = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            //Label EmptyAddress = new Label()
            //{
            //    Text = ValidationMessages.EmptyAddress,
            //    FontAttributes = FontAttributes.Bold,
            //    HorizontalOptions = LayoutOptions.CenterAndExpand,
            //    VerticalOptions = LayoutOptions.CenterAndExpand,
            //    FontSize = size,
            //};
            //addressEmpty.Children.Clear();
            //addressEmpty.Children.Add(EmptyAddress);
        }
    }
}