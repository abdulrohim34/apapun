using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GroceryStore.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using GroceryStore.Helpers;
using GroceryStore.ViewModels;
using System.Text.RegularExpressions;
using ModernHttpClient;
using GroceryStore.Logic;

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        private readonly HttpClient _client = new HttpClient(new NativeMessageHandler());
        private User _user = new User();
        private RegisterResponse _registerResponse = new RegisterResponse();

        public RegisterPage()
        {
            InitializeComponent();
            loadFirstData();
        }

        async void loadFirstData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>{
                    {"temp", "1"}};
            var countries = await UserLogic.GetCountry(data);
            countries.data.Insert(0, new Country() { id = 0, name = "Select Country" });
            country.ItemsSource = countries.data.ToList();
            country.SelectedIndex = 0;
            state.ItemsSource = new List<State>() { new State() { id = 0, name = "Select State" } };
            state.SelectedIndex = 0;
            city.ItemsSource = new List<City>() { new City() { id = 0, name = "Select City" } };
            city.SelectedIndex = 0;
        }

        private async void btnRegister_Clicked(object sender, EventArgs e)
        {
            try
            {
                string pattern = "^[A-Za-z ]+$";
                Regex regex = new Regex(pattern);
                Regex numberRegex = new Regex(@"^[0-9]*$");
                var a = numberRegex.IsMatch(mobile_number.Text);

                if (!CrossConnectivity.Current.IsConnected)
                {
                    await DisplayAlert("Alert", "Your device is not connected to internet. Please try again later.",
                        "Ok");
                }

                if (string.IsNullOrWhiteSpace(name.Text) || string.IsNullOrEmpty(name.Text))
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.NameRequired);
                    return;
                }
                else if (regex.IsMatch(name.Text) == false)
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.NameLetters);
                    return;
                }
                else if (string.IsNullOrWhiteSpace(email.Text) || string.IsNullOrEmpty(email.Text))
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.EmailRequired);
                    return;
                }
                else if (string.IsNullOrWhiteSpace(mobile_number.Text) || string.IsNullOrEmpty(mobile_number.Text))
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.PhoneNumberRequired);
                    return;
                }
                else if (mobile_number.Text.Length < 11)
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.PhoneNumberMinimum);
                    return;
                }
                else if (mobile_number.Text.Length > 13)
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.PhoneNumberMaximum);
                    return;
                }
                else if (numberRegex.IsMatch(mobile_number.Text) == false)
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.PhoneNumberOnly);
                    return;
                }
                else if (string.IsNullOrWhiteSpace(password.Text) || string.IsNullOrEmpty(password.Text))
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.PasswordRequired);
                    return;
                }
                else if (password.Text.Length < 6)
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.PasswordMinimum);
                    return;
                }
                else if (country.SelectedIndex == 0)
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.CountryRequired);
                }
                else if (state.SelectedIndex == 0)
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.StateRequired);
                }
                else if (city.SelectedIndex == 0)
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.CityRequired);
                }
                //else if (!terms.IsChecked)
                //{
                //    Config.ErrorSnackbarMessage(ValidationMessages.TermsAccept);
                //    return;
                //}
                else
                {
                    Config.ShowDialog();
                    var user = new User()
                    {
                        name = name.Text,
                        email = email.Text,
                        mobile_number = mobile_number.Text,
                        password = password.Text,
                        device_id = null,
                        country = country.Items[country.SelectedIndex],
                        state = state.Items[state.SelectedIndex],
                        city = city.Items[city.SelectedIndex],
                    };
                    if (Application.Current.Properties.ContainsKey("device_token"))
                    {
                        user.device_token = Application.Current.Properties["device_token"].ToString();
                    }
                    else
                    {
                        user.device_token = "";
                    }
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        user.device_type = "ios";
                    }
                    else if (Device.RuntimePlatform == Device.Android)
                    {
                        user.device_type = "android";
                    }
                    else
                    {
                        user.device_type = "windows";
                    }
                    _registerResponse = await _user.Register(user);
                    if (_registerResponse.status == "200")
                    {
                        Config.HideDialog();
                        //App.Current.MainPage = new MasterTemplate();
                        Config.SnackbarMessage("Proses Registrasi Berhasil, Mohon tunggu informasi approval dari kami melalui sms, setelah itu anda dapat login!");
                        Config.ShowDialog();
                        // var loginUser = new User()
                        // {
                        //     email = email.Text,
                        //     password = password.Text,
                        //     user_type = "user",
                        //     device_id = null,
                        // };

                        // if (Application.Current.Properties.ContainsKey("device_token"))
                        // {
                        //     loginUser.device_token = Application.Current.Properties["device_token"].ToString();
                        // }
                        // if (Device.RuntimePlatform == Device.iOS)
                        // {
                        //     loginUser.device_type = "ios";
                        // }
                        // else if (Device.RuntimePlatform == Device.Android)
                        // {
                        //     loginUser.device_type = "android";
                        // }
                        // else
                        // {
                        //     loginUser.device_type = "windows";
                        // }

                        // var _registerResponse = await _user.Login(loginUser);
                        // if (_registerResponse.status == "200")
                        // {
                        //     Config.HideDialog();
                        //     App.user = _registerResponse.data;
                        //     Application.Current.Properties["isLoggedIn"] = true;
                        //     Application.Current.Properties["user_id"] = _registerResponse.data.id;
                        //     Application.Current.Properties["name"] = _registerResponse.data.name;
                        //     Application.Current.Properties["mobile_number"] = _registerResponse.data.mobile_number;
                        //     Application.Current.Properties["email"] = _registerResponse.data.email;
                        //     Application.Current.Properties["profile_picture"] = _registerResponse.data.profile_picture;
                        //     Application.Current.Properties["order_message"] = _registerResponse.refer_message;
                        //     if (_registerResponse.user_address != null)
                        //     {
                        //         Application.Current.Properties["full_address"] = _registerResponse.user_address.full_address;
                        //         Application.Current.Properties["address_type"] = _registerResponse.user_address.address_type;
                        //     }
                        //     else
                        //     {
                        //         Application.Current.Properties["full_address"] = "";
                        //         Application.Current.Properties["address_type"] = "";
                        //     }
                        //     HomeVM.MyCartCounter = _registerResponse.cart_count;
                        //     HomeVM.MyFavCounter = _registerResponse.fav_count;
                        //     MessagingCenter.Send((App)Application.Current, "NavigationBar", "");

                        //     App.Current.MainPage = new MasterTemplate();
                        // }
                        // else
                        // {
                        //     Config.HideDialog();
                        //     Config.ErrorSnackbarMessage(_registerResponse.message);
                        // }
                    }
                    else
                    {
                        Config.HideDialog();
                        await DisplayAlert("Alert", _registerResponse.message, "Ok");
                    }
                    Config.HideDialog();
                }
            }
            catch (Exception ex)
            {
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        private void loginPage_Tapped(object sender, EventArgs e)
        {
            //CustomNavigationBarVM.MenuIcon = "back.png";
            //CustomNavigationBarVM.PageName = "login";
            Navigation.PushModalAsync(new LoginPage());
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
                //Navigation.PopAsync();
                Navigation.PopModalAsync();
            }
        }

        private async void country_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Config.ShowDialog();
                var picker = (Picker)sender;
                var item = (Country)picker.SelectedItem;
                if (item.id != 0)
                {
                    Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        {"country_id", item.id.ToString()}
                    };

                    var states = await UserLogic.GetState(data);
                    states.data.Insert(0, new State() { id = 0, name = "Select State" });
                    state.ItemsSource = states.data.ToList();
                    Config.HideDialog();
                    state.SelectedIndex = 0;
                    Config.HideDialog();
                }
                Config.HideDialog();
            }
            catch (Exception ex)
            {
                Config.HideDialog();
                //Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        private async void state_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Config.ShowDialog();
                var picker = (Picker)sender;
                var item = (State)picker.SelectedItem;
                if (item.id != 0)
                {
                    Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        {"state_id", item.id.ToString()}
                    };

                    var cities = await UserLogic.GetCity(data);
                    cities.data.Insert(0, new City() { id = 0, name = "Select City" });
                    city.ItemsSource = cities.data.ToList();
                    Config.HideDialog();
                    city.SelectedIndex = 0;
                    Config.HideDialog();
                }
                Config.HideDialog();
            }
            catch (Exception ex)
            {
                Config.HideDialog();
                //Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }
    }
}