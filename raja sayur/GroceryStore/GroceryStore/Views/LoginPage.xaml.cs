using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using GroceryStore.Helpers;
using GroceryStore.Models;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GroceryStore.ViewModels;
using ModernHttpClient;

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly HttpClient _client = new HttpClient(new NativeMessageHandler());
        private readonly User _user = new User();
        private RegisterResponse _registerResponse = new RegisterResponse();

        public LoginPage()
        {
            InitializeComponent();
            //var abc = App.AndroidDeviceToken;
            //var xyz = App.XamarinDeviceToken;
        }

        private async void btn_login_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await DisplayAlert("Alert", "Your device is not connected to internet. Please try again later.",
                        "Ok");
                }
                if (string.IsNullOrWhiteSpace(email.Text) || string.IsNullOrEmpty(email.Text))
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.EmailRequired);
                    return;
                }
                else if (string.IsNullOrWhiteSpace(password.Text) || string.IsNullOrEmpty(password.Text))
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.PasswordRequired);
                }
                else
                {
                    Config.ShowDialog();
                    var user = new User()
                    {
                        email = email.Text,
                        password = password.Text,
                        user_type = "user",
                        device_id = null,
                    };
                    if (Application.Current.Properties.ContainsKey("device_token"))
                    {
                        user.device_token = Application.Current.Properties["device_token"].ToString();
                    }
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        Application.Current.Properties["device_type"] = "ios";
                        user.device_type = "ios";
                    }
                    else if (Device.RuntimePlatform == Device.Android)
                    {
                        Application.Current.Properties["device_type"] = "android";
                        user.device_type = "android";
                    }
                    else
                    {
                        Application.Current.Properties["device_type"] = "windows";
                        user.device_type = "windows";
                    }

                    var _registerResponse = await _user.Login(user);
                    if (_registerResponse.status == "200")
                    {
                        Config.HideDialog();
                        App.user = _registerResponse.data;
                        Application.Current.Properties["isLoggedIn"] = true;
                        Application.Current.Properties["user_id"] = _registerResponse.data.id;
                        Application.Current.Properties["name"] = _registerResponse.data.name;
                        Application.Current.Properties["mobile_number"] = _registerResponse.data.mobile_number;
                        Application.Current.Properties["email"] = _registerResponse.data.email;
                        Application.Current.Properties["profile_picture"] = _registerResponse.data.profile_picture;
                        Application.Current.Properties["order_message"] = _registerResponse.refer_message;
                        if (_registerResponse.user_address != null)
                        {
                            Application.Current.Properties["full_address"] = _registerResponse.user_address.full_address;
                            Application.Current.Properties["address_type"] = _registerResponse.user_address.address_type;
                        }
                        else
                        {
                            Application.Current.Properties["full_address"] = "";
                            Application.Current.Properties["address_type"] = "";
                        }
                        HomeVM.MyCartCounter = _registerResponse.cart_count;
                        HomeVM.MyFavCounter = _registerResponse.fav_count;
                        MessagingCenter.Send((App)Application.Current, "NavigationBar", "");

                        App.Current.MainPage = new MasterTemplate();
                    }
                    else
                    {
                        Config.HideDialog();
                        Config.ErrorSnackbarMessage(_registerResponse.message);
                    }
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("LoginPage-btn_login_Clicked", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        private void forgotPassword_Tapped(object sender, EventArgs e)
        {
            CustomNavigationBarVM.MenuIcon = "back.png";
            Navigation.PushModalAsync(new ForgotPasswordPage());
        }

        private void signUpTap_Tapped(object sender, EventArgs e)
        {
            //CustomNavigationBarVM.MenuIcon = "back.png";
            //CustomNavigationBarVM.PageName = "register";
            Navigation.PushModalAsync(new RegisterPage());
        }

        private void CrossPage_Tapped(object sender, EventArgs e)
        {
            if (CustomNavigationBarVM.PageName == "from_drawer")
            {
                CustomNavigationBarVM.MenuIcon = "menu.png";
                CustomNavigationBarVM.PageName = "home";
                Navigation.PushAsync(new Home());
            }
            else
            {
                Navigation.PopModalAsync();
                //Navigation.PopAsync();
            }
        }
    }
}