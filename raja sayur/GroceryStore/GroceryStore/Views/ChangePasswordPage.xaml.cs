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
    public partial class ChangePasswordPage : ContentPage
    {
        string _pageTitle = "Change Password";
        public ChangePasswordPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
        }

        private async void changePassword_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(currentPassword.Text))
                {
                    Config.SnackbarMessage(ValidationMessages.CurrentPasswordRequired);
                }
                else if (string.IsNullOrWhiteSpace(newPassword.Text))
                {
                    Config.SnackbarMessage(ValidationMessages.NewPasswordRequired);
                }
                else if (newPassword.Text.Length < 6)
                {
                    Config.SnackbarMessage(ValidationMessages.NewPasswordMinimum);
                    return;
                }
                else if (string.IsNullOrWhiteSpace(repeatPassword.Text))
                {
                    Config.SnackbarMessage(ValidationMessages.RepeatPasswordRequired);
                }
                else if (newPassword.Text != repeatPassword.Text)
                {
                    Config.SnackbarMessage(ValidationMessages.PasswordMatch);
                }
                else
                {
                    Dictionary<string, string> valuePairs = new Dictionary<string, string>();
                    valuePairs.Add("user_id", Application.Current.Properties["user_id"].ToString());
                    valuePairs.Add("password", currentPassword.Text);
                    valuePairs.Add("new_password", newPassword.Text);

                    var response = await User.ChangePassword(valuePairs);
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
                Config.ErrorStore("ChangePasswordPage-changePassword_Clicked", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }
    }
}