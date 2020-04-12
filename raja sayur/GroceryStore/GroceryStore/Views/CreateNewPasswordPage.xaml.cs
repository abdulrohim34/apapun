using GroceryStore.Helpers;
using GroceryStore.Models;
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
    public partial class CreateNewPasswordPage : ContentPage
    {
        public ForgotPassword _OTPdata;
        public CreateNewPasswordPage(ForgotPassword otpData)
        {
            InitializeComponent();
            _OTPdata = otpData;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                otp.Text = _OTPdata.otp.ToString();
            }
            catch (Exception)
            {
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        private async void changePassword_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(password.Text))
                {
                    Config.SnackbarMessage(ValidationMessages.PasswordRequired);
                }
                else if (string.IsNullOrWhiteSpace(confirmPassword.Text))
                {
                    Config.SnackbarMessage(ValidationMessages.ConfirmPasswordRequired);
                }
                else if (password.Text != confirmPassword.Text)
                {
                    Config.SnackbarMessage(ValidationMessages.ConfirmPasswordMatch);
                }
                else if (otp.Text != _OTPdata.otp.ToString())
                {
                    Config.SnackbarMessage(ValidationMessages.OTPvalidate);
                }
                else
                {
                    Dictionary<string, string> valuePairs = new Dictionary<string, string>();
                    valuePairs.Add("mobile_number", _OTPdata.mobile_number.ToString());
                    valuePairs.Add("password", password.Text);
                    valuePairs.Add("re_password", confirmPassword.Text);

                    var response = await User.UpdatePassword(valuePairs);
                    if (response.status == 200)
                    {
                        Config.SnackbarMessage(response.message);
                        App.Current.MainPage = new MasterTemplate();
                    }
                    else
                    {
                        Config.SnackbarMessage(response.message);
                    }
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("CreateNewPasswordPage-changePassword_Clicked", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        private async void resendTap_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_OTPdata.mobile_number.ToString()))
                {
                    Config.SnackbarMessage(ValidationMessages.MobileNumberRequired);
                }
                else
                {
                    Dictionary<string, string> valuePairs = new Dictionary<string, string>();
                    valuePairs.Add("mobile_number", _OTPdata.mobile_number.ToString());
                    valuePairs.Add("user_type", "user");

                    var response = await User.ForgotPassword(valuePairs);
                    if (response.status == 200)
                    {
                        _OTPdata = response;
                        Config.SnackbarMessage(response.message);
                        //await DisplayAlert("Success", response.message, "Ok");
                        otp.Text = _OTPdata.otp.ToString();
                    }
                    else
                    {
                        Config.SnackbarMessage(response.message);
                    }
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("CreateNewPasswordPage-resendTap_Tapped", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }
    }
}